﻿using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
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

        public DateEditRibbon startDateEditRibbon { get; set; }
        public DateEditRibbon endDateEditRibbon { get; set; }
        public ComboBoxRibbon<string> comboBoxRibbon { get; set; }

        public RibbonButton Reports { get; set; }


        public DepreciationViewModel(ExportableView view, string pageCaption = "")
            : base(view)
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            comboBoxRibbon = new ComboBoxRibbon<string>("Type");
            comboBoxRibbon.ItemsSource.Add("Mensuel");
            comboBoxRibbon.ItemsSource.Add("Journalier");
            comboBoxRibbon.EditValue = comboBoxRibbon.ItemsSource.FirstOrDefault();
            endDateEditRibbon = new DateEditRibbon("Date Fin");
            startDateEditRibbon = new DateEditRibbon("Date debut");
            var sttingsGroup = new Group("Parametres de calcul");
            sttingsGroup.Commands.Add(startDateEditRibbon);
            sttingsGroup.Commands.Add(endDateEditRibbon);
            sttingsGroup.Commands.Add(comboBoxRibbon);
            sttingsGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Simuler",
                LargeGlyph = IconProvider.Walking,
                Action = InitData
            });
            this.Groups.Insert(0, sttingsGroup);

            this.SetExportGroup(view);

            var accountingPeriodHelper = new AccountingPeriodHelper();
            Daily = new DailyCalculator(accountingPeriodHelper);
            Monthely = new MonthelyCalculator(accountingPeriodHelper);
            this.Caption = pageCaption;

            this.uIMessage.TryDoAction(logger, () =>
                {
                    _RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
                    ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
                    var currentAccountingPeriod = accountingPeriodHelper.GetOpenPeriod();
                    startDateEditRibbon.Date = currentAccountingPeriod.StartDate;
                    endDateEditRibbon.Date = currentAccountingPeriod.EndDate;

                    view.DataContext = this;


                });
            var processName = parameterProvider.GetAndSetIfMissing("DepreciationReport", "simulation.exe");

            this.Reports = this.AddNewGroup().AddCommand("Etats", () => this.uIMessage.TryDoAction(logger, ()=> ExternalProcess.StartProcess(processName)));
            this.uIMessage.TryDoActionAsync(logger, () =>
             {
                 this.Reports.IsEnabled = Simulation.Any();
             });

        }


        public ICalculator Daily { get; set; }
        public ICalculator Monthely { get; set; }

        public override void InitData()
        {
            this.uIMessage.TryDoActionAsync(logger, () =>
            {
                logger.Info("Starting computing depreciation for periode between : " + startDateEditRibbon.Date.ToShortDateString() + " and " + endDateEditRibbon.Date.ToShortDateString());
                if (endDateEditRibbon.Date <= this.startDateEditRibbon.Date)
                {
                    throw new Exception("Parametres de caclul invalides, la date de fin de calcul doit etre antérieur par rapport au date du debut de calcul !");
                }
                ICalculator calculator;
                if (comboBoxRibbon.EditValue == "Mensuel")
                {
                    calculator = Monthely;
                }
                else
                {
                    calculator = Daily;
                }

                this.ShowLoadingPanel = true;
                ListOfRows = null;

                var stopwatcher = Stopwatch.StartNew();
                var items = ItemService.SelectAll().ToList();
                Parallel.ForEach(items, x => _RepositoryDataProvider.BindItemFields(x));
                stopwatcher.Stop();
                logger.Info("Loading Items done in : " + stopwatcher.Elapsed + " and " + items.Count + " item(s) retreived");
                logger.Info("Computing and preparing Data ...");
                stopwatcher.Restart();
                var result = calculator.GetDepriciation(items, startDateEditRibbon.Date, endDateEditRibbon.Date);
                var data = result.Where(v => v.Value.Any(d => d.StartDate >= startDateEditRibbon.Date && d.EndDate <= endDateEditRibbon.Date)).Select(x => new Wrapper()
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
                        this.uIMessage.Warning("Votre simulation ne concerne pas l'exercice en cours, l'archivage automatique est déscativé !");
                    }
                }


                ListOfRows = new System.Collections.ObjectModel.ObservableCollection<Wrapper>(data);
                stopwatcher.Stop();
                logger.Info("Computing done in " + stopwatcher.Elapsed);
            }, () => this.ShowLoadingPanel = false);
        }

        private bool ShouldSaveDepreciation()
        {
            return this.startDateEditRibbon.Date.Day >= 1 && startDateEditRibbon.Date.Month >= 1
                && this.endDateEditRibbon.Date.Day <= 31 && endDateEditRibbon.Date.Month <= 12 &&
                (this.startDateEditRibbon.Date.Year - this.endDateEditRibbon.Date.Year) == 0;

        }
    }
}