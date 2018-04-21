using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DomainLib.ReportRows
{
    public static class SoaRowExtensions
    {
        public static List<SoaRowDTO> SortRows(this List<SoaRowDTO> unsorted)
            => unsorted.OrderBy(_ => _.DateOffset)
                       .ThenBy (_ => _.Id)
                       .ToList ();


        public static int SoaRowOffset(this DateTime date)
            => (date.Date - DateTime.MinValue).Days;


        public static DateTime GetDate(this SoaRowDTO dto)
            => DateTime.MinValue.AddDays(dto.DateOffset);
    }
}
