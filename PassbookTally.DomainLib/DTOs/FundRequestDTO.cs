﻿using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ReflectionTools;
using System;
using System.Collections.Generic;

namespace PassbookTally.DomainLib.DTOs
{
    public class FundRequestDTO : IDocumentDTO, ICloneable
    {
        public int        Id           { get; set; }
        public string     Author       { get; set; }
        public DateTime   Timestamp    { get; set; }
        public string     Remarks      { get; set; }
                          
        public int        SerialNum      { get; set; }
        public int        BankAccountId  { get; set; }
        public string     Payee          { get; set; }
        public string     Purpose        { get; set; }
        public DateTime   RequestDate    { get; set; }
        public decimal?   Amount         { get; set; }

        public List<AccountAllocation>  Allocations  { get; set; }


        public class AccountAllocation
        {
            public GLAccountDTO  Account    { get; set; }
            public decimal       SubAmount  { get; set; }
        }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
