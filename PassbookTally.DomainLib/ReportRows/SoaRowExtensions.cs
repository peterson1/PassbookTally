using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DomainLib.ReportRows
{
    public static class SoaRowExtensions
    {
        public static decimal LastBalance(this IEnumerable<SoaRowDTO> rows)
            => rows.Last().RunningBalance;


        public static int SoaRowOffset(this DateTime date)
            => (date - DateTime.MinValue).Days;


        public static DateTime GetDate(this SoaRowDTO dto)
            => DateTime.MinValue.AddDays(dto.DateOffset);
    }
}
