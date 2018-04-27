using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.LiteDbTools;
using CommonTools.Lib45.ThreadTools;
using PassbookTally.DatabaseLib.StateTransitions;
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
        private MainWindowVM _mainWin;

        public FundReqListVM(MainWindowVM mainWindow) : base(mainWindow.AppArgs.DCDR.ActiveRequests, mainWindow.AppArgs, false)
        {
            _mainWin       = mainWindow;
            Crud           = new FundReqCrudVM(AppArgs);
            InputChequeCmd = R2Command.Relay(InputChequeDetails, null, "Input Cheque Details");
        }


        public FundReqCrudVM  Crud            { get; }
        public IR2Command     InputChequeCmd  { get; }


        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount ?? 0;


        private void InputChequeDetails()
        {
            var req = ItemsList.CurrentItem;
            if (req == null) return;
            if (!req.Amount.HasValue)
            {
                Alert.Show("Amount requested should not be blank.");
                return;
            }
            var pbk = AppArgs.DCDR;
            //if (!PopUpInput.TryGetIndex("Bank Account" , out int idx, pbk.AccountNames, 0)) return;
            if (!PopUpInput.TryGetInt ("Cheque Number", out int num)) return;
            if (!PopUpInput.TryGetDate("Cheque Date"  , out DateTime date)) return;
            pbk.ToPreparedCheque(req, num, date);
            _mainWin.ClickRefresh();
        }


        protected override List<FundRequestDTO> QueryItems(SharedCollectionBase<FundRequestDTO> db)
            => db.GetAll().Where(_ => _.BankAccountId == AppArgs.AccountId)
                          .OrderByDescending(_ => _.SerialNum).ToList();


        protected override bool CanEditRecord   (FundRequestDTO e) => AppArgs.CanEditVoucherRequest(true);
        protected override bool CanDeletetRecord(FundRequestDTO e) => AppArgs.CanDeleteVoucherRequest(true);

        protected override void LoadRecordForEditing
            (FundRequestDTO rec) => Crud.EditCurrentRecord(rec);
    }
}
