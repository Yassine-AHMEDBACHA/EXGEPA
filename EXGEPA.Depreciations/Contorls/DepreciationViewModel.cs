using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Depreciations.Core;
using EXGEPA.Depreciations.Model;
using EXGEPA.Model;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
using CORESI.Tools;

namespace EXGEPA.Depreciations.Contorls
{
    public class DepreciationViewModel : BasicExportableViewModel<Wrapper>
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IRepositoryDataProvider _RepositoryDataProvider;

        protected readonly IParameterProvider parameterProvider;

        IDataProvider<Item> ItemService { get; set; }
        ICalculator Calculator { get; set; }

        public DateEditRibbon StartDateEditRibbon { get; set; }

        public DateEditRibbon EndDateEditRibbon { get; set; }

        public ComboBoxRibbon<string> ComboBoxRibbon { get; set; }

        public RibbonButton Reports { get; set; }


        public DepreciationViewModel(ExportableView view, string pageCaption = "")
            : base(view)
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            ComboBoxRibbon = new ComboBoxRibbon<string>("Type");
            ComboBoxRibbon.ItemsSource.Add("Mensuel");
            ComboBoxRibbon.ItemsSource.Add("Journalier");
            ComboBoxRibbon.EditValue = ComboBoxRibbon.ItemsSource.FirstOrDefault();
            EndDateEditRibbon = new DateEditRibbon("Date Fin");
            StartDateEditRibbon = new DateEditRibbon("Date debut");
            Group sttingsGroup = new Group("Parametres de calcul");
            sttingsGroup.Commands.Add(StartDateEditRibbon);
            sttingsGroup.Commands.Add(EndDateEditRibbon);
            sttingsGroup.Commands.Add(ComboBoxRibbon);
            sttingsGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Simuler",
                LargeGlyph = IconProvider.Walking,
                Action = InitData
            });
            this.Groups.Insert(0, sttingsGroup);

            this.SetExportGroup(view);

            AccountingPeriodHelper accountingPeriodHelper = new AccountingPeriodHelper();
            Daily = new DailyCalculator(accountingPeriodHelper);
            Monthely = new MonthelyCalculator(accountingPeriodHelper);
            this.Caption = pageCaption;

            this.UIMessage.TryDoAction(logger, () =>
                {
                    _RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
                    ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
                    AccountingPeriod currentAccountingPeriod = accountingPeriodHelper.GetOpenPeriod();
                    StartDateEditRibbon.Date = currentAccountingPeriod.StartDate;
                    EndDateEditRibbon.Date = currentAccountingPeriod.EndDate;

                    view.DataContext = this;


                });
            string processName = parameterProvider.GetAndSetIfMissing("DepreciationReport", "simulation.exe");

            this.Reports = this.AddNewGroup().AddCommand("Etats", () => this.UIMessage.TryDoAction(logger, () => ExternalProcess.StartProcess(processName)));
            this.UIMessage.TryDoActionAsync(logger, () =>
             {
                 this.Reports.IsEnabled = Simulation.Any();
             });

        }


        public ICalculator Daily { get; set; }
        public ICalculator Monthely { get; set; }

        public override void InitData()
        {
            this.UIMessage.TryDoActionAsync(logger, () =>
            {
                logger.Info("Starting computing depreciation for periode between : " + StartDateEditRibbon.Date.ToShortDateString() + " and " + EndDateEditRibbon.Date.ToShortDateString());
                if (EndDateEditRibbon.Date <= this.StartDateEditRibbon.Date)
                {
                    throw new Exception("Parametres de caclul invalides, la date de fin de calcul doit etre antérieur par rapport au date du debut de calcul !");
                }
                ICalculator calculator;
                if (ComboBoxRibbon.EditValue == "Mensuel")
                {
                    calculator = Monthely;
                }
                else
                {
                    calculator = Daily;
                }

                this.ShowLoadingPanel = true;
                ListOfRows = null;

                Stopwatch stopwatcher = Stopwatch.StartNew();
                System.Collections.Generic.List<Item> items = ItemService.SelectAll().ToList();
                Parallel.ForEach(items, x => _RepositoryDataProvider.BindItemFields(x));
                stopwatcher.Stop();
                logger.Info("Loading Items done in : " + stopwatcher.Elapsed + " and " + items.Count + " item(s) retreived");
                logger.Info("Computing and preparing Data ...");
                stopwatcher.Restart();
                System.Collections.Generic.Dictionary<Item, System.Collections.Generic.List<Depreciation>> result = calculator.GetDepriciation(items, StartDateEditRibbon.Date, EndDateEditRibbon.Date);
                System.Collections.Generic.List<Wrapper> data = result.Where(v => v.Value.Any(d => d.StartDate >= StartDateEditRibbon.Date && d.EndDate <= EndDateEditRibbon.Date)).Select(x => new Wrapper()
                {
                    Item = x.Key,
                    Depreciations = x.Value.OrderBy(d => d.StartDate).ToList(),
                    Annuity = x.Value.Sum(d => d.Annuity),
                    PreviouseDepreciation = x.Value.Sum(d => d.PreviousDepreciation),
                    NetAccountingValue = x.Value.Last().AccountingNetValue,
                    Duration = x.Value.Sum(d => d.Period)
                }).ToList();

                if (parameterProvider.GetAndSetIfMissing("AllowDepreciationDump", true))
                {
                    if (this.ShouldSaveDepreciation())
                    {
                        Simulation.SaveDepreciation(result.SelectMany(x => x.Value));
                    }
                    else
                    {
                        this.UIMessage.Warning("Votre simulation ne concerne pas l'exercice en cours, l'archivage automatique est déscativé !");
                    }
                }


                ListOfRows = new System.Collections.ObjectModel.ObservableCollection<Wrapper>(data);
                stopwatcher.Stop();
                logger.Info("Computing done in " + stopwatcher.Elapsed);
            }, () => this.ShowLoadingPanel = false);
        }

        private bool ShouldSaveDepreciation()
        {
            return this.StartDateEditRibbon.Date.Day >= 1 && StartDateEditRibbon.Date.Month >= 1
                && this.EndDateEditRibbon.Date.Day <= 31 && EndDateEditRibbon.Date.Month <= 12 &&
                (this.StartDateEditRibbon.Date.Year - this.EndDateEditRibbon.Date.Year) == 0;

        }
    }
}