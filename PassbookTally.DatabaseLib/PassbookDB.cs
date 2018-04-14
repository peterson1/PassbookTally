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


        public IEnumerable<string>  AccountNames  { get; private set; }


        //public void DepositTo(int bankAcctId, DateTime transactionDate,
        //    string subject, string description, decimal amount)
        //        => ForAccount(bankAcctId).Deposit(transactionDate, subject, amount, description);


        //public void WithdrawFrom(int bankAcctId, DateTime transactionDate,
        //    string subject, string description, decimal amount)
        //        => ForAccount(bankAcctId).Withdraw(transactionDate, subject, amount, description);


        public SoaRowsRepo1 ForAccount(int bankAcctId)
        {
            var key = $"Account{bankAcctId}_SoaRows";

            if (_soaReposDict.TryGetValue(key, out SoaRowsRepo1 repo))
                return repo;

            repo = new SoaRowsRepo1(bankAcctId, key, this);
            _soaReposDict.Add(key, repo);
            return repo;
        }


        private IEnumerable<string> GetAccountNames()
            => Metadata.Find    (_ => _.Name.Contains(ACCT_PREFIX))
                       .OrderBy (_ => _.Name)
                       .Select  (_ => _.Value);


        protected override void InitializeCollections()
        {
            AccountNames = GetAccountNames();
            if (!AccountNames.Any())
            {
                //Metadata[GetAccountNameKey(1)] = "Bank Account 1";
                ForAccount(1);
                AccountNames = GetAccountNames();
            }
        }


        internal string GetAccountNameKey(int bankAcctId)
            => $"{ACCT_PREFIX}{bankAcctId}";
    }
}
