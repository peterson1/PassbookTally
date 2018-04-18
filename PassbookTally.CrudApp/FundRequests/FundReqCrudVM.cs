using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;

namespace PassbookTally.CrudApp.FundRequests
{
    public class FundReqCrudVM : CrudWindowVMBase<FundRequestDTO, FundReqCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Voucher Request";
        protected override string CaptionPrefix   => "Voucher Request";

        private ActiveFundReqsRepo _repo;


        public FundReqCrudVM(ActiveFundReqsRepo activeFundReqsRepo, AppArguments appArguments) : base(appArguments)
        {
            _repo = activeFundReqsRepo;
        }



        protected override void SaveNewRecord(FundRequestDTO draft) 
            => _repo.Insert(draft);


        protected override void UpdateRecord(FundRequestDTO record)
            => _repo.Update(record);


        protected override bool IsValidDraft(FundRequestDTO draft, out string whyInvalid)
        {
            if (draft.Amount == 0)
            {
                whyInvalid = "Amount should not be zero.";
                return false;
            }
            if (draft.Payee.IsBlank())
            {
                whyInvalid = "Payee should not be blank.";
                return false;
            }
            if (draft.Purpose.IsBlank())
            {
                whyInvalid = "Purpose should not be blank.";
                return false;
            }
            whyInvalid = "";
            return true;
        }
    }
}
