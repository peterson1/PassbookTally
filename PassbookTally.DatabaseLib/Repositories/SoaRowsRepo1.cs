using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.Exceptions;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Linq;

namespace PassbookTally.DatabaseLib.Repositories
{
    public partial class SoaRowsRepo1 : NamedCollectionBase<SoaRowDTO>
    {
        private const string DATE_FMT = "yyyy-MM-dd";

        public SoaRowsRepo1(decimal baseBalance, DateTime baseDate, PassbookDB db) : base(db.RepoKey, db)
        {
            BaseBalance = baseBalance;
            BaseDate    = baseDate;
        }


        public decimal   BaseBalance  { get; }
        public DateTime  BaseDate     { get; }



        public decimal ClosingBalanceFor(DateTime date)
        {
            if (date < BaseDate) return 0;
            var rows = GetUpTo(date);
            if (!rows.Any()) return BaseBalance;
            if (rows.Count == 1) return rows[0].RunningBalance;
            return rows.Last().RunningBalance;
        }


        public void Deposit(DateTime transactionDate, string subject, string description, decimal amount, string transactionRef)
            => UpsertAndUpdateBalances(SoaRowDTO
                .Deposit(transactionDate, subject, description, amount, transactionRef));


        public void Withdraw(DateTime transactionDate, string subject, string description, decimal amount, string transactionRef)
            => UpsertAndUpdateBalances(SoaRowDTO
                .Withdrawal(transactionDate, subject, description, amount, transactionRef));


        public virtual void UpsertAndUpdateBalances(SoaRowDTO dto)
        {
            Upsert(dto);
            var rows = GetFrom(BaseDate);
            rows[0].RunningBalance = BaseBalance + rows[0].Amount;

            for (int i = 1; i < rows.Count; i++)
                rows[i].RunningBalance
                    = rows[i - 1].RunningBalance + rows[i].Amount;

            Update(rows);
        }



        public override void Validate(SoaRowDTO model, SharedLiteDB db)
        {
            if (model.GetDate() < BaseDate)
                throw new PredatesBaseBalanceException(model, BaseDate);
        }


        protected override void EnsureIndeces(LiteCollection<SoaRowDTO> coll)
        {
            coll.EnsureIndex(_ => _.DateOffset, false);
        }
    }
}
