using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.Tools.StringTools;
using CORESI.WPF;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EXGEPA.Saidal.Core
{
    public class AquisitionSerializer
    {
        protected readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IParameterProvider parameterProvider;

        private List<int> fieldLenghts;

        private IUIMessage uIMessage;

        private string separator;

        private string additionalCharacter;
        public AquisitionSerializer()
        {
            ServiceLocator.Resolve(out parameterProvider);
            separator = this.parameterProvider.GetAndSetIfMissing("InterfaceSerializerSeparator", ",");
            additionalCharacter = this.parameterProvider.GetAndSetIfMissing("InterfaceAdditionalCharacter", " ");
            this.fieldLenghts = new List<int> { 2, 6, 3, 2, 10, 6, 12, 1, 50, 20 };
            ServiceLocator.GetDefault(out this.uIMessage);
        }



        public void Serialize(IEnumerable<Item> items)
        {
            if(!items.Any())
            {
                this.uIMessage.Error("Veuillez selectionner des articles à envoyer");
                return;
            }
                    
            var itemGroupedByFacture = items.Where(x => x.Invoice != null).GroupBy(x => x.Invoice);

            if (!itemGroupedByFacture.Any())
            {
                this.uIMessage.Error("La selection ne contient aucune facture !");
                return;
            }

            var stringBuilder = new StringBuilder();
            foreach (var group in itemGroupedByFacture)
            {
                var invoice = group.Key;
                int i = 1;
                var lastPart = $"FACT {invoice.Key} {invoice.Date.ToString("dd/MM/yyyy")} {invoice.Provider.Caption};{invoice.Key}";
                var firstPart = $"21;{invoice.InputSheet.Key}";
                var itemGroupedByGenralAccount = group.GroupBy(x => x.GeneralAccount);

                foreach (var subGroup in itemGroupedByGenralAccount)
                {
                    var totalAmount = subGroup.Sum(x => x.Amount);
                    stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day, subGroup.Key.Key, " ", totalAmount, "D", lastPart)));
                    i++;
                }
                
                var invoiceAmount = invoice.Amount - invoice.Holdback;

                var thirdPartyAccount = invoice.Provider.Country.ToLower().Contains("alger") ? "404000" : "404010";
                stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day, thirdPartyAccount, " ", invoiceAmount, "C", lastPart)));
                i++;
                stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day, "404020", " ", invoice.Holdback, "C", lastPart)));
            }

            var outputDirectory = parameterProvider.GetAndSetIfMissing("InterfaceOutputDirectory", @"C:\SQLIMMO\");
            var FileNamePattern = parameterProvider.GetAndSetIfMissing("InterfaceFileNamePattern", "Dump");
            var FileExtension = parameterProvider.GetAndSetIfMissing("InterfaceFileExtension", ".csv");
            var fileName = Path.Combine(outputDirectory, $"{FileNamePattern}_{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}.{FileExtension.Replace(".", "")}");
            logger.Info($"FileName = {fileName}");
            TextAppender.Append(fileName, stringBuilder.ToString(), true);
            this.uIMessage.Notify("Fichier généré avec succès");
        }

        public string Align(string input)
        {
            var data = input.Split(';');
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = data[i].Align(fieldLenghts[i], this.additionalCharacter, AdditionnalCharPosition.Right);
            }

            return string.Join(this.separator, data);
        }
    }
}
