using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    public static class MonthlySoaRowsDB_Read
    {
        public static decimal ClosingBalanceFor(this MonthlySoaRowsDB db, DateTime date)
            => db.GetRepo(date).ClosingBalanceFor(date);


        public static IEnumerable<SoaRowDTO> RowsStartingFrom(this MonthlySoaRowsDB db, DateTime date)
            => db.GetRepo(date).RowsStartingFrom(date);
    }
}
