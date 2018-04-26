using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib45.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.CrudApp.FundRequests
{
    public class AllocationsListVM
    {
        private FundReqCrudVM      _crud;
        private List<GLAccountDTO> _accts;


        public AllocationsListVM(FundReqCrudVM fundReqCrudVM)
        {
            _crud        = fundReqCrudVM;
            _accts       = _crud.AppArgs.DCDR.GLAccounts.GetAll();
            AddDebitCmd  = R2Command.Relay(_ => AddNewItem("Debit" , -1), null, "Add Debit entry");
            AddCreditCmd = R2Command.Relay(_ => AddNewItem("Credit", +1), null, "Add Credit entry");
        }


        public IR2Command            AddDebitCmd   { get; }
        public IR2Command            AddCreditCmd  { get; }
        public UIList<AllocationVM>  Items         { get; } = new UIList<AllocationVM>();

        public decimal TotalDebit  => Items.Sum(_ => _.Debit  ?? 0);
        public decimal TotalCredit => Items.Sum(_ => _.Credit ?? 0);


        private void AddNewItem(string amtType, decimal multiplier)
        {
            if (!PopUpInput.TryGetIndex("GL Account", out int index, _accts)) return;
            if (!PopUpInput.TryGetDecimal($"{amtType} Amount", out decimal amount, GetSuggestedAmount())) return;
            Items.Add(new AllocationVM(_accts[index], amount * multiplier));
        }


        private decimal? GetSuggestedAmount()
        {
            var total = _crud.Draft.Amount;
            if (!Items.Any()) return total;
            return Math.Abs(TotalCredit - TotalDebit);
        }
    }
}
