using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.Authorization;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;

namespace PassbookTally.CrudApp.TransactionCRUD
{
    [AddINotifyPropertyChangedInterface]
    public class TransactionCrudVM : CrudWindowVMBase<SoaRowDTO, TransactionCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Bank Transaction";
        protected override string CaptionPrefix   => "Bank Transaction";

        private SoaRowsRepo1 _repo;

        public TransactionCrudVM(SoaRowsRepo1 soaRowsRepo, AppArguments appArguments) : base(appArguments)
        {
            _repo = soaRowsRepo;
        }


        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public bool     IsDeposit       { get; set; } = true;
        public string   TransactionType => IsDeposit ? "deposit" : "withdrawal";
        public string   AmountType      => $"{TransactionType} amount";
        public string   TxnRefType      => $"{TransactionType} slip #";


        protected override void SaveNewRecord(SoaRowDTO draft)  => TweakThenSave(draft);
        protected override void UpdateRecord (SoaRowDTO record) => TweakThenSave(record);


        protected override bool CanEncodeNewDraft()
            => AppArgs.CanAddBankTransaction(false);


        private void TweakThenSave(SoaRowDTO dto)
        {
            dto.DocRefType = this.GetType().Namespace;
            dto.DateOffset = TransactionDate.SoaRowOffset();
            dto.Amount     = Math.Abs(dto.Amount) 
                            * (IsDeposit ? 1.0M : -1.0M);

            _repo.UpsertAndUpdateBalances(dto);
        }


        protected override SoaRowDTO CreateDraftFromRecord(SoaRowDTO record)
        {
            TransactionDate = record.GetDate();
            IsDeposit       = record.Amount > 0;
            record.Amount   = Math.Abs(record.Amount);
            return record;
        }


        protected override bool IsValidDraft(SoaRowDTO draft, out string whyInvalid)
        {
            if (draft.Amount == 0)
            {
                whyInvalid = "Amount should not be zero.";
                return false;
            }
            if (draft.Subject.IsBlank() && draft.Description.IsBlank())
            {
                whyInvalid = "Subject and Description should not be blank.";
                return false;
            }
            whyInvalid = "";
            return true;
        }
    }
}
