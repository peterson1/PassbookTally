using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;

namespace PassbookTally.DatabaseLib.MonthlySoaRows
{
    public static class MonthlyDBsIndex_Write
    {
        public static void Insert(this MonthlyDBsIndex db, SoaRowDTO dto) 
            => db.GetRepo(dto.GetDate()).Insert(dto);


        public static void SetBaseBalance(this MonthlyDBsIndex db, DateTime date, decimal openingBalance)
            => db.GetRepo(date).SetBaseBalance(date, openingBalance);


        public static void Deposit(this MonthlyDBsIndex db, DateTime transactionDate, string subject, string description, decimal amount, string transactionRef)
            => db.GetRepo(transactionDate).Deposit(transactionDate, subject, description, amount, transactionRef);


        public static void Withdraw(this MonthlyDBsIndex db, DateTime transactionDate, string subject, string description, decimal amount, string transactionRef)
            => db.GetRepo(transactionDate).Withdraw(transactionDate, subject, description, amount, transactionRef);
    }
}
