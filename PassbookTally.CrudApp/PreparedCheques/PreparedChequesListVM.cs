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

namespace PassbookTally.CrudApp.PreparedCheques
{
    [AddINotifyPropertyChangedInterface]
    public class PreparedChequesListVM : FilteredSavedListVMBase<RequestedChequeDTO, PreparedChequesFilterVM, AppArguments>
    {
        private MainWindowVM _mainWin;

        public PreparedChequesListVM(MainWindowVM mainWindow) : base(mainWindow.AppArgs.DCDR.ActiveCheques, mainWindow.AppArgs, false)
        {
            _mainWin       = mainWindow;
            IssueChequeCmd = R2Command.Relay(IssueChequeToPayee, null, "Issue Cheque to Payee");
        }


        public IR2Command IssueChequeCmd { get; }


        private void IssueChequeToPayee()
        {
            var chq = ItemsList.CurrentItem;
            if (chq == null) return;

            if (!PopUpInput.TryGetString("Issued To", out string issuedTo, chq.Request.Payee)) return;
            if (!PopUpInput.TryGetDate("Date Issued", out DateTime issuedDate)) return;
            AppArgs.DCDR.ToIssuedCheque(chq, issuedTo, issuedDate);
            _mainWin.ClickRefresh();
        }


        protected override List<RequestedChequeDTO> QueryItems(SharedCollectionBase<RequestedChequeDTO> db)
            => db.GetAll().Where(_ => !_.IssuedDate.HasValue
                                    && _.Request.BankAccountId == _mainWin.AccountId).ToList();


        protected override Func<RequestedChequeDTO, decimal> SummedAmount => _ => _.Request.Amount.Value;
    }
}
