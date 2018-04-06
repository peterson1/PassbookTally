using CommonTools.Lib.ns11.ExceptionTools;
using System;

namespace PassbookTally.DomainLib.DTOs
{
    public class SoaRowDTO
    {
        public int       Id               { get; set; }
        public string    Author           { get; set; }
        public DateTime  Timestamp        { get; set; }

        public string    Subject          { get; set; }
        public string    Description      { get; set; }
        public int       DateOffset       { get; set; } //todo: index this
        public string    TransactionRef   { get; set; }
        public decimal   Amount           { get; set; }
        public string    DocRefType       { get; set; }
        public int       DocRefId         { get; set; }


        private static int GetOffset(DateTime date)
            => (date - DateTime.MinValue).Days;


        public static SoaRowDTO Deposit(DateTime transactionDate, 
            string subject, string description, decimal amount)
                => GetSoaRowDTO(transactionDate, subject, description, amount, "Deposit", 1);


        public static SoaRowDTO Withdrawal(DateTime transactionDate,
            string subject, string description, decimal amount)
                => GetSoaRowDTO(transactionDate, subject, description, amount, "Withdrawal", -1);


        private static SoaRowDTO GetSoaRowDTO(DateTime transactionDate, 
            string subject, string description, decimal amount, 
            string txnType, decimal multiplier)
        {
            if (amount <= 0) throw Fault.BadArg($"{txnType} Amount", amount);
            return new SoaRowDTO
            {
                DateOffset  = GetOffset(transactionDate),
                Subject     = subject,
                Description = description,
                Amount      = amount * multiplier
            };
        }
    }
}
