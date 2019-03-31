using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Model;

namespace EXGEPA.Items.Core
{
    public static class ItemValidator
    {
        private static IDataProvider<AccountingPeriod> AccountingPeriodService { get; set; }
        static ItemValidator()
        {
            AccountingPeriodService = ServiceLocator.Resolve<IDataProvider<AccountingPeriod>>();
        }

        public static string CheckItem(Item item)
        {

            string result = null;
            if (CheckGeneralAccounts(item))
            {
                result += "\tLes cpt genereaux ne doivent pas etre vide\t\n";
            }

            if (item.Reference == null)
                result += "\tLa reference ne doit pas etre vide\t\n";
            if (item.Office == null)
                result += "\tLa localisation ne doit pas etre vide\t\n";
            //if(item.AccountingPeriod ==null)
            //     item.AccountingPeriod = AccountingPeriodService.SelectAll().FirstOrDefault(x => x.Approved == false);
            //if (item.AquisitionDate < item.AccountingPeriod.StartDate)
            //    result = result + "\tDate d'aquisition ne doit pas etre antrieure au debut d'exercice : " + item.AccountingPeriod.StartDate.ToShortDateString() + "\t\n";
            if (result != null)
                result = "Veuillez corriger les champs ci dessous avant de continuer \t\n" + result;
            return result;

        }

        private static bool CheckGeneralAccounts(Item item)
        {
            return false;
        }
    }
}
