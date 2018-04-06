using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;

namespace PassbookTally.DatabaseLib.Persistors
{
    internal class SoaRowsRepo1 : List<SoaRowDTO>
    {
        public DateTime  BaseDate     { get; set; }
        public decimal   BaseBalance  { get; set; }
    }
}
