using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Core.Database;
using EXGEPA.Core.Tools;
using EXGEPA.Model;
using EXGEPA.Transfert.Core.RepositoryHelper;

namespace EXGEPA.Transfert.Core
{
    internal class Loader
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Load()
        {
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            var dataBaseTransfertEngin = new DataBaseTransfertEngin();
            var companyName = dataBaseTransfertEngin.GetCompanyName();
            parameterProvider.TrySetOrAdd("CompanyName", companyName);

            logger.Info("Loading Regions");
            var regions = DbInitializer.InsertData(dataBaseTransfertEngin.LoadRegion());

            logger.Info("Loading ReceiveOrders");
            var ListOfReceiveOrder = DbInitializer.InsertData(dataBaseTransfertEngin.LoadReceiveOrder());

            logger.Info("Loading InputSheet");
            var ListOfInputSheet = DbInitializer.InsertData(dataBaseTransfertEngin.LoadInputSheet());

            logger.Info("Loading Providers");
            var ListOfProvider = DbInitializer.InsertData(dataBaseTransfertEngin.LoadProviders());

            logger.Info("Loading Persons");
            var ListOfPerson = DbInitializer.InsertData(dataBaseTransfertEngin.LoadPeron());

            logger.Info("Loading OrderDocumentTypes");
            var listOfOrderDocumentTypes = DbInitializer.InsertData(dataBaseTransfertEngin.LoadOrderDocumentTypes());


            logger.Info("loading GeneralAccountType");
            var generalAccountTypes = DbInitializer.InsertData(dataBaseTransfertEngin.ListOfGeneralAccountType());


            logger.Info("Loading GeneralAccount");


            Func<GeneralAccount, GeneralAccount> gAMapper = (ga) =>
            {
                ga.GeneralAccountType = generalAccountTypes.Single(x => x.Type == ga.GeneralAccountType.Type);
                return ga;
            };

            var ListOfGeneralAccount = dataBaseTransfertEngin.LoadGeneralAccount();

            var endowment = DbInitializer.InsertInstances(ListOfGeneralAccount.Select(x => x.Children?.Children).Where(x => x != null).GroupBy(x => x.Key).Select(x => x.First()).Select(gAMapper));
            foreach (var item in ListOfGeneralAccount.Select(x => x.Children).Where(x => x != null).GroupBy(x => x.Key).Select(x => x.First()).Select(gAMapper))
            {
                item.Children = endowment.FirstOrDefault(x => x.Key == item.Children?.Key);
                DbInitializer.InsertInstance(item);
            }

            DbInitializer.InsertInstances(ListOfGeneralAccount.Where(x => x != null).GroupBy(x => x.Key).Select(x => x.First()).Select(gAMapper));

            logger.Info("Loading Transfert Order");
            var ListOfTransferOrder = DbInitializer.InsertData(dataBaseTransfertEngin.loadTransferOrder());

            logger.Info("Loading Currencies");
            var ListOfAllCurrencies = DbInitializer.InsertData(CurrencyLoader.LoadCurrencies());

            logger.Info("Loading Invoices");
            var ListOfInvoice = dataBaseTransfertEngin.LoadInvoices();
            ListOfInvoice.ForEach(invoice =>
            {
                invoice.InputSheet = ListOfInputSheet.FirstOrDefault(inputsheet => inputsheet.Key == invoice.InputSheet.Key);
                invoice.Provider = ListOfProvider.FirstOrDefault(provider => provider.Key == invoice.Provider.Key);

            });
            ListOfInvoice = DbInitializer.InsertData(ListOfInvoice);

            logger.Info("loading references type");
            var listOfRefenreceType = DbInitializer.InsertData(dataBaseTransfertEngin.LoadReferenceType());

            logger.Info("Loading References");
            var ListOfReference = dataBaseTransfertEngin.LoadReferences();
            ListOfReference.ForEach(reference =>
            {
                reference.InvestmentAccount = ListOfGeneralAccount.FirstOrDefault(ga => ga.Key == reference.InvestmentAccount?.Key);
                reference.ChargeAccount = ListOfGeneralAccount.FirstOrDefault(ga => ga.Key == reference.ChargeAccount?.Key);
                reference.ReferenceType = listOfRefenreceType.FirstOrDefault(rt => rt.Key == reference.ReferenceType?.Key);
            });
            ListOfReference = DbInitializer.InsertData(ListOfReference);

            logger.Info("Loading Analytical Account types");
            var AnalyticalAccountTypes = DbInitializer.InsertData(dataBaseTransfertEngin.LoadAnalyticalAccountTypes());

            logger.Info("Loading Analytical Account");
            var ListOfAnalyticalAccount = dataBaseTransfertEngin.LoadAnalyticalAccount();
            ListOfAnalyticalAccount.ForEach(a =>
            {
                a.AnalyticalAccountType = AnalyticalAccountTypes.FirstOrDefault(x => x.Key == a.AnalyticalAccountType.Key);
            });
            ListOfAnalyticalAccount = DbInitializer.InsertData(ListOfAnalyticalAccount);

            logger.Info("Loading sites");
            var sites = LoadSites(dataBaseTransfertEngin, regions);

            logger.Info("Loading buildings");
            var buildings = LoadBuildings(dataBaseTransfertEngin, sites);

            logger.Info("Loading levels");
            var levels = LoadLevels(dataBaseTransfertEngin, buildings);

            logger.Info("Loading offices");
            var offices = LoadOffices(dataBaseTransfertEngin, ListOfAnalyticalAccount, levels);

            logger.Info("Loading reformes certificates");
            var ListOfReformeCertificate = DbInitializer.InsertData(dataBaseTransfertEngin.LoadReformsCertificate());

            logger.Info("Loading output certificates");
            var ListOfOutputCertificate = DbInitializer.InsertData(dataBaseTransfertEngin.LoadOutputCertificate());

            logger.Info("Loading States");
            var states = dataBaseTransfertEngin.LoadStates();
            if (states != null && states.Count == 0)
            {
                states = new List<ItemState>();
                states.Add(new ItemState() { Key = "Neuf" });
                states.Add(new ItemState() { Key = "Bon état" });
                states.Add(new ItemState() { Key = "Détériorer" });
            }
            states = DbInitializer.InsertData(states);

            logger.Info("Loading inventory");
            var ListOfinventory = dataBaseTransfertEngin.loadInventory();
            ListOfinventory.ForEach(row =>
            {
                row.ItemState = states.FirstOrDefault(state => state.Key == row.ItemState?.Key);
            });
            ListOfinventory = DbInitializer.InsertData(ListOfinventory);

            logger.Info("Loading Periods");
            var periods = dataBaseTransfertEngin.LoadAccountingPeriods();
            periods.Last().Approved = false;
            periods = DbInitializer.InsertData(periods);

            logger.Info("Loading Items");
            var assignements = DbInitializer.InsertData(dataBaseTransfertEngin.LoadAssignament());

            var stopwatch = Stopwatch.StartNew();
            var Items = dataBaseTransfertEngin.LoadItem();
            stopwatch.Stop();
            logger.Info(stopwatch.Elapsed + " + to Load Items");
            stopwatch.Restart();

            var allPersons = ListOfPerson.ToDictionary(x => x.Key);
            var allInvoices = ListOfInvoice.ToDictionary(x => x.Key);
            var allOffices = offices.ToDictionary(x => x.Key.ToUpper());
            var allProviders = ListOfProvider.ToDictionary(x => x.Key);
            var allGA = ListOfGeneralAccount.ToDictionary(x => x.Key);
            var CodeLength = 15;
            parameterProvider.TrySetOrAdd($"{typeof(Item).Name}KeyLength", CodeLength);
            // parameterProvider.TrySetOrAdd("CodeLength", CodeLength);
            Parallel.ForEach(Items, item =>
            {
                item.Key = KeyLengthNormalizer.Normalize(item.Key, CodeLength);
                Assignment assig = assignements.FirstOrDefault(x => x.Key == item.Key);
                if (assig != null)
                {
                    allPersons.TryGetValue(assig.Person.Key, out Person person);
                    item.Person = person;
                    item.UserAssignmentStartDate = assig.StartDate;
                }
                if (allOffices.TryGetValue(item.Office.Key.ToUpper(), out Office office))
                {
                    item.Office = office;
                }
                else
                {
                    throw new Exception($"Item office doesn't existe : {item.Office.Key.ToUpper()}");
                }

                if (item.AnalyticalAccount != null)
                    item.AnalyticalAccount = ListOfAnalyticalAccount.FirstOrDefault(account => account.Key == item.AnalyticalAccount.Key);

                if (item.GeneralAccount != null)
                    item.GeneralAccount = ListOfGeneralAccount.FirstOrDefault(account => account.Key == item.GeneralAccount.Key);


                if (item.Reference != null)
                    item.Reference = ListOfReference.FirstOrDefault(reference => reference.Key == item.Reference.Key);


                if (item.Invoice != null)
                {
                    allInvoices.TryGetValue(item.Invoice.Key, out Invoice invoice);
                    item.Invoice = invoice;
                }

                if (item.Provider != null)
                {
                    allProviders.TryGetValue(item.Provider.Key, out Provider provider);
                    item.Provider = provider;
                }

                if (item.InputSheet != null)
                    item.InputSheet = ListOfInputSheet.FirstOrDefault(inputSheet => inputSheet.Key == item.InputSheet.Key);

                if (item.ReceiveOrder != null)
                    item.ReceiveOrder = ListOfReceiveOrder.FirstOrDefault(receiveOrder => receiveOrder.Key == item.ReceiveOrder.Key);

                if (item.TransferOrder != null)
                    item.TransferOrder = ListOfTransferOrder.FirstOrDefault(transferOrder => transferOrder.Key == item.TransferOrder.Key);

                if (item.UserAssignmentStartDate != null)
                    item.UserAssignmentStartDate = item.OfficeAssignmentStartDate;

                if (item.ReformeCertificate != null)
                    item.ReformeCertificate = ListOfReformeCertificate.FirstOrDefault(pv => pv.Key == item.ReformeCertificate.Key);

                if (item.OutputCertificate != null)
                {
                    item.OutputCertificate = ListOfOutputCertificate.FirstOrDefault(pv => pv.Key == item.OutputCertificate.Key);
                }

                if (item.ItemState != null)
                {
                    item.ItemState = states.FirstOrDefault(stat => stat.Key == item.ItemState.Key);
                }
                else
                {
                    item.ItemState = states.FirstOrDefault(stat => stat.Key == "BON");
                }
                if (item.AccountingPeriod != null)
                {
                    item.AccountingPeriod = periods.FirstOrDefault(x => x.StartDate <= item.AquisitionDate && x.EndDate >= item.AquisitionDate);
                }
            });
            stopwatch.Stop();
            logger.Info(stopwatch.Elapsed + " + to maps Items");
            stopwatch.Restart();
            DbInitializer.JustInsertData(Items.OrderBy(x => x.Key));
            logger.Info(stopwatch.Elapsed + " + to save Items");
        }

