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
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            DataBaseTransfertEngin dataBaseTransfertEngin = new DataBaseTransfertEngin();
            string companyName = dataBaseTransfertEngin.GetCompanyName();
            parameterProvider.TrySetOrAdd("CompanyName", companyName);

            logger.Info("Loading Regions");
            List<Region> regions = DbInitializer.InsertData(dataBaseTransfertEngin.LoadRegion());

            logger.Info("Loading ReceiveOrders");
            List<ReceiveOrder> ListOfReceiveOrder = DbInitializer.InsertData(dataBaseTransfertEngin.LoadReceiveOrder());

            logger.Info("Loading InputSheet");
            List<InputSheet> ListOfInputSheet = DbInitializer.InsertData(dataBaseTransfertEngin.LoadInputSheet());

            logger.Info("Loading Providers");
            List<Provider> ListOfProvider = DbInitializer.InsertData(dataBaseTransfertEngin.LoadProviders());

            logger.Info("Loading Persons");
            List<Person> ListOfPerson = DbInitializer.InsertData(dataBaseTransfertEngin.LoadPeron());

            logger.Info("Loading OrderDocumentTypes");
            List<OrderDocumentType> listOfOrderDocumentTypes = DbInitializer.InsertData(dataBaseTransfertEngin.LoadOrderDocumentTypes());


            logger.Info("loading GeneralAccountType");
            List<GeneralAccountType> generalAccountTypes = DbInitializer.InsertData(dataBaseTransfertEngin.ListOfGeneralAccountType());


            logger.Info("Loading GeneralAccount");


            Func<GeneralAccount, GeneralAccount> gAMapper = (ga) =>
            {
                ga.GeneralAccountType = generalAccountTypes.Single(x => x.Type == ga.GeneralAccountType.Type);
                return ga;
            };

            List<GeneralAccount> ListOfGeneralAccount = dataBaseTransfertEngin.LoadGeneralAccount();

            IEnumerable<GeneralAccount> endowment = DbInitializer.InsertInstances(ListOfGeneralAccount.Select(x => x.Children?.Children).Where(x => x != null).GroupBy(x => x.Key).Select(x => x.First()).Select(gAMapper));
            foreach (GeneralAccount item in ListOfGeneralAccount.Select(x => x.Children).Where(x => x != null).GroupBy(x => x.Key).Select(x => x.First()).Select(gAMapper))
            {
                item.Children = endowment.FirstOrDefault(x => x.Key == item.Children?.Key);
                DbInitializer.InsertInstance(item);
            }

            DbInitializer.InsertInstances(ListOfGeneralAccount.Where(x => x != null).GroupBy(x => x.Key).Select(x => x.First()).Select(gAMapper));

            logger.Info("Loading Transfert Order");
            List<TransferOrder> ListOfTransferOrder = DbInitializer.InsertData(dataBaseTransfertEngin.loadTransferOrder());

            logger.Info("Loading Currencies");
            List<Currency> ListOfAllCurrencies = DbInitializer.InsertData(CurrencyLoader.LoadCurrencies());

            logger.Info("Loading Invoices");
            List<Invoice> ListOfInvoice = dataBaseTransfertEngin.LoadInvoices();
            ListOfInvoice.ForEach(invoice =>
            {
                invoice.InputSheet = ListOfInputSheet.FirstOrDefault(inputsheet => inputsheet.Key == invoice.InputSheet.Key);
                invoice.Provider = ListOfProvider.FirstOrDefault(provider => provider.Key == invoice.Provider.Key);

            });
            ListOfInvoice = DbInitializer.InsertData(ListOfInvoice);

            logger.Info("loading references type");
            List<ReferenceType> listOfRefenreceType = DbInitializer.InsertData(dataBaseTransfertEngin.LoadReferenceType());

            logger.Info("Loading References");
            List<Reference> ListOfReference = dataBaseTransfertEngin.LoadReferences();
            ListOfReference.ForEach(reference =>
            {
                reference.InvestmentAccount = ListOfGeneralAccount.FirstOrDefault(ga => ga.Key == reference.InvestmentAccount?.Key);
                reference.ChargeAccount = ListOfGeneralAccount.FirstOrDefault(ga => ga.Key == reference.ChargeAccount?.Key);
                reference.ReferenceType = listOfRefenreceType.FirstOrDefault(rt => rt.Key == reference.ReferenceType?.Key);
            });
            ListOfReference = DbInitializer.InsertData(ListOfReference);

            logger.Info("Loading Analytical Account types");
            List<AnalyticalAccountType> AnalyticalAccountTypes = DbInitializer.InsertData(dataBaseTransfertEngin.LoadAnalyticalAccountTypes());

            logger.Info("Loading Analytical Account");
            List<AnalyticalAccount> ListOfAnalyticalAccount = dataBaseTransfertEngin.LoadAnalyticalAccount();
            ListOfAnalyticalAccount.ForEach(a =>
            {
                a.AnalyticalAccountType = AnalyticalAccountTypes.FirstOrDefault(x => x.Key == a.AnalyticalAccountType.Key);
            });
            ListOfAnalyticalAccount = DbInitializer.InsertData(ListOfAnalyticalAccount);

            logger.Info("Loading sites");
            List<Site> sites = LoadSites(dataBaseTransfertEngin, regions);

            logger.Info("Loading buildings");
            List<Building> buildings = LoadBuildings(dataBaseTransfertEngin, sites);

            logger.Info("Loading levels");
            List<Level> levels = LoadLevels(dataBaseTransfertEngin, buildings);

            logger.Info("Loading offices");
            List<Office> offices = LoadOffices(dataBaseTransfertEngin, ListOfAnalyticalAccount, levels);

            logger.Info("Loading reformes certificates");
            List<ReformeCertificate> ListOfReformeCertificate = DbInitializer.InsertData(dataBaseTransfertEngin.LoadReformsCertificate());

            logger.Info("Loading output certificates");
            List<OutputCertificate> ListOfOutputCertificate = DbInitializer.InsertData(dataBaseTransfertEngin.LoadOutputCertificate());

            logger.Info("Loading States");
            List<ItemState> states = dataBaseTransfertEngin.LoadStates();
            if (states != null && states.Count == 0)
            {
                states = new List<ItemState>();
                states.Add(new ItemState() { Key = "Neuf" });
                states.Add(new ItemState() { Key = "Bon état" });
                states.Add(new ItemState() { Key = "Détériorer" });
            }
            states = DbInitializer.InsertData(states);

            logger.Info("Loading inventory");
            List<InventoryRow> ListOfinventory = dataBaseTransfertEngin.loadInventory();
            ListOfinventory.ForEach(row =>
            {
                row.ItemState = states.FirstOrDefault(state => state.Key == row.ItemState?.Key);
            });
            ListOfinventory = DbInitializer.InsertData(ListOfinventory);

            logger.Info("Loading Periods");
            List<AccountingPeriod> periods = dataBaseTransfertEngin.LoadAccountingPeriods();
            periods.Last().Approved = false;
            periods = DbInitializer.InsertData(periods);

            logger.Info("Loading Items");
            List<Assignment> assignements = DbInitializer.InsertData(dataBaseTransfertEngin.LoadAssignament());

            Stopwatch stopwatch = Stopwatch.StartNew();
            List<Item> Items = dataBaseTransfertEngin.LoadItem();
            stopwatch.Stop();
            logger.Info(stopwatch.Elapsed + " + to Load Items");
            stopwatch.Restart();

            Dictionary<string, Person> allPersons = ListOfPerson.ToDictionary(x => x.Key);
            Dictionary<string, Invoice> allInvoices = ListOfInvoice.ToDictionary(x => x.Key);
            Dictionary<string, Office> allOffices = offices.ToDictionary(x => x.Key.ToUpper());
            Dictionary<string, Provider> allProviders = ListOfProvider.ToDictionary(x => x.Key);
            Dictionary<string, GeneralAccount> allGA = ListOfGeneralAccount.ToDictionary(x => x.Key);
            int CodeLength = 15;
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
            List<Office> probleme = new List<Office>();
            List<Office> offices = dataBaseTransfertEngin.LoadOffices().GroupBy(x => x.Key).Select(y => y.First()).ToList();
            foreach (Office office in offices)
            {
                string levelSubKey = $"{office.Level.Building.Site.Code}{office.Level.Building.Code}{office.Level.Code }";
                Level level = levels.FirstOrDefault(l => levelSubKey == l.Key.Substring(3));
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
            List<Level> levels = dataBaseTransfertEngin.LoadLevels();
            levels.ForEach(level =>
            {
                Building build = buildings.FirstOrDefault(building => $"{level.Building.Site.Code}{level.Building.Code}" == building.Key.Substring(3));

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
            List<Building> buildings = dataBaseTransfertEngin.LoadBuildings();
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
            IDataProvider<Site> siteService = ServiceLocator.Resolve<IDataProvider<Site>>();
            List<Site> sites = dataBaseTransfertEngin.LoadSites();
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
