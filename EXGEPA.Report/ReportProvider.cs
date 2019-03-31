using CORESI.WPF.Core;
using CORESI.WPF.Model;
using System.Collections.Generic;

namespace EXGEPA.Report
{

    public abstract class ReportProvider<T> : AReportPreviwer<T> where T : CORESI.Data.IRowId
    {
        public override Group GetGroupForReportBottons()
        {
            var group = new Group();
            group.AddCommand("Edition", IconProvider.Reading, this.PrintSheet);
            var additionalButtons = this.GetAdditionalReportCommand();
            if (additionalButtons?.Count > 0)
            {
                additionalButtons.ForEach(button => group.Commands.Add(button));
            }
            return group;
        }
        public abstract void PrintSheet();

        public virtual List<SimpleItem> GetAdditionalReportCommand()
        {
            return null;
        }
    }
}

