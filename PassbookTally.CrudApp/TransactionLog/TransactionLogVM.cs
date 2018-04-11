using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassbookTally.CrudApp.TransactionLog
{
    public class TransactionLogVM : SavedListVMBase<SoaRowDTO>
    {
        public TransactionLogVM(SharedCollectionBase<SoaRowDTO> sharedCollection, bool doReload = true) : base(sharedCollection, doReload)
        {
        }

        protected override Func<SoaRowDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
