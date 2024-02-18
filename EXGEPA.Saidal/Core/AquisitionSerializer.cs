// <copyright file="AquisitionSerializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.Tools;
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
            this.separator = this.parameterProvider.TryGet("InterfaceSerializerSeparator", ",");
            this.additionalCharacter = this.parameterProvider.TryGet("InterfaceAdditionalCharacter", " ");
            ServiceLocator.GetDefault(out this.uIMessage);
            this.fieldAligner = new List<Func<string, string>>()
            {
                (str) => "21",
                (str) => $"{str}  ".Align(5, " ", AdditionnalCharPosition.Left),
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

        public List<Invoice> Serialize(IEnumerable<Invoice> invoices)
        {
            var generatedInvoices = new List<Invoice>();
            if (!invoices.Any())
            {
                this.uIMessage.Error("Selection vide ou deja traitée, Veuillez selectionner des lignes à envoyer !");
                return generatedInvoices;
            }

            var rows = new List<string>();
            int j = 0;
            foreach (var invoice in invoices)
            {
                j++;
                int i = 1;
                var lastPart = $"FACT {invoice.Key} {invoice.Date:dd/MM/yyyy} {invoice.Provider.Caption};{invoice.Key}";
                var firstPart = $"21;{j}";
                var itemGroupedByGenralAccount = invoice.Items.Values.OrderBy(x => x.GeneralAccount.Caption).GroupBy(x => x.GeneralAccount);
                var totalInvestmentAccount = 0.0M;
                var totalChargeAccount = 0.0M;
                foreach (var subGroup in itemGroupedByGenralAccount)
                {
                    var subTotal = subGroup.Sum(x => x.Amount);
                    if (subGroup.Key.GeneralAccountType.Type == EGeneralAccountType.Investment)
                    {
                        totalInvestmentAccount += subTotal;
                    }
                    else if (subGroup.Key.GeneralAccountType.Type == EGeneralAccountType.Charge)
                    {
                        totalChargeAccount += subTotal;
                    }

                    rows.Add(this.Align(string.Join(";", firstPart, i, invoice.Date.ToString("dd"), subGroup.Key.Key, " ", subTotal.ToString(CultureInfo.InvariantCulture), "D", lastPart)));
                    i++;
                }

                var invoiceAmount = totalInvestmentAccount - invoice.Holdback;
                var account = invoice.Provider.Country.ToLower().Contains("alger") ? "404000" : "404010";
                rows.Add(this.Align(string.Join(";", firstPart, i, invoice.Date.ToString("dd"), account, invoice.Provider.ThirdPartyAccount, invoiceAmount.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                if (totalChargeAccount > 0)
                {
                    i++;
                    rows.Add(this.Align(string.Join(";", firstPart, i, invoice.Date.ToString("dd"), "401010", invoice.Provider.ThirdPartyAccount, totalChargeAccount.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                }

                if (invoice.Holdback > 0)
                {
                    i++;
                    rows.Add(this.Align(string.Join(";", firstPart, i, invoice.Date.ToString("dd"), "404020", invoice.Provider.ThirdPartyAccount, invoice.Holdback.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                }

                invoice.Caption = true.ToString();
                generatedInvoices.Add(invoice);
            }

            var fileName = this.GetFileName();
            this.logger.Info($"FileName = {fileName}");
            TextAppender.Append(fileName, rows, true);
            return generatedInvoices;
        }

        private string GetFileName()
        {
            var outputDirectory = this.parameterProvider.TryGet("InterfaceOutputDirectory", @"C:\SQLIMMO\");
            var fileNamePattern = this.parameterProvider.TryGet("InterfaceFileNamePattern", "Dump");
            var fileExtension = this.parameterProvider.TryGet("InterfaceFileExtension", ".csv");
            var fileName = Path.Combine(outputDirectory, $"{fileNamePattern}_{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}.{fileExtension.Replace(".", string.Empty)}");
            return fileName;
        }

        private string Align(string input)
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
