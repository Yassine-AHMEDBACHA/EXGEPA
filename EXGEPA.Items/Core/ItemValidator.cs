using System.Linq;
using System.Text;
using CORESI.Data;
using CORESI.Data.Tools;
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

            var result = stringBuilder.ToString();
            if (result.IsValid())
            {
                result = "Veuillez corriger les champs ci dessous avant de continuer \t\n" + result;
            }

            return null;
        }

        private static bool CheckGeneralAccounts(Item item)
        {
            return item.GeneralAccount != null;
        }
    }
}
