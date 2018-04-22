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
