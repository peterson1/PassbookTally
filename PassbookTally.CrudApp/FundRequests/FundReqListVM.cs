using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.LiteDbTools;
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
        public FundReqListVM(AppArguments appArguments) : base(appArguments.PassbookDB.ActiveRequests, appArguments, false)
        {
            Crud          = new FundReqCrudVM(AppArgs);
            IssueCheckCmd = R2Command.Relay(IssueCheck, null, "Issue Check to Payee");
        }


        public FundReqCrudVM  Crud           { get; }
        public IR2Command     IssueCheckCmd  { get; }


        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount ?? 0;


        private void IssueCheck()
        {
            throw new NotImplementedException();
        }


        protected override List<FundRequestDTO> QueryItems(SharedCollectionBase<FundRequestDTO> db)
            => db.GetAll().OrderByDescending(_ => _.SerialNum).ToList();


        protected override bool CanEditRecord   (FundRequestDTO e) => AppArgs.CanEditVoucherRequest(true);
        protected override bool CanDeletetRecord(FundRequestDTO e) => AppArgs.CanDeleteVoucherRequest(true);

        protected override void LoadRecordForEditing
            (FundRequestDTO rec) => Crud.EditCurrentRecord(rec);
    }
}
