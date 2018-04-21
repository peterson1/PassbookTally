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

        private Dictionary<string, SoaRowsRepo1> _soaReposDict = new Dictionary<string, SoaRowsRepo1>();


        public PassbookDB(int bankAcctId, string dbFilePath, string currentUser) : base(dbFilePath, currentUser, false)
        {
            BankAccountId = bankAcctId;
            InitializeCommons(currentUser);
        }

        public PassbookDB(int bankAcctId, MemoryStream memoryStream, string currentUser) : base(memoryStream, currentUser, false)
        {
            BankAccountId = bankAcctId;
            InitializeCommons(currentUser);
        }


        public int                   BankAccountId     { get; } = 1;
        public List<string>          AccountNames      { get; } = new List<string>();
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




        public SoaRowsRepo1 GetRepo(decimal? baseBalance = null, DateTime? baseDate = null)
        {
            if (_soaReposDict.TryGetValue(RepoKey, out SoaRowsRepo1 repo))
                return repo;

            var bseBal  = baseBalance ?? decimal .Parse(Metadata[BaseBalanceKey]);
            var bseDate = baseDate    ?? DateTime.Parse(Metadata[BaseDateKey]);

            repo = new SoaRowsRepo1(bseBal, bseDate, this);
            _soaReposDict.Add(RepoKey, repo);
            return repo;
        }


        internal string RepoKey
            => $"Account{BankAccountId}_SoaRows";


        private string AccountNameKey => $"{ACCT_PREFIX}{BankAccountId}";
        private string BaseDateKey    => $"Acct{BankAccountId}_BaseDate";
        private string BaseBalanceKey => $"Acct{BankAccountId}_BaseBalance";


        protected override void InitializeCollections()
        {
            EnsureBaseBalances();

            ActiveRequests   = new ActiveFundReqsRepo(this);
            InactiveRequests = new InactiveFundReqsRepo(this);

            AccountNames.Clear();
            AccountNames.AddRange(GetAccountNames());
            if (!AccountNames.Any())
            {
                GetRepo();
                AccountNames.Clear();
                AccountNames.AddRange(this.GetAccountNames());
            }
        }
    }
}
