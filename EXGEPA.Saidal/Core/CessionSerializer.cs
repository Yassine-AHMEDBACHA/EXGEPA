namespace EXGEPA.Saidal.Core
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

    public class CessionSerializer : Serializer<OutputCertificate>
    {
        private readonly IRepositoryDataProvider repositoryDataProvider;

        public CessionSerializer(IRepositoryDataProvider repositoryDataProvider)
        {
            this.repositoryDataProvider = repositoryDataProvider;
        }

        public override string OperationHeader => "81";

        public override string GetLastPart(OutputCertificate instance)
        {
            return $"Cession {instance.Key} {instance.Date:dd/MM/yyyy} {instance.Tag};{instance.Key}";
        }

        public override List<OutputCertificate> Serialize(IEnumerable<OutputCertificate> instances)
        {
            var result = new List<OutputCertificate>();
            if (!instances.Any())
            {
                this.uIMessage.Error("Selection vide ou deja traitée, Veuillez selectionner des lignes à envoyer !");
                return result;
            }

            var analyticalAccounts = this.repositoryDataProvider.ListOfAnalyticalAccount.ToDictionary(x => x.Key);

            var rows = new List<string>();
            int j = 0;
            foreach (var instance in instances)
            {
                j++;
                int i = 1;
                string lastPart = this.GetLastPart(instance);
                var firstPart = $"{this.OperationHeader};{j}";
                var groups = instance.Items.Values.OrderBy(x => x.GeneralAccount.Key).GroupBy(x => x.GeneralAccount)
                    .Select(g => new
                    {
                        GeneralAccount = g.Key,
                        TotalAmount = g.Sum(x => x.Amount),
                        TotalPreviousDepreciations = g.Sum(x => x.PreviousDepreciation)
                    })
                    .ToList();

                foreach (var item in groups)
                {
                    rows.Add(this.Align(string.Join(";", firstPart, i, instance.Date.ToString("dd"), item.GeneralAccount.Key, " ", item.TotalAmount.ToString(CultureInfo.InvariantCulture), "D", lastPart)));
                    i++;
                }

                var analyticalAccount = analyticalAccounts[instance.Tag.ToString()];
                foreach (var item in groups)
                {
                    rows.Add(this.Align(string.Join(";", firstPart, i, instance.Date.ToString("dd"), analyticalAccount.ThirdPartyAccount, " ", item.TotalAmount.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                    i++;
                }

                foreach (var item in groups)
                {
                    rows.Add(this.Align(string.Join(";", firstPart, i, instance.Date.ToString("dd"), analyticalAccount.ThirdPartyAccount, " ", item.TotalPreviousDepreciations.ToString(CultureInfo.InvariantCulture), "D", lastPart)));
                    i++;
                }

                foreach (var item in groups.Where(x => x.GeneralAccount.Children != null))
                {
                    rows.Add(this.Align(string.Join(";", firstPart, i, instance.Date.ToString("dd"), item.GeneralAccount.Children.Key, " ", item.TotalPreviousDepreciations.ToString(CultureInfo.InvariantCulture), "C", lastPart)));
                    i++;
                }

                instance.Caption = true.ToString();
                result.Add(instance);
            }

            this.SaveFile(rows);
            return result;
        }

        protected override string GetFileNamePattern()
        {
            return this.parameterProvider.TryGet("InterfaceFileNamePatternForCession", "CessionDump");
        }
    }
}
