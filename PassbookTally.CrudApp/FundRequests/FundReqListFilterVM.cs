using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;

namespace PassbookTally.CrudApp.FundRequests
{
    public class FundReqListFilterVM : TextFilterBase<FundRequestDTO>
    {
        public string  FilterSerialNum    { get; set; }
        public string  FilterRequestDate  { get; set; }
        public string  FilterPayee        { get; set; }
        public string  FilterPurpose      { get; set; }
        public string  FilterAmount       { get; set; }

        protected override Dictionary<string, Func<FundRequestDTO, string>> FilterProperties => new Dictionary<string, Func<FundRequestDTO, string>>
        {
            { nameof(FilterSerialNum  ), _ => _.SerialNum.ToString()               },
            { nameof(FilterPayee      ), _ => _.Payee                              },
            { nameof(FilterPurpose    ), _ => _.Purpose                            },
            { nameof(FilterRequestDate), _ => _.RequestDate.ToString("d MMM yyyy") },
            { nameof(FilterAmount     ), _ => _.Amount.ToString()                  },
        };
    }
}
