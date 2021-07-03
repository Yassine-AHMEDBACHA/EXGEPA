using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Depreciations.Core;
using EXGEPA.Depreciations.Model;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Contorls
{
    public class DepreciationViewModel : BasicExportableViewModel<Wrapper>
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IRepositoryDataProvider repositoryDataProvider;

        protected readonly IParameterProvider parameterProvider;

        IDataProvider<Item> ItemService { get; set; }

        private readonly List<OutputType> outputTypeToIgnore;


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
            var sttingsGroup = new Group("Parametres de calcul");
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
                    this.repositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
                    this.ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
                    var currentAccountingPeriod = accountingPeriodHelper.GetOpenPeriod();
                    this.StartDateEditRibbon.Date = currentAccountingPeriod.StartDate;
                    this.EndDateEditRibbon.Date = currentAccountingPeriod.EndDate;
                    view.DataContext = this;
                });

            string processName = parameterProvider.TryGet("DepreciationReport", "simulation.exe");

            this.Reports = this.AddNewGroup().AddCommand("Etats", () => this.UIMessage.TryDoAction(logger, () => ExternalProcess.StartProcess(processName)));
            this.UIMessage.TryDoActionAsync(logger, () =>
             {
                 this.Reports.IsEnabled = Simulation.Any();
             });

            this.outputTypeToIgnore = new List<OutputType> { OutputType.Cession, OutputType.Destruction, OutputType.Disparition, OutputType.Vente };
        }

        public DateEditRibbon StartDateEditRibbon { get; set; }

        public DateEditRibbon EndDateEditRibbon { get; set; }

        public ICalculator Daily { get; set; }
        public ICalculator Monthely { get; set; }

        public override void InitData()
        {
            this.UIMessage.TryDoActionAsync(logger, () =>
            {
                logger.Info("Starting computing depreciation for periode between : " + StartDateEditRibbon.Date.ToShortDateString() + " and " + EndDateEditRibbon.Date.ToShortDateString());
                if (EndDateEditRibbon.Date <= this.StartDateEditRibbon.Date)
                {
                    throw new Exception("Parametres de caclul invalides, la date de fin de calcul doit etre antérieur à la date de debut du calcul !");
                }

                this.ShowLoadingPanel = true;
                ICalculator calculator = ComboBoxRibbon.EditValue == "Mensuel" ? Monthely : Daily;
                ListOfRows = null;

                var stopwatcher = Stopwatch.StartNew();
                var items = ItemService.All;
                this.repositoryDataProvider.BindPropertyAndSetExtended(items);
                if (items.Select(x => x.Invoice).Any(i => i != null && i.Date <= StartDateEditRibbon.Date))
                {
                    this.UIMessage.Warning("Des factures non validées font partie de votre simulation !");
                }

                stopwatcher.Stop();
                logger.Info("Loading Items done in : " + stopwatcher.Elapsed + " and " + items.Count + " item(s) retreived");
                logger.Info("Computing and preparing Data ...");
                stopwatcher.Restart();
                var result = calculator.GetDepriciation(items, StartDateEditRibbon.Date, EndDateEditRibbon.Date);
                var data = result.Where(v => v.Value.Any(d => d.StartDate >= StartDateEditRibbon.Date && d.EndDate <= EndDateEditRibbon.Date)).Select(x => new Wrapper()
                {
                    Item = x.Key,
                    Depreciations = x.Value.OrderBy(d => d.StartDate).ToList(),
                    Annuity = x.Value.Sum(d => d.Annuity),
                    PreviouseDepreciation = x.Value.Sum(d => d.PreviousDepreciation),
                    NetAccountingValue = x.Value.Last().AccountingNetValue,
                    Duration = x.Value.Sum(d => d.Period)
                }).ToList();

                if (parameterProvider.TryGet("AllowDepreciationDump", true))
                {
                    if (this.ShouldSaveDepreciation())
                    {
                        var DepToSave = result.SelectMany(x => x.Value);
                        var accountingPeriod = DepToSave.FirstOrDefault()?.AccountingPeriod;
                        var oldItems = GetOldItems(items, accountingPeriod);
                        var OnlyActiveItems = DepToSave.Union(oldItems).Where(ShouldBeSaved);
                        Simulation.SaveDepreciation(OnlyActiveItems);
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

        private bool ShouldBeSaved(Depreciation depreciation)
        {
            if (depreciation.Item.OutputCertificate != null)
            {
                var outputCertificate = depreciation.Item.OutputCertificate;
                if (this.outputTypeToIgnore.Contains(outputCertificate.OutputType) && outputCertificate.Date < StartDateEditRibbon.Date)
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<Depreciation> GetOldItems(IList<Item> items, AccountingPeriod accountingPeriod)
        {
            var second = items.Where(x => x.LimiteDate < StartDateEditRibbon.Date).Select(x => new Depreciation
            {
                AccountingNetValue = x.FiscalRate == 0 ? x.Amount : 0,
                AccountingPeriod = accountingPeriod,
                Annuity = 0,
                InitialValue = x.Amount,
                PreviousDepreciation = x.FiscalRate == 0 ? 0 : x.Amount,
                Item = x,
                StartDate = StartDateEditRibbon.Date,
                EndDate = EndDateEditRibbon.Date,
                Rate = x.FiscalRate,
                DepreciationType = DepreciationType.LinearDepreciation
            });

            return second;
        }

        private bool ShouldSaveDepreciation()
        {
            return this.StartDateEditRibbon.Date.Day >= 1 && StartDateEditRibbon.Date.Month >= 1
                && this.EndDateEditRibbon.Date.Day <= 31 && EndDateEditRibbon.Date.Month <= 12 &&
                (this.StartDateEditRibbon.Date.Year - this.EndDateEditRibbon.Date.Year) == 0;
        }
    }
}