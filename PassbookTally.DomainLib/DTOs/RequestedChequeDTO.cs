using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace PassbookTally.DomainLib.DTOs
{
    public class RequestedChequeDTO : IDocumentDTO, ICloneable
    {
        public int       Id         { get; set; }
        public string    Author     { get; set; }
        public DateTime  Timestamp  { get; set; }
        public string    Remarks    { get; set; }


        public FundRequestDTO  Request       { get; set; }
        public int             BankAccountId { get; set; }
        public DateTime        ChequeDate    { get; set; }
        public int             ChequeNumber  { get; set; }
        public string          IssuedTo      { get; set; }
        public DateTime?       IssuedDate    { get; set; }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
