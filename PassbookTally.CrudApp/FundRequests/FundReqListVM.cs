using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;

namespace PassbookTally.CrudApp.FundRequests
{
    [AddINotifyPropertyChangedInterface]
    public class FundReqListVM : SavedListVMBase<FundRequestDTO, AppArguments>
    {
        public FundReqListVM(ActiveFundReqsRepo activeFundReqsRepo, AppArguments appArguments) : base(activeFundReqsRepo, appArguments, false)
        {
        }


        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
