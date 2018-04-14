using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.Exceptions;
using PassbookTally.DomainLib.ReportRows;
using System;

namespace PassbookTally.DatabaseLib.Repositories
{
    public partial class SoaRowsRepo1 : NamedCollectionBase<SoaRowDTO>
    {
        private const string DATE_FMT = "yyyy-MM-dd";

        public SoaRowsRepo1(int bankAcctId, string collectionName, PassbookDB db) : base(collectionName, db)
        {
            BankAccountId = bankAcctId;

            _db.Metadata.AddIfNone(db.GetAccountNameKey(BankAccountId), $"Bank Account {BankAccountId}");
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


        private SoaRowDTO GetBaseBalanceDTO() => new SoaRowDTO
        {
            Subject        = "Base Balance",
            DateOffset     = BaseDate.SoaRowOffset(),
            RunningBalance = BaseBalance,
        };



        private string GetBaseDateKey   () => $"Acct{BankAccountId}_BaseDate";
        private string GetBaseBalanceKey() => $"Acct{BankAccountId}_BaseBalance";


        protected override void EnsureIndeces(LiteCollection<SoaRowDTO> coll)
        {
            coll.EnsureIndex(_ => _.DateOffset, false);
        }
    }
}
