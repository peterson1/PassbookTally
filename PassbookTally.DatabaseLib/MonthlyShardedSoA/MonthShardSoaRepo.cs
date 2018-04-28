using CommonTools.Lib11.DateTimeTools;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DatabaseLib.MonthlyShardedSoA
{
    public class MonthShardSoaRepo : SoaRowsRepo1
    {
        private MonthShardPassbookDB _moDB;


        public MonthShardSoaRepo(decimal baseBalance, DateTime baseDate, MonthShardPassbookDB db) : base(baseBalance, baseDate, db)
        {
            _moDB = db;
        }


        protected override void InsertOrUpdate(SoaRowDTO dto) 
            => _moDB.GetMonthRepo(dto.GetDate()).Upsert(dto);


        public override void BulkUpdate(IEnumerable<SoaRowDTO> rows)
        {
            var grpd = rows.GroupBy(_ => _.GetDate().MonthFirstDay());
            foreach (var grp in grpd)
            {
                var repo = _moDB.GetMonthRepo(grp.Key);
                repo.BulkUpdate(grp);
            }
        }


        protected override void Remove(SoaRowDTO dto)
            => _moDB.GetMonthRepo(dto.GetDate()).Delete(dto);


        public override List<SoaRowDTO> GetUpTo(DateTime maxDate) 
            => _moDB.GetMonthsUpTo(maxDate)
                    .Select       (_ => _.GetUpTo(maxDate))
                    .SelectMany   (_ => _).ToList();


        public override List<SoaRowDTO> GetFrom(DateTime minDate)
            => _moDB.GetMonthsFrom(minDate)
                    .Select       (_ => _.GetFrom(minDate))
                    .SelectMany   (_ => _).ToList();
        //{
        //    var repos = _moDB.GetMonthsFrom(minDate);
        //    var lists = repos.Select(_ => _.GetFrom(minDate));
        //    return lists.SelectMany(_ => _).ToList();
        //}
    }
}
