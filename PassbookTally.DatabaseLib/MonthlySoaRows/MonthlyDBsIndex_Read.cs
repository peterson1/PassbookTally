using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    public static class MonthlyDBsIndex_Read
    {
        public static decimal ClosingBalanceFor(this MonthlyDBsIndex db, DateTime date)
            => db.GetRepo(date).ClosingBalanceFor(date);


        public static IEnumerable<SoaRowDTO> RowsStartingFrom(this MonthlyDBsIndex db, DateTime date)
            => db.GetRepo(date).RowsStartingFrom(date);
    }
}
