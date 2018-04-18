using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DomainLib.ReportRows
{
    public static class SoaRowExtensions
    {
        public static IOrderedEnumerable<SoaRowDTO> Sort(this IEnumerable<SoaRowDTO> unsorted)
            => unsorted.OrderBy(_ => _.DateOffset)
                       .ThenBy (_ => _.Id);


        public static decimal LastBalance(this IEnumerable<SoaRowDTO> rows)
            => rows.LastRow().RunningBalance;


        public static SoaRowDTO LastRow(this IEnumerable<SoaRowDTO> rows)
            => rows.Sort().Last();


        public static int SoaRowOffset(this DateTime date)
            => (date - DateTime.MinValue).Days;


        public static DateTime GetDate(this SoaRowDTO dto)
            => DateTime.MinValue.AddDays(dto.DateOffset);
    }
}
