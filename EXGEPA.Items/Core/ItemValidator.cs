using System.Linq;
using System.Text;
using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Model;

namespace EXGEPA.Items.Core
{
    public static class ItemValidator
    {   
        static ItemValidator()
        {
            // AccountingPeriodService = ServiceLocator.Resolve<IDataProvider<AccountingPeriod>>();
        }

        public static string CheckItem(Item item)
        {
            //if (item.AccountingPeriod == null)
            //{
            //    item.AccountingPeriod = AccountingPeriodService.SelectAll().FirstOrDefault(x => x?.Approved == false);
            //}

            var stringBuilder = new StringBuilder();
            if (CheckGeneralAccounts(item))
            {
                stringBuilder.AppendLine("Le cpt general ne doit pas etre vide");
            }

            if (item.Reference == null)
            {
                stringBuilder.AppendLine("La reference ne doit pas etre vide");
            }
            if (item.Office == null)
            {
                stringBuilder.AppendLine("La localisation ne doit pas etre vide");
            }

            //if (item.AquisitionDate < item.AccountingPeriod.StartDate)
            //{
            //    stringBuilder.AppendLine("La date d'aquisition ne doit pas etre antrieure au debut d'exercice : " + item.AccountingPeriod.StartDate.ToShortDateString());
            //}

            var result = stringBuilder.ToString();
            if (result != null)
            {
                result = "Veuillez corriger les champs ci dessous avant de continuer \t\n" + result;
            }

            return result;

        }

        private static bool CheckGeneralAccounts(Item item)
        {
            return item.GeneralAccount != null;
        }
    }
}
