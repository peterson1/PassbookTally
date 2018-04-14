using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PassbookTally.CrudApp.TransactionLog;
using PassbookTally.DatabaseLib;
using PassbookTally.DomainLib45.BaseViewModels;
using PassbookTally.DomainLib45.Configuration;
using System.Linq;

namespace PassbookTally.CrudApp
{
    internal class MainWindowVM : MaterialWindowBase
    {
        private PassbookDB _db;

        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            _db = AppArgs.PassbookDB;
            AccountNames.SetItems(_db.AccountNames);
            AccountName = AccountNames.FirstOrDefault();
        }


        public UIList<string>    AccountNames    { get; } = new UIList<string>();
        public string            AccountName     { get; set; }
        public TransactionLogVM  TransactionLog  { get; private set; }


        public int AccountId => AccountNames.IndexOf(AccountName) + 1;


        protected override void OnRefreshClicked()
        {
            var repo       = _db.ForAccount(AccountId);
            TransactionLog = new TransactionLogVM(repo);
        }
    }
}
