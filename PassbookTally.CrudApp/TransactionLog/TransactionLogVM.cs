using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using PassbookTally.CrudApp.TransactionCRUD;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.Authorization;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.CrudApp.TransactionLog
{
    [AddINotifyPropertyChangedInterface]
    public class TransactionLogVM : SavedListVMBase<SoaRowDTO, AppArguments>
    {
        private SoaRowsRepo1   _repo1;
        private DateTime       _startDate;
        private List<SoaRowVM> _queried;

        public TransactionLogVM(SoaRowsRepo1 soaRowsRepo, AppArguments args, DateTime startDate, bool doReload = false) : base(soaRowsRepo, args, doReload)
        {
            _repo1     = soaRowsRepo;
            _startDate = startDate;
            Crud       = new TransactionCrudVM(_repo1, args);
            ReloadFromDB();

            Crud  .SaveCompleted     += (s, e) => ReloadFromDB();
            Filter.TextFilterChanged += (s, e) => ApplyTextFilters();
            Rows  .ItemOpened        += Rows_ItemOpened;
            Rows  .ItemDeleted       += Rows_ItemDeleted;
        }


        public TransactionCrudVM       Crud         { get; }
        public UIList<SoaRowVM>        Rows         { get; } = new UIList<SoaRowVM>();
        public TransactionLogFilterVM  Filter       { get; } = new TransactionLogFilterVM();
        public decimal                 LastBalance  { get; private set; }
        public DateTime                LastDate     { get; private set; }


        private void Rows_ItemOpened(object sender, SoaRowVM e)
        {
            if (!AppArgs.CanEditBankTransaction(true)) return;

            var typ = e.DTO.DocRefType;

            if (typ == typeof(TransactionCrudVM).FullName)
                Crud.EditCurrentRecord(e.DTO);
            else if (typ == typeof(RequestedChequeDTO).FullName)
                EditClearedDate(e.DTO);
        }


        private void EditClearedDate(SoaRowDTO dto)
        {
            if (!PopUpInput.TryGetDate("Cleared Date", out DateTime date, dto.GetDate())) return;
            dto.DateOffset = date.SoaRowOffset();
            _repo1.UpsertAndUpdateBalances(dto);
            ReloadFromDB();
        }


        private void Rows_ItemDeleted(object sender, SoaRowVM e)
        {
            if (!AppArgs.CanDeleteBankTransaction(true)) goto Reload;
            if (e.DTO.DocRefType != typeof(TransactionCrudVM).FullName) goto Reload;

            _repo1.DeleteAndUpdateBalances(e.DTO);

            Reload:
            ReloadFromDB();
        }


        public override void ReloadFromDB()
        {
            Rows.Clear();
            var dtos    = _repo1.GetFrom(_startDate);
            if (!dtos.Any()) return;

            var lastRow = dtos.Last();
            LastBalance = lastRow.RunningBalance;
            LastDate    = lastRow.GetDate();
            _queried    =  dtos.Select (_ => new SoaRowVM(_))
                               .OrderBy(_ => _.TransactionDate)
                               .ToList();
            ApplyTextFilters();
        }


        private void ApplyTextFilters()
        {
            var list = _queried.ToList();
            Filter.RemoveNonMatches(ref list);
            Rows.SetItems(list);
        }


        protected override Func<SoaRowDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
