using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DatabaseLib.StateTransitions;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.CrudApp.IssuedCheques
{
    [AddINotifyPropertyChangedInterface]
    public class IssuedChequesListVM : FilteredSavedListVMBase<RequestedChequeDTO, IssuedChequesFilterVM, AppArguments>
    {
        private MainWindowVM _mainWin;

        public IssuedChequesListVM(MainWindowVM mainWindow) : base(mainWindow.AppArgs.DCDR.ActiveCheques, mainWindow.AppArgs, false)
        {
            _mainWin       = mainWindow;
            ClearChequeCmd = R2Command.Relay(MarkAsCleared, null, "Mark Cheque as “Cleared”");
        }


        public IR2Command ClearChequeCmd { get; }


        private void MarkAsCleared()
        {
            var chq = ItemsList.CurrentItem;
            if (chq == null) return;

            if (!PopUpInput.TryGetDate("Date Cleared", out DateTime clearedDate)) return;
            AppArgs.DCDR.ToBankTransaction(chq, clearedDate, _mainWin.GetSoaRepo());
            _mainWin.ClickRefresh();
        }


        protected override List<RequestedChequeDTO> QueryItems(SharedCollectionBase<RequestedChequeDTO> db)
            => db.GetAll().Where(_ => _.IssuedDate.HasValue
                                   && _.BankAccountId == _mainWin.AccountId).ToList();


        protected override Func<RequestedChequeDTO, decimal> SummedAmount => _ => _.Request.Amount.Value;
    }
}
