using PassbookTally.DomainLib.DTOs;
using System;

namespace PassbookTally.DomainLib.ReportRows
{
    public class SoaRowVM
    {
        public SoaRowVM(SoaRowDTO soaRowDTO)
        {
            DTO = soaRowDTO;
        }

        public SoaRowDTO  DTO             { get; }
        public decimal    RunningBalance  { get; set; }

        public string    Subject          => DTO.Subject;
        public string    Description      => DTO.Description;
        public DateTime  TransactionDate  => DateTime.MinValue.AddDays(DTO.DateOffset);
        public string    TransactionRef   => DTO.TransactionRef;
        public bool      IsDeposit        => DTO.Amount >= 0;
        public bool      IsWithdrawal     => DTO.Amount < 0;
        public decimal?  Deposit          => IsDeposit ? DTO.Amount : (decimal?)null;
        public decimal?  Withdrawal       => IsWithdrawal ? Math.Abs(DTO.Amount) : (decimal?)null;
    }
}
