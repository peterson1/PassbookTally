﻿using CommonTools.Lib11.DataStructures;
using PassbookTally.CrudApp.FundRequests;
using PassbookTally.CrudApp.TransactionLog;
using PassbookTally.DatabaseLib;
using PassbookTally.DomainLib45.BaseViewModels;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Linq;

namespace PassbookTally.CrudApp
{
    [AddINotifyPropertyChangedInterface]
    internal class MainWindowVM : MaterialWindowBase
    {
        private PassbookDB _db;

        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            _db = AppArgs.PassbookDB;
            AccountNames.SetItems(_db.AccountNames);
            AccountName  = AccountNames.FirstOrDefault();
            FundRequests = new FundReqListVM(_db, AppArgs);
            ClickRefresh();
        }


        public UIList<string>    AccountNames    { get; } = new UIList<string>();
        public string            AccountName     { get; set; }
        public TransactionLogVM  TransactionLog  { get; private set; }
        public FundReqListVM     FundRequests    { get; }
        public DateTime          StartDate       { get; set; } = DateTime.Now.AddDays(-10);


        public int AccountId => AccountNames.IndexOf(AccountName) + 1;


        protected override void OnRefreshClicked()
        {
            var repo       = _db.ForAccount(AccountId);
            TransactionLog = new TransactionLogVM(repo, AppArgs, StartDate);
            FundRequests.ReloadFromDB();
        }
    }
}
