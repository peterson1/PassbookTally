using PassbookTally.DatabaseLib.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    public class MonthlySoaRowsDB
    {
        private Dictionary<string, PassbookDB> _dict = new Dictionary<string, PassbookDB>();


        public MonthlySoaRowsDB(int bankAcctId, PassbookDB passbookDB)
        {
            MainDB        = passbookDB;
            BankAccountId = bankAcctId;
        }


        public PassbookDB  MainDB         { get; }
        public int         BankAccountId  { get; }


        internal SoaRowsRepo1 GetRepo(DateTime date)
            => new SoaRowsRepo1(BankAccountId,
                                MainDB.GetKey(BankAccountId),
                                GetMonthDB(date));


        private PassbookDB GetMonthDB(DateTime date)
            => MainDB.IsInMemory ? GetInMemoryMonthDB  (date)
                                 : GetPersistentMonthDB(date);


        private PassbookDB GetInMemoryMonthDB(DateTime date)
        {
            var key = GetMonthKey(date);

            if (!_dict.TryGetValue(key, out PassbookDB db))
            {
                db = new PassbookDB(new MemoryStream(), MainDB.CurrentUser);
                db.AccountNames.Clear();
                db.AccountNames.AddRange(MainDB.AccountNames);
                _dict.Add(key, db);
            }
            return db;
        }


        private static string GetMonthKey(DateTime date)
            => date.ToString("yyyy-MM");


        private PassbookDB GetPersistentMonthDB(DateTime date)
        {
            throw new NotImplementedException();
        }

    }
}
