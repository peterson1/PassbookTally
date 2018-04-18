using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.Authorization;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using PassbookTally.DomainLib45.Configuration;
using System;

namespace PassbookTally.CrudApp.TransactionCRUD
{
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


        protected override void SaveNewRecord(SoaRowDTO draft)  => TweakThenSave(draft);
        protected override void UpdateRecord (SoaRowDTO record) => TweakThenSave(record);


        protected override bool CanEncodeNewDraft()
            => AppArgs.CanAddBankTransaction(false);


        private void TweakThenSave(SoaRowDTO dto)
        {
            dto.DocRefType = this.GetType().Namespace;
            dto.DateOffset = TransactionDate.SoaRowOffset();
            _repo.UpsertAndUpdateBalances(dto);
        }


        protected override SoaRowDTO CreateDraftFromRecord(SoaRowDTO record)
        {
            TransactionDate = record.GetDate();
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
