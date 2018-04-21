using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Collections.Generic;

namespace PassbookTally.DatabaseLib.Repositories
{
    public partial class SoaRowsRepo1
    {
        public virtual List<SoaRowDTO> GetUpTo(DateTime maxDate)
        {
            var nextDay = maxDate.Date.AddDays(1).SoaRowOffset();
            return Find(_ => _.DateOffset < nextDay).SortRows();
        }


        public virtual List<SoaRowDTO> GetFrom(DateTime minDate)
        {
            var minOffset = minDate.Date.SoaRowOffset();
            return Find(_ => _.DateOffset >= minOffset).SortRows();
        }
    }
}
