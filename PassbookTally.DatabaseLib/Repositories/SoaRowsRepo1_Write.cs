using PassbookTally.DomainLib.DTOs;
using System;
using System.Collections.Generic;

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
            //Upsert(dto);
            InsertOrUpdate(dto);
            var rows = GetFrom(BaseDate);
            rows[0].RunningBalance = BaseBalance + rows[0].Amount;

            for (int i = 1; i < rows.Count; i++)
                rows[i].RunningBalance
                    = rows[i - 1].RunningBalance + rows[i].Amount;

            BulkUpdate(rows);
        }

        protected virtual void InsertOrUpdate(SoaRowDTO dto)
            => Upsert(dto);

        public virtual void BulkUpdate(IEnumerable<SoaRowDTO> rows) 
            => Update(rows);
    }
}
