using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DomainLib.ReportRows
{
    public static class SoaRowVMExtensions
    {
        public static decimal LastBalance(this IEnumerable<SoaRowVM> rows)
            => rows.Last().RunningBalance;
    }
}
