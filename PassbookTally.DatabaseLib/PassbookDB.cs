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
        private Dictionary<string, SoaRowsRepo1> _soaReposDict = new Dictionary<string, SoaRowsRepo1>();


        public PassbookDB(string dbFilePath, string currentUser) : base(dbFilePath, currentUser)
        {
        }

        public PassbookDB(MemoryStream memoryStream, string currentUser) : base(memoryStream, currentUser)
        {
        }


        public List<string>          AccountNames      { get; } = new List<string>();
        public ActiveFundReqsRepo    ActiveRequests    { get; private set; }
        public InactiveFundReqsRepo  InactiveRequests  { get; private set; }


        public SoaRowsRepo1 ForAccount(int bankAcctId)
        {
            var key = $"Account{bankAcctId}_SoaRows";

            if (_soaReposDict.TryGetValue(key, out SoaRowsRepo1 repo))
                return repo;

            repo = new SoaRowsRepo1(bankAcctId, key, this);
            _soaReposDict.Add(key, repo);
            return repo;
        }



        protected override void InitializeCollections()
        {
            ActiveRequests   = new ActiveFundReqsRepo(this);
            InactiveRequests = new InactiveFundReqsRepo(this);

            AccountNames.Clear();
            AccountNames.AddRange(this.GetAccountNames());
            if (!AccountNames.Any())
            {
                ForAccount(1);
                AccountNames.Clear();
                AccountNames.AddRange(this.GetAccountNames());
            }
        }
    }
}
