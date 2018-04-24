using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;

namespace PassbookTally.CrudApp.PreparedCheques
{
    public class PreparedChequesFilterVM : TextFilterBase<RequestedChequeDTO>
    {
        public string  FilterSerialNum    { get; set; }
        public string  FilterChequeNumber { get; set; }
        public string  FilterChequeDate   { get; set; }
        public string  FilterPayee        { get; set; }
        public string  FilterPurpose      { get; set; }
        public string  FilterAmount       { get; set; }


        protected override Dictionary<string, Func<RequestedChequeDTO, string>> FilterProperties => new Dictionary<string, Func<RequestedChequeDTO, string>>
        {
            { nameof(FilterSerialNum   ), _ => _.Request.SerialNum.ToString()     },
            { nameof(FilterChequeNumber), _ => _.ChequeNumber.ToString()},
            { nameof(FilterChequeDate  ), _ => _.ChequeDate.ToString("d MMM yyyy")},
            { nameof(FilterPayee       ), _ => _.Request.Payee                    },
            { nameof(FilterPurpose     ), _ => _.Request.Purpose                  },
            { nameof(FilterAmount      ), _ => _.Request.Amount.ToString()        },
        };
    }
}
