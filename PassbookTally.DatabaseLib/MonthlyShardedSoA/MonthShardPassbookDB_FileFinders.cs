using CommonTools.Lib11.DateTimeTools;
using PassbookTally.DatabaseLib.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace PassbookTally.DatabaseLib.MonthlyShardedSoA
{
    public static class MonthShardPassbookDB_FileFinders
    {
        internal static Dictionary<DateTime, SoaRowsRepo1> CreateMemoryBasedIndex(this MonthShardPassbookDB monthDb, int dictLength = 10)
        {
            var dict     = new Dictionary<DateTime, SoaRowsRepo1>();
            var monthNow = DateTime.Now.MonthFirstDay();
            var origRepo = monthDb.GetSoaRepo();

            for (int i = 0; i < dictLength; i++)
            {
                var db   = new PassbookDB(monthDb.BankAccountId, new MemoryStream(), monthDb.CurrentUser);
                var repo = new SoaRowsRepo1(origRepo.BaseBalance, origRepo.BaseDate, db);
                dict.Add(monthNow.AddMonths(i * -1), repo);
            }
            return dict;
        }


        internal static Dictionary<DateTime, SoaRowsRepo1> CreateFileBasedIndex(this MonthShardPassbookDB db)
        {
            throw new NotImplementedException();
        }
    }
}
