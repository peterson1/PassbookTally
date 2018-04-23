using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DatabaseLib.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PassbookTally.DatabaseLib
{
    public class PassbookDB : SharedLiteDB
    {
        private const string ACCT_PREFIX = "Acct_";


        public PassbookDB(int bankAcctId, string dbFilePath, string currentUser) : base(dbFilePath, currentUser, false)
        {
            BankAccountId = bankAcctId;
            InitializeCollections();
        }

        public PassbookDB(int bankAcctId, MemoryStream memoryStream, string currentUser) : base(memoryStream, currentUser, false)
        {
            BankAccountId = bankAcctId;
            InitializeCollections();
        }


        public int                   BankAccountId     { get; } = 1;
        public List<string>          AccountNames      { get; } = new List<string>();
        public List<int>             AccountIDs        { get; } = new List<int>();
        public ActiveFundReqsRepo    ActiveRequests    { get; private set; }
        public InactiveFundReqsRepo  InactiveRequests  { get; private set; }


        private void EnsureBaseBalances()
        {
            Metadata.AddIfNone(AccountNameKey, $"Bank Account {BankAccountId}");
            Metadata.AddIfNone(BaseBalanceKey, "0");
            Metadata.AddIfNone(BaseDateKey   , "2018-01-01");
        }


        private IEnumerable<string> GetAccountNames()
            => Metadata.Find   (_ => _.Name.Contains(ACCT_PREFIX))
                       .OrderBy(_ => _.Name)
                       .Select (_ => _.Value);




        public SoaRowsRepo1 GetSoaRepo(decimal? baseBalance = null, DateTime? baseDate = null)
        {
            var bseBal  = baseBalance ?? decimal.Parse(Metadata[BaseBalanceKey]);
            var bseDate = baseDate    ?? DateTime.Parse(Metadata[BaseDateKey]);

            return CreateRepoInstance(bseBal, bseDate);
        }


        protected virtual SoaRowsRepo1 CreateRepoInstance(decimal baseBalance, DateTime baseDate) 
            => new SoaRowsRepo1(baseBalance, baseDate, this);


        internal string RepoKey
            => $"Account{BankAccountId}_SoaRows";


        protected string AccountNameKey => $"{ACCT_PREFIX}{BankAccountId}";
        protected string BaseDateKey    => $"Acct{BankAccountId}_BaseDate";
        protected string BaseBalanceKey => $"Acct{BankAccountId}_BaseBalance";


        protected override void InitializeCollections()
        {
            EnsureBaseBalances();

            ActiveRequests   = new ActiveFundReqsRepo(this);
            InactiveRequests = new InactiveFundReqsRepo(this);

            AccountNames.Clear();
            AccountNames.AddRange(GetAccountNames());
            if (!AccountNames.Any())
            {
                GetSoaRepo();
                AccountNames.Clear();
                AccountNames.AddRange(this.GetAccountNames());
                AccountIDs.Clear();
                AccountIDs.AddRange(AccountNames.Select((s, i) => i + 1));
            }
        }
    }
}
