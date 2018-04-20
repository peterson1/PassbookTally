using CommonTools.Lib11.DateTimeTools;
using PassbookTally.DatabaseLib.Repositories;
using System;
using System.IO;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    public class MonthlyPassbookDB : PassbookDB
    {
        public MonthlyPassbookDB(MonthlyDBsIndex index, DateTime date, string dbFilePath, string currentUser) : base(dbFilePath, currentUser)
        {
            Index     = index;
            MonthKey  = GetMonthKey(date);
            MonthDay1 = date.MonthFirstDay();
        }

        public MonthlyPassbookDB(MonthlyDBsIndex index, DateTime date, MemoryStream memoryStream, string currentUser) : base(memoryStream, currentUser)
        {
            Index     = index;
            MonthKey  = GetMonthKey(date);
            MonthDay1 = date.MonthFirstDay();
        }


        public MonthlyDBsIndex  Index      { get; }
        public string            MonthKey   { get; }
        public DateTime          MonthDay1  { get; }


        public SoaRowsRepo1 Repo 
            => ForAccount(Index.BankAccountId);


        public static string GetMonthKey(DateTime date)
            => date.ToString("yyyy-MM");
    }
}