        private static List<Office> LoadOffices(IDataBaseTransfertEngin dataBaseTransfertEngin, List<AnalyticalAccount> ListOfAnalyticalAccount, List<Level> levels)
        {
            var probleme = new List<Office>();
            var offices = dataBaseTransfertEngin.LoadOffices().GroupBy(x => x.Key).Select(y => y.First()).ToList();
            foreach (var office in offices)
            {
                var levelSubKey = $"{office.Level.Building.Site.Code}{office.Level.Building.Code}{office.Level.Code }";
                var level = levels.FirstOrDefault(l => levelSubKey == l.Key.Substring(3));
                if (level == null)
                {
                    logger.Warn("Bureau avec code niveau non valide " + office.Code);
                    probleme.Add(office);
                }
                else
                {
                    office.Level = level;
                    if (office.AnalyticalAccount != null)
                        office.AnalyticalAccount = ListOfAnalyticalAccount.FirstOrDefault(a => a.Key == office.AnalyticalAccount.Key);
                }
            }
            return DbInitializer.InsertData(offices);
        }

        private static List<Level> LoadLevels(IDataBaseTransfertEngin dataBaseTransfertEngin, List<Building> buildings)
        {
            var levels = dataBaseTransfertEngin.LoadLevels();
            levels.ForEach(level =>
            {
                var build = buildings.FirstOrDefault(building => $"{level.Building.Site.Code}{level.Building.Code}" == building.Key.Substring(3));

                if (build != null)
                {
                    level.Building = build;
                    level.Key = level.Building.Key + level.Code;
                }
                else
                    throw new Exception($"Can't find building for level, Building code = {level.Building.Site.Code}, site code = {level.Building.Code}");
            });
            return DbInitializer.InsertData(levels);
        }

        private static List<Building> LoadBuildings(IDataBaseTransfertEngin dataBaseTransfertEngin, List<Site> sites)
        {
            var buildings = dataBaseTransfertEngin.LoadBuildings();
            buildings.ForEach(building =>
            {
                building.Site = sites.FirstOrDefault(site => site.Code == building.Site.Code);
                building.Key = building.Site.Key + building.Code;
            });
            return DbInitializer.InsertData(buildings);
        }

        private static List<Site> LoadSites(IDataBaseTransfertEngin dataBaseTransfertEngin, List<Region> regions)
        {
            logger.Info("Loading Sites");
            var siteService = ServiceLocator.Resolve<IDataProvider<Site>>();
            var sites = dataBaseTransfertEngin.LoadSites();
            sites.ForEach(site =>
            {
                site.Region = regions.FirstOrDefault(region => region.Key == site.Region.Key);
                if (site.Region == null)
                {
                    site.Region = regions.FirstOrDefault();
                }
                site.Key = site.Region.Key + site.Key;
                if (site.Region == null)
                {
                    site.Region = regions[0];
                }

            });
            return DbInitializer.InsertData(sites);
        }


    }
}
