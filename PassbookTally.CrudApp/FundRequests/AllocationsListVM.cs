using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.CrudApp.FundRequests
{
    [AddINotifyPropertyChangedInterface]
    public class AllocationsListVM
    {
        private FundReqCrudVM      _crud;
        private List<GLAccountDTO> _accts;
        private AppArguments       _arg;


        public AllocationsListVM(FundReqCrudVM fundReqCrudVM)
        {
            _crud        = fundReqCrudVM;
            _arg         = _crud.AppArgs;
            _accts       = _arg.DCDR.GLAccounts.GetAll();
            AddDebitCmd  = R2Command.Relay(_ => AddNewItem("Debit" , -1), null, "Add Debit entry");
            AddCreditCmd = R2Command.Relay(_ => AddNewItem("Credit", +1), null, "Add Credit entry");
            Items.Add(AllocationVM.CashInBank(_arg.AccountName));
        }


        public IR2Command            AddDebitCmd   { get; }
        public IR2Command            AddCreditCmd  { get; }
        public UIList<AllocationVM>  Items         { get; } = new UIList<AllocationVM>();
        public bool                  CanAddItem    { get; private set; }

        public decimal TotalDebit  => Items.Sum(_ => _.Debit  ?? 0);
        public decimal TotalCredit => Items.Sum(_ => _.Credit ?? 0);


        internal void UpdateBaseAmount(decimal? amount)
        {
            Items.RemoveAt(0);
            Items.Insert(0, AllocationVM.CashInBank(_arg.AccountName, amount));
            CanAddItem = amount.HasValue && amount > 0;
        }


        private void AddNewItem(string amtType, decimal multiplier)
        {
            if (!PopUpInput.TryGetIndex($"GL Account ({amtType})", out int index, _accts)) return;
            if (!PopUpInput.TryGetDecimal($"{amtType} Amount", out decimal amount, GetSuggestedAmount())) return;
            Items.Add(new AllocationVM(_accts[index], amount * multiplier));
        }


        //todo: correct this
        private decimal? GetSuggestedAmount()
        {
            var total = _crud.Draft.Amount;
            if (!Items.Any()) return total;
            return Math.Abs(TotalCredit - TotalDebit);
        }
    }
}
