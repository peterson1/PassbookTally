using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DatabaseLib.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PassbookTally.DatabaseLib
{
    public class PassbookDB : SharedLiteDB
    {
        private Dictionary<string, SoaRowsRepo1> _soaReposDict = new Dictionary<string, SoaRowsRepo1>();
        private const string ACCT_PREFIX = "Acct_";


        public PassbookDB(string dbFilePath, string currentUser) : base(dbFilePath, currentUser)
        {
        }

        public PassbookDB(MemoryStream memoryStream, string currentUser) : base(memoryStream, currentUser)
        {
        }


        public List<string>        AccountNames    { get; } = new List<string>();
        public ActiveFundReqsRepo  ActiveRequests  { get; private set; }


        public SoaRowsRepo1 ForAccount(int bankAcctId)
        {
            var key = $"Account{bankAcctId}_SoaRows";

            if (_soaReposDict.TryGetValue(key, out SoaRowsRepo1 repo))
                return repo;

            repo = new SoaRowsRepo1(bankAcctId, key, this);
            _soaReposDict.Add(key, repo);
            return repo;
        }


        public int NextRequestSerial()
        {
            var activesMax = ActiveRequests.GetMaxSerial();
            //todo: get max for inactives
            return activesMax + 1;
        }


        private IEnumerable<string> GetAccountNames()
            => Metadata.Find    (_ => _.Name.Contains(ACCT_PREFIX))
                       .OrderBy (_ => _.Name)
                       .Select  (_ => _.Value);


        protected override void InitializeCollections()
        {
            ActiveRequests = new ActiveFundReqsRepo(this);

            AccountNames.Clear();
            AccountNames.AddRange(GetAccountNames());
            if (!AccountNames.Any())
            {
                ForAccount(1);
                AccountNames.Clear();
                AccountNames.AddRange(GetAccountNames());
            }
        }


        internal string GetAccountNameKey(int bankAcctId)
            => $"{ACCT_PREFIX}{bankAcctId}";
    }
}
