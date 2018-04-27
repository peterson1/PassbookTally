using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.FileSystemTools;
using PassbookTally.CrudApp.FundRequests;
using PassbookTally.CrudApp.IssuedCheques;
using PassbookTally.CrudApp.PreparedCheques;
using PassbookTally.CrudApp.TransactionLog;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib45.BaseViewModels;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Linq;

namespace PassbookTally.CrudApp
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : MaterialWindowBase
    {
        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            FillAccountNames();
            UpdateNotifier  = new UpdatedExeNotifier(AppArgs.OrigCrudExe);
            FundRequests    = new FundReqListVM(this);
            PreparedCheques = new PreparedChequesListVM(this);
            IssuedCheques   = new IssuedChequesListVM(this);
            ClickRefresh();
        }


        public UpdatedExeNotifier     UpdateNotifier   { get; }
        public UIList<string>         AccountNames     { get; } = new UIList<string>();
        public string                 AccountName      { get; set; }
        public TransactionLogVM       TransactionLog   { get; private set; }
        public FundReqListVM          FundRequests     { get; }
        public PreparedChequesListVM  PreparedCheques  { get; }
        public IssuedChequesListVM    IssuedCheques    { get; }
        public DateTime               StartDate        { get; set; } = DateTime.Now.AddDays(-10);

        public int AccountId => AccountNames.IndexOf(AccountName) + 1;


        public void OnAccountNameChanged()
        {
            AppArgs.AccountId   = AccountId;
            AppArgs.AccountName = AccountName;
        }


        private void FillAccountNames()
        {
            AccountNames.SetItems(AppArgs.DCDR.AccountNames);
            AccountName = AccountNames.FirstOrDefault();
        }


        internal SoaRowsRepo1 GetSoaRepo()
            => AppArgs.GetPassbookDB(AccountId).GetSoaRepo();


        protected override void OnRefreshClicked()
        {
            TransactionLog = new TransactionLogVM(GetSoaRepo(), AppArgs, StartDate);
            FundRequests   .ReloadFromDB();
            PreparedCheques.ReloadFromDB();
            IssuedCheques  .ReloadFromDB();
        }
    }
}
