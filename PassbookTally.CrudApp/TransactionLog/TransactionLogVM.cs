using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.LiteDbTools;
using CommonTools.Lib45.ThreadTools;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.CrudApp.TransactionLog
{
    [AddINotifyPropertyChangedInterface]
    public class TransactionLogVM : SavedListVMBase<SoaRowDTO>
    {
        public TransactionLogVM(SharedCollectionBase<SoaRowDTO> sharedCollection, bool doReload = true) : base(sharedCollection, doReload)
        {
        }


        public UIList<SoaRowVM> Rows { get; } = new UIList<SoaRowVM>();


        protected override IEnumerable<SoaRowDTO> QueryItems(SharedCollectionBase<SoaRowDTO> db)
        {
            var dtos = db.GetAll();
            Rows.SetItems(dtos.Select(_ => new SoaRowVM(_)));
            return dtos;
        }


        protected override Func<SoaRowDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
