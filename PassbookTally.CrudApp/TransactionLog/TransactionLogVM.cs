using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.LiteDbTools;
using PassbookTally.CrudApp.TransactionCRUD;
using PassbookTally.DatabaseLib.Repositories;
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
    public class TransactionLogVM : SavedListVMBase<SoaRowDTO>
    {
        private SoaRowsRepo1 _repo;


        public TransactionLogVM(AppArguments args, SoaRowsRepo1 soaRowsRepo, bool doReload = true) : base(soaRowsRepo, doReload)
        {
            _repo = soaRowsRepo;
            Crud  = new TransactionCrudVM(_repo, args);
            Crud.SaveCompleted += (s, e) => ReloadFromDB();
            Rows.ItemOpened    += Rows_ItemOpened;
            Rows.ItemDeleted   += Rows_ItemDeleted;
        }


        public UIList<SoaRowVM>    Rows         { get; } = new UIList<SoaRowVM>();
        public TransactionCrudVM   Crud         { get; }
        public decimal             LastBalance  { get; private set; }


        protected override IEnumerable<SoaRowDTO> QueryItems(SharedCollectionBase<SoaRowDTO> db)
        {
            var dtos = db.GetAll();
            Rows.SetItems(dtos.Select (_ => new SoaRowVM(_))
                              .OrderBy(_ => _.TransactionDate));
            LastBalance = dtos.LastBalance();
            return dtos;
        }


        private void Rows_ItemOpened(object sender, SoaRowVM e)
        {
            if (IsEditable(e)) Crud.EditCurrentRecord(e.DTO);
        }


        private void Rows_ItemDeleted(object sender, SoaRowVM e)
        {
            if (IsEditable(e)) _repo.Delete(e.DTO);
            ReloadFromDB();
        }



        private bool IsEditable(SoaRowVM row)
            => row.DTO.DocRefType == typeof(TransactionCrudVM).Namespace;


        protected override Func<SoaRowDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
