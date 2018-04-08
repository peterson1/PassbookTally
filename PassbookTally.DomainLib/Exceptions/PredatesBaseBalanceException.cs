using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;

namespace PassbookTally.DomainLib.Exceptions
{
    public class PredatesBaseBalanceException : Exception
    {
        public PredatesBaseBalanceException(SoaRowDTO dto, DateTime baseBalDate)
            : base(GetMessage(dto, baseBalDate))
        {
        }


        private static string GetMessage(SoaRowDTO dto, DateTime baseBalDate)
        {
            var dtoDesc = $"[{dto.GetDate():d MMM yyyy}] “{dto.Subject}”";
            return $"{dtoDesc} predates base balance ({baseBalDate:d MMM yyyy})";
        }
    }
}
