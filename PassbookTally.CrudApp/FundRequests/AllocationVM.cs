using PassbookTally.DomainLib.DTOs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using static PassbookTally.DomainLib.DTOs.FundRequestDTO;

namespace PassbookTally.CrudApp.FundRequests
{
    [AddINotifyPropertyChangedInterface]
    public class AllocationVM
    {
        public AllocationVM(AccountAllocation accountAllocationDTO)
        {
            DTO = accountAllocationDTO;
        }

        public AllocationVM(GLAccountDTO glAccount, decimal amount)
            : this(new AccountAllocation
            {
                Account   = glAccount,
                SubAmount = amount
            }
            ) { }


        public AccountAllocation  DTO  { get; }

        public virtual string    Account => DTO?.Account?.Name;
        public virtual decimal?  Debit   => PolarizeAmount(_ => _ < 0);
        public virtual decimal?  Credit  => PolarizeAmount(_ => _ > 0);


        private decimal? PolarizeAmount(Predicate<decimal?> predicate)
        {
            var val = DTO?.SubAmount;
            if (!val.HasValue) return null;
            return predicate(val) 
                ? Math.Abs(val.Value) : (decimal?)null;
        }


        internal static AllocationVM CashInBank(string accountName, decimal? amount = null)
            => new AllocationVM(new GLAccountDTO
            {
                AccountType = GLAcctType.Asset,
                Name        = $"Cash in Bank - {accountName}"
            }, 
            amount ?? 0);
    }


    public class AllocationVMTotal : AllocationVM
    {
        public AllocationVMTotal(IEnumerable<AllocationVM> items) : base(null)
        {
            Account = "total";
            Debit   = items.Sum(_ => _.Debit);
            Credit  = items.Sum(_ => _.Credit);
        }

        public override string    Account { get; }
        public override decimal?  Debit   { get; }
        public override decimal?  Credit  { get; }
    }
}
