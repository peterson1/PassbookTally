using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Collections.Generic;

namespace PassbookTally.CrudApp.TransactionLog
{
    public class TransactionLogFilterVM : TextFilterBase<SoaRowVM>
    {
        public string  FilterTransactionDate  { get; set; }
        public string  FilterTransactionRef   { get; set; }
        public string  FilterSubject          { get; set; }
        public string  FilterDescription      { get; set; }
        public string  FilterDeposit          { get; set; }
        public string  FilterWithdrawal       { get; set; }
        public string  FilterRunningBalance   { get; set; }

        protected override Dictionary<string, Func<SoaRowVM, string>> FilterProperties => new Dictionary<string, Func<SoaRowVM, string>>
        {
            { nameof(FilterTransactionDate), _ => _.TransactionDate.ToString("d MMM") },
            { nameof(FilterTransactionRef ), _ => _.TransactionRef            },
            { nameof(FilterSubject        ), _ => _.Subject                   },
            { nameof(FilterDescription    ), _ => _.Description               },
            { nameof(FilterDeposit        ), _ => _.Deposit       .ToString() },
            { nameof(FilterWithdrawal     ), _ => _.Withdrawal    .ToString() },
            { nameof(FilterRunningBalance ), _ => _.RunningBalance.ToString() }
        };
    }
}
