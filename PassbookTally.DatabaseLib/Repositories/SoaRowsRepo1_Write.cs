using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Linq;

namespace PassbookTally.DatabaseLib.Repositories
{
    public partial class SoaRowsRepo1
    {
        public void SetBaseBalance(DateTime date, decimal openingBalance)
        {
            _db.Metadata[GetBaseDateKey   ()] = date.ToString(DATE_FMT);
            _db.Metadata[GetBaseBalanceKey()] = openingBalance.ToString();
            SetBaseValues();
        }


        public void Deposit(DateTime transactionDate, string subject, decimal amount, string description = null)
            => InsertAndUpdateBalances(SoaRowDTO
                .Deposit(transactionDate, subject, description, amount));


        public void Withdraw(DateTime transactionDate, string subject, decimal amount, string description = null)
            => InsertAndUpdateBalances(SoaRowDTO
                .Withdrawal(transactionDate, subject, description, amount));


        private void InsertAndUpdateBalances(SoaRowDTO dto)
        {
            Insert(dto);
            var rows = RowsStartingFrom(BaseDate).ToList();
            rows[0].RunningBalance = BaseBalance + rows[0].Amount;

            for (int i = 1; i < rows.Count; i++)
                rows[i].RunningBalance
                    = rows[i - 1].RunningBalance + rows[i].Amount;

            Update(rows);
        }
    }
}
