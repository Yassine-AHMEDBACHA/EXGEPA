﻿// <copyright file="AquisitionSerializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.Tools.StringTools;
    using CORESI.WPF;
    using EXGEPA.Model;

    public class AquisitionSerializer
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IParameterProvider parameterProvider;

        private readonly string additionalCharacter;

        private readonly List<Func<string, string>> fieldAligner;

        private readonly IUIMessage uIMessage;

        private readonly string separator;

        public AquisitionSerializer()
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            this.separator = this.parameterProvider.GetAndSetIfMissing("InterfaceSerializerSeparator", ",");
            this.additionalCharacter = this.parameterProvider.GetAndSetIfMissing("InterfaceAdditionalCharacter", " ");
            ServiceLocator.GetDefault(out this.uIMessage);
            this.fieldAligner = new List<Func<string, string>>()
            {
                (str) => "21",
                (str) => "   1  ",
                (str) => $"{this.additionalCharacter}{str.Align(2, "0")}",
                (str) => str.Align(2, this.additionalCharacter, AdditionnalCharPosition.Right),
                (str) => str.Align(10, this.additionalCharacter, AdditionnalCharPosition.Right),
                (str) => str.Align(6, this.additionalCharacter, AdditionnalCharPosition.Right),
                (str) => str.Align(16, this.additionalCharacter),
                (str) => str.Align(1, this.additionalCharacter, AdditionnalCharPosition.Right),
                (str) => str.Align(50, this.additionalCharacter, AdditionnalCharPosition.Right),
                (str) => str.Align(8, this.additionalCharacter, AdditionnalCharPosition.Right)
            };
        }

        public void Serialize(IEnumerable<Item> items)
        {
            if (!items.Any())
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
                var totalInvestmentAccount = 0.0M;
                var totalChargeAccount = 0.0M;
                foreach (var subGroup in itemGroupedByGenralAccount)
                {
                    foreach (var item in subGroup)
                    {
                        if (item.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment)
                        {
                            totalInvestmentAccount += item.Amount;
                        }
                        else if (item.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Charge)
                        {
                            totalChargeAccount += item.Amount;
                        }
                    }

                    var totalAmount = totalChargeAccount + totalInvestmentAccount;
                    stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day.ToString("dd"), subGroup.Key.Key, " ", totalAmount.ToString(CultureInfo.InvariantCulture), "D", lastPart)));
                    i++;
                }

                var invoiceAmount = totalInvestmentAccount - invoice.Holdback;
                var account = invoice.Provider.Country.ToLower().Contains("alger") ? "404000" : "404010";
                stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day.ToString("dd"), account, invoice.Provider.ThirdPartyAccount, invoiceAmount.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                if (totalChargeAccount > 0)
                {
                    i++;
                    stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day.ToString("dd"), account, "401010", totalChargeAccount.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                }

                if (invoice.Holdback > 0)
                {
                    i++;
                    stringBuilder.AppendLine(this.Align(string.Join(";", firstPart, i, invoice.Date.Day.ToString("dd"), "404020", " ", invoice.Holdback.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                }
            }

            var outputDirectory = this.parameterProvider.GetAndSetIfMissing("InterfaceOutputDirectory", @"C:\SQLIMMO\");
            var fileNamePattern = this.parameterProvider.GetAndSetIfMissing("InterfaceFileNamePattern", "Dump");
            var fileExtension = this.parameterProvider.GetAndSetIfMissing("InterfaceFileExtension", ".csv");
            var fileName = Path.Combine(outputDirectory, $"{fileNamePattern}_{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}.{fileExtension.Replace(".", string.Empty)}");
            this.logger.Info($"FileName = {fileName}");
            TextAppender.Append(fileName, stringBuilder.ToString(), true);
            this.uIMessage.Notify("Fichier généré avec succès");
        }

        public string Align(string input)
        {
            var data = input.Split(';');
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = this.fieldAligner[i](data[i]);
            }

            return string.Join(this.separator, data);
        }
    }
}
