using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.Exceptions;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Collections.Generic;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class SoaRowsRepo1 : NamedCollectionBase<SoaRowDTO>
    {
        private const string DATE_FMT = "yyyy-MM-dd";

        public SoaRowsRepo1(int bankAcctId, string collectionName, SharedLiteDB sharedLiteDB) : base(collectionName, sharedLiteDB)
        {
            BankAccountId = bankAcctId;

            _db.Metadata.AddIfNone(GetBaseBalanceKey(), "0");
            _db.Metadata.AddIfNone(GetBaseDateKey(), "2018-01-01");

            SetBaseValues();
        }


        public int       BankAccountId  { get; }
        public decimal   BaseBalance    { get; private set; }
        public DateTime  BaseDate       { get; private set; }


        private void SetBaseValues()
        {
            BaseBalance = decimal.Parse(_db.Metadata[GetBaseBalanceKey()]);
            BaseDate    = DateTime.Parse(_db.Metadata[GetBaseDateKey()]);
        }


        public override void Validate(SoaRowDTO model, SharedLiteDB db)
        {
            if (model.GetDate() < BaseDate)
                throw new PredatesBaseBalanceException(model, BaseDate);
        }


        public void SetBaseBalance(DateTime date, decimal openingBalance)
        {
            _db.Metadata[GetBaseDateKey   ()] = date.ToString(DATE_FMT);
            _db.Metadata[GetBaseBalanceKey()] = openingBalance.ToString();
            SetBaseValues();
        }


        public void Deposit(DateTime transactionDate, string subject, decimal amount, string description = null)
        {
            var dto = SoaRowDTO.Deposit(transactionDate, subject, description, amount);
            dto.RunningBalance = GetRunningBalance(dto);

        }


        private decimal GetRunningBalance(SoaRowDTO dto)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<SoaRowDTO> RowsStartingFrom(DateTime date)
            => Find(_ => _.DateOffset >= date.SoaRowOffset());


        private string GetBaseDateKey   () => $"Acct{BankAccountId}_BaseDate";
        private string GetBaseBalanceKey() => $"Acct{BankAccountId}_BaseBalance";


        protected override void EnsureIndeces(LiteCollection<SoaRowDTO> coll)
        {
            coll.EnsureIndex(_ => _.DateOffset, false);
        }
    }
}
