using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using System;

namespace PassbookTally.CrudApp.PreparedCheques
{
    public class PreparedChequesListVM : FilteredSavedListVMBase<RequestedChequeDTO, PreparedChequesFilterVM, AppArguments>
    {
        private MainWindowVMBase<AppArguments> _mainWin;

        public PreparedChequesListVM(MainWindowVMBase<AppArguments> mainWindow) : base(mainWindow.AppArgs.PassbookDB.ActiveCheques, mainWindow.AppArgs, false)
        {
            _mainWin = mainWindow;
        }


        public IR2Command IssueChequeCmd { get; }


        protected override Func<RequestedChequeDTO, decimal> SummedAmount => _ => _.Request.Amount.Value;
    }
}
