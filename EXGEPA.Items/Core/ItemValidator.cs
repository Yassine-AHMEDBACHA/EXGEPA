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

        }

        public static string CheckItem(Item item, bool requireInvoice)
        {
            var stringBuilder = new StringBuilder();
            if (requireInvoice)
            {
                ServiceLocator.Resolve(out IParameterProvider parameterProvider);
                requireInvoice = parameterProvider.TryGet("requireInvoice", false);


                if (requireInvoice && item.Invoice == null && item.TransferOrder == null)
                {
                    stringBuilder.AppendLine(@"La facture / bon de transfert ne doit pas étre vide");
                }
            }

            if (item.GeneralAccount == null)
            {
                stringBuilder.AppendLine("Le compte general ne doit pas étre vide");
            }

            if (item.Reference == null)
            {
                stringBuilder.AppendLine("La reference ne doit pas étre vide");
            }
            if (item.Office == null)
            {
                stringBuilder.AppendLine("La localisation ne doit pas étre vide");
            }

            var result = stringBuilder.ToString();
            if (result.IsValid())
            {
                result = "Veuillez corriger les champs ci dessous avant de continuer \t\n" + result;
                return result;
            }

            return null;
        }
    }
}
