using CommonTools.Lib11.DataStructures;
using PassbookTally.CrudApp.TransactionLog;
using PassbookTally.DomainLib45.BaseViewModels;
using PassbookTally.DomainLib45.Configuration;
using System;
using System.Collections.Generic;

namespace PassbookTally.CrudApp
{
    internal class MainWindowVM : MaterialWindowBase
    {
        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            AccountNames.SetItems(GetAccountNames());
        }


        public UIList<string>    AccountNames    { get; } = new UIList<string>();
        public string            AccountName     { get; set; }
        public TransactionLogVM  TransactionLog  { get; private set; }


        public int AccountId => AccountNames.IndexOf(AccountName);


        private IEnumerable<string> GetAccountNames()
        {
            throw new NotImplementedException();
        }
    }
}
