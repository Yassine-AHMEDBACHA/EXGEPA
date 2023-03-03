namespace EXGEPA.Saidal.Core.Dotations
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class DotationSerializer : Serializer<Wrapper>
    {
        public override string OperationHeader => "81";

        public override string GetLastPart(Wrapper instance)
        {
            return "DOTATION AUX AMORTS AU ";
        }

        protected override string GetFileNamePattern()
        {
            return this.parameterProvider.TryGet("InterfaceFileNamePatternForDotation", "DotationDump");
        }

        public override List<Wrapper> Serialize(IEnumerable<Wrapper> instances)
        {
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

            }

            this.SaveFile(rows);

            return instances.ToList();
        }
    }
}
