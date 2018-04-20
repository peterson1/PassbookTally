using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    internal static class MonthlyDBsIndex_BaseBalance
    {
        internal static void SetBaseBalance(this MonthlyDBsIndex idx, MonthlyPassbookDB monthDB)
        {
            var prevMonth = monthDB.MonthDay1.AddDays(-1);
            var prevRepo  = idx.GetRepo(prevMonth);
            var lastBal   = prevRepo.ClosingBalanceFor(prevMonth);
            monthDB.Repo.SetBaseBalance(prevMonth, lastBal);
        }
    }
}
