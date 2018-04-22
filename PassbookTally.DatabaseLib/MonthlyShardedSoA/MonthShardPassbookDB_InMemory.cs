using CommonTools.Lib11.DateTimeTools;
using PassbookTally.DatabaseLib.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace PassbookTally.DatabaseLib.MonthlyShardedSoA
{
    internal static class MonthShardPassbookDB_InMemory
    {
        internal static Dictionary<DateTime, SoaRowsRepo1> CreateMemoryBasedIndex(this MonthShardPassbookDB monthDb, int dictLength = 10)
        {
            var dict     = new Dictionary<DateTime, SoaRowsRepo1>();
            var monthNow = DateTime.Now.MonthFirstDay();

            for (int i = 0; i < dictLength; i++)
            {
                var repo = monthDb.CreateInMemoryShard();
                dict.Add(monthNow.AddMonths(i * -1), repo);
            }
            return dict;
        }


        internal static SoaRowsRepo1 CreateInMemoryShard(this MonthShardPassbookDB monthDb)
        {
            var moRepo = monthDb.GetSoaRepo();
            var pbkDB  = new PassbookDB(monthDb.BankAccountId, new MemoryStream(), monthDb.CurrentUser);
            return new SoaRowsRepo1(moRepo.BaseBalance, moRepo.BaseDate, pbkDB);
        }
    }
}
