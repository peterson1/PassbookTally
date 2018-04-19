using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.LiteDbTools;
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
        private SoaRowsRepo1 _repo1;
        private DateTime     _startDate;


        public TransactionLogVM(SoaRowsRepo1 soaRowsRepo, AppArguments args, DateTime startDate, bool doReload = false) : base(soaRowsRepo, args, doReload)
        {
            _repo1     = soaRowsRepo;
            _startDate = startDate;
            Crud       = new TransactionCrudVM(_repo1, args);
            ReloadFromDB();

            Crud.SaveCompleted += (s, e) => ReloadFromDB();
            Rows.ItemOpened    += Rows_ItemOpened;
            Rows.ItemDeleted   += Rows_ItemDeleted;
        }


        public UIList<SoaRowVM>    Rows         { get; } = new UIList<SoaRowVM>();
        public TransactionCrudVM   Crud         { get; }
        public decimal             LastBalance  { get; private set; }
        public DateTime            LastDate     { get; private set; }


        protected override List<SoaRowDTO> QueryItems(SharedCollectionBase<SoaRowDTO> db)
        {
            var dtos = _repo1.RowsStartingFrom(_startDate).ToList();
            Rows.SetItems(dtos.Select (_ => new SoaRowVM(_))
                              .OrderBy(_ => _.TransactionDate));

            var lastRow = dtos.LastRow();
            LastBalance = lastRow.RunningBalance;
            LastDate    = lastRow.GetDate();
            return dtos;
        }


        private void Rows_ItemOpened(object sender, SoaRowVM e)
        {
            if (IsEditable(e) &&
                AppArgs.CanEditBankTransaction(true))
                    Crud.EditCurrentRecord(e.DTO);
        }


        private void Rows_ItemDeleted(object sender, SoaRowVM e)
        {
            if (IsEditable(e) &&
                AppArgs.CanDeleteBankTransaction(true))
                    _repo1.Delete(e.DTO);

            ReloadFromDB();
        }


        private bool IsEditable(SoaRowVM row)
            => row.DTO.DocRefType == typeof(TransactionCrudVM).Namespace;


        protected override Func<SoaRowDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
