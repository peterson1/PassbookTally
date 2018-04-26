using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DatabaseLib;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.Authorization;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Linq;

namespace PassbookTally.CrudApp.FundRequests
{
    [AddINotifyPropertyChangedInterface]
    public class FundReqCrudVM : CrudWindowVMBase<FundRequestDTO, FundReqCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Voucher Request";
        protected override string CaptionPrefix   => "Voucher Request";

        private int                _savedSerial;


        public FundReqCrudVM(AppArguments appArguments) : base(appArguments)
        {
            Allocations = new AllocationsListVM(this);
        }


        public UIList<string>     Payees       { get; } = new UIList<string>();
        public AllocationsListVM  Allocations  { get; }


        private PassbookDB         DB   => AppArgs.DCDR;
        private ActiveFundReqsRepo Repo => DB.ActiveRequests;


        protected override void SetNewDraftDefaults(FundRequestDTO draft)
        {
            draft.RequestDate = DateTime.Now;
            draft.SerialNum   = DB.NextRequestSerial();
            Payees.SetItems(DB.GetPayees());
            Allocations.DisplayItems(draft);
        }


        protected override bool CanEncodeNewDraft()
            => AppArgs.CanAddVoucherRequest(false);


        protected override void SaveNewRecord(FundRequestDTO draft)
        {
            draft.Allocations = Allocations.ToDTOs();
            Repo.Insert(draft);
        }


        protected override void UpdateRecord(FundRequestDTO record)
        {
            record.Allocations = Allocations.ToDTOs();
            Repo.Update(record);
        }


        protected override bool IsValidDraft(FundRequestDTO draft, out string whyInvalid)
        {
            Allocations.UpdateBaseAmount(draft.Amount);

            if (draft.Amount.HasValue)
            {
                if (Allocations.TotalDebit  != draft.Amount
                || Allocations .TotalCredit != draft.Amount)
                {
                    whyInvalid = "Total Debits and Credits must match voucher amount.";
                    return false;
                }
            }

            if (IsDuplicateSerial(draft))
            {
                whyInvalid = "Serial number is used in another request.";
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


        protected override FundRequestDTO CreateDraftFromRecord(FundRequestDTO record)
        {
            _savedSerial = record.SerialNum;
            Allocations.DisplayItems(record);
            return base.CreateDraftFromRecord(record);
        }


        private bool IsDuplicateSerial(FundRequestDTO draft)
        {
            if (draft.Id == 0)
                return DB.HasRequestSerial(draft.SerialNum);

            if (_savedSerial == draft.SerialNum)
                return false;

            return DB.HasRequestSerial(draft.SerialNum);
        }
    }
}
