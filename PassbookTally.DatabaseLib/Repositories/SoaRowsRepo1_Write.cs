using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DatabaseLib.Repositories
{
    public partial class SoaRowsRepo1
    {


        public void Deposit(DateTime transactionDate, string subject, string description, decimal amount, string transactionRef)
            => UpsertAndUpdateBalances(SoaRowDTO
                .Deposit(transactionDate, subject, description, amount, transactionRef));


        public void Withdraw(DateTime transactionDate, string subject, string description, decimal amount, string transactionRef)
            => UpsertAndUpdateBalances(SoaRowDTO
                .Withdrawal(transactionDate, subject, description, amount, transactionRef));


        public void UpsertAndUpdateBalances(SoaRowDTO dto)
        {
            InsertOrUpdate(dto);
            UpdateAllBalances();
        }


        public void DeleteAndUpdateBalances(SoaRowDTO dto)
        {
            Remove(dto);
            UpdateAllBalances();
        }


        private void UpdateAllBalances()
        {
            var rows = GetFrom(BaseDate);
            if (!rows.Any()) return;
            rows[0].RunningBalance = BaseBalance + rows[0].Amount;

            for (int i = 1; i < rows.Count; i++)
                rows[i].RunningBalance
                    = rows[i - 1].RunningBalance + rows[i].Amount;

            BulkUpdate(rows);
        }


        protected virtual void Remove(SoaRowDTO dto)
            => Delete(dto);


        protected virtual void InsertOrUpdate(SoaRowDTO dto)
            => Upsert(dto);

        public virtual void BulkUpdate(IEnumerable<SoaRowDTO> rows) 
            => Update(rows);
    }
}
