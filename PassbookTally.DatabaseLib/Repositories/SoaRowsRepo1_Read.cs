using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DatabaseLib.Repositories
{
    public partial class SoaRowsRepo1
    {
        public IEnumerable<SoaRowDTO> RowsStartingFrom(DateTime date)
            => Find(_ => _.DateOffset >= date.SoaRowOffset()).Sort();


        public decimal ClosingBalanceFor(DateTime date)
        {
            if (date < BaseDate) return 0;
            if (!Any()) return BaseBalance;
            if (CountAll() == 1) return GetAll().Single().RunningBalance;

            var nextDay = date.AddDays(1).SoaRowOffset();
            return Find(_ => _.DateOffset < nextDay).LastBalance();
        }
    }
}
