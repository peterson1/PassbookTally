using PassbookTally.DomainLib.DTOs;
using System;
using static PassbookTally.DomainLib.DTOs.FundRequestDTO;

namespace PassbookTally.CrudApp.FundRequests
{
    public class AllocationVM
    {
        public AllocationVM(GLAccountDTO glAccount, decimal amount)
        {
            DTO = new AccountAllocation
            {
                Account   = glAccount,
                SubAmount = amount,
            };
        }


        public AccountAllocation DTO { get; }


        public string    Account => DTO?.Account?.Name;
        public decimal?  Debit   => PolarizeAmount(_ => _ < 0);
        public decimal?  Credit  => PolarizeAmount(_ => _ > 0);


        private decimal? PolarizeAmount(Predicate<decimal?> predicate)
        {
            var val = DTO?.SubAmount;
            if (!val.HasValue) return null;
            return predicate(val) 
                ? val.Value : (decimal?)null;
        }
    }
}
