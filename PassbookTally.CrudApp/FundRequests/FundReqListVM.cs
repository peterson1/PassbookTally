using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DatabaseLib;
using PassbookTally.DomainLib.Authorization;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.CrudApp.FundRequests
{
    [AddINotifyPropertyChangedInterface]
    public class FundReqListVM : FilteredSavedListVMBase<FundRequestDTO, FundReqListFilterVM, AppArguments>
    {
        private PassbookDB _db;


        public FundReqListVM(PassbookDB passbookDB, AppArguments appArguments) : base(passbookDB.ActiveRequests, appArguments, false)
        {
            _db  = passbookDB;
            Crud = new FundReqCrudVM(_db, AppArgs);
        }


        public FundReqCrudVM Crud { get; }


        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount ?? 0;


        protected override List<FundRequestDTO> QueryItems(SharedCollectionBase<FundRequestDTO> db)
            => db.GetAll().OrderByDescending(_ => _.SerialNum).ToList();


        protected override void DeleteRecord(SharedCollectionBase<FundRequestDTO> db, FundRequestDTO dto)
        {
            if (AppArgs.CanDeleteVoucherRequest(true))
                db.Delete(dto);

            ReloadFromDB();
        }


        protected override void OnItemOpened(FundRequestDTO e)
        {
            if (AppArgs.CanEditVoucherRequest(true))
                Crud.EditCurrentRecord(e);
        }
    }
}
