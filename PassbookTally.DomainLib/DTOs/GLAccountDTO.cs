using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace PassbookTally.DomainLib.DTOs
{
    public enum GLAcctType
    {
        Asset,
        Liability,
        Equity,
        Income,
        Expense
    }

    public class GLAccountDTO : IDocumentDTO, ICloneable
    {
        public int       Id         { get; set; }
        public string    Author     { get; set; }
        public DateTime  Timestamp  { get; set; }
        public string    Remarks    { get; set; }


        public GLAcctType  AccountType  { get; set; }
        public string      Name         { get; set; }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
