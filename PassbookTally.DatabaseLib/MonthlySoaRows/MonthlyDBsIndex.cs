using PassbookTally.DatabaseLib.Repositories;
using CommonTools.Lib11.DateTimeTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    public class MonthlyDBsIndex
    {
        private Dictionary<DateTime, MonthlyPassbookDB> _dict = new Dictionary<DateTime, MonthlyPassbookDB>();


        public MonthlyDBsIndex(int bankAcctId, PassbookDB passbookDB)
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


        private MonthlyPassbookDB GetMonthDB(DateTime date)
            => MainDB.IsInMemory ? GetInMemoryMonthDB  (date)
                                 : GetPersistentMonthDB(date);


        private MonthlyPassbookDB GetInMemoryMonthDB(DateTime date)
        {
            var day1 = date.MonthFirstDay();
            if (!_dict.TryGetValue(day1, out MonthlyPassbookDB db))
            {
                db = new MonthlyPassbookDB(this, date, new MemoryStream(), MainDB.CurrentUser);
                db.AccountNames.Clear();
                db.AccountNames.AddRange(MainDB.AccountNames);
                this.SetBaseBalance(db);
                _dict.Add(day1, db);
            }
            return db;
        }




        private MonthlyPassbookDB GetPersistentMonthDB(DateTime date)
        {
            throw new NotImplementedException();
        }

    }
}
