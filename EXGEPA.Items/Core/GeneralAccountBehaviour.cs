using CORESI.IoC;

namespace EXGEPA.Items.Core
{
    class GeneralAccountBehaviour : IPriority
    {
        public GeneralAccountBehaviour()
        {

        }

        public int Priority { get { return 0; } }

        //private void Setup(decimal amount)
        //{
        //    if (Amount >= MinAmount)
        //    {
        //        ChargeAccount = null;

        //        _EnableChargeAccount = false;
        //        _EnableInvestissementAndDepriciationAccount = true;

        //        if (DepreciationAccount != null)
        //        {
        //            ConcernedItem.AccountingRate = DepreciationAccount.Rate;
        //            ConcernedItem.FiscaleRate = DepreciationAccount.Rate;
        //        }
        //    }
        //    else
        //    {
        //        InvestissementAccount = null;
        //        ConcernedItem.DepreciationAccount = null;
        //        _EnableChargeAccount = true;
        //        _EnableInvestissementAndDepriciationAccount = false;
        //        ConcernedItem.AccountingRate = 0;
        //        ConcernedItem.FiscaleRate = 0;
        //        ConcernedItem.LimiteDate = AquisitionDate;
        //    }
        //}
    }
}
