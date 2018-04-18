using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.ReflectionTools;
using PassbookTally.DomainLib.ReportRows;
using System;

namespace PassbookTally.DomainLib.DTOs
{
    public class SoaRowDTO : IDocumentDTO, ICloneable
    {
        public int       Id               { get; set; }
        public string    Author           { get; set; }
        public DateTime  Timestamp        { get; set; }
        public string    Remarks          { get; set; }

        public string    Subject          { get; set; }
        public string    Description      { get; set; }
        public int       DateOffset       { get; set; }
        public string    TransactionRef   { get; set; }
        public decimal   Amount           { get; set; }
        public decimal   RunningBalance   { get; set; }//??
        public string    DocRefType       { get; set; }
        public int       DocRefId         { get; set; }


        public static SoaRowDTO Deposit(DateTime transactionDate, 
            string subject, string description, decimal amount, string transactionRef)
                => GetSoaRowDTO(transactionDate, subject, description, amount, 
                        transactionRef, "Deposit", 1);


        public static SoaRowDTO Withdrawal(DateTime transactionDate,
            string subject, string description, decimal amount, string transactionRef)
                => GetSoaRowDTO(transactionDate, subject, description, amount, 
                        transactionRef, "Withdrawal", -1);


        private static SoaRowDTO GetSoaRowDTO(DateTime transactionDate, 
            string subject, string description, decimal amount,
            string transactionRef, string txnType, decimal multiplier)
        {
            if (amount <= 0) throw Fault.BadArg($"{txnType} Amount", amount);
            return new SoaRowDTO
            {
                DateOffset     = transactionDate.SoaRowOffset(),
                Subject        = subject,
                Description    = description,
                Amount         = amount * multiplier,
                TransactionRef = transactionRef
            };
        }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
