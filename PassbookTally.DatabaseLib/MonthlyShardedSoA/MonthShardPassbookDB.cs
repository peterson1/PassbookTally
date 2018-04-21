using PassbookTally.DatabaseLib.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PassbookTally.DatabaseLib.MonthlyShardedSoA
{
    public class MonthShardPassbookDB : PassbookDB
    {
        private Dictionary<DateTime, SoaRowsRepo1> _idx;


        public MonthShardPassbookDB(int bankAcctId, string dbFilePath, string currentUser) : base(bankAcctId, dbFilePath, currentUser)
        {
        }

        public MonthShardPassbookDB(int bankAcctId, MemoryStream memoryStream, string currentUser) : base(bankAcctId, memoryStream, currentUser)
        {
        }


        protected override void InitializeCollections()
        {
            base.InitializeCollections();
            _idx = IsInMemory ? this.CreateMemoryBasedIndex()
                              : this.CreateFileBasedIndex  ();
        }

        protected override SoaRowsRepo1 CreateRepoInstance(decimal baseBalance, DateTime baseDate)
            => new MonthShardSoaRepo(baseBalance, baseDate, this);


        internal IEnumerable<SoaRowsRepo1> GetMonthsUpTo(DateTime maxDate)
        {
            var nextMo = new DateTime(maxDate.Year, maxDate.Month + 1, 1);
            return _idx.Where(_ => _.Key < nextMo).Select(_ => _.Value);
        }


        internal IEnumerable<SoaRowsRepo1> GetMonthsFrom(DateTime minDate)
        {
            var minMonth = new DateTime(minDate.Year, minDate.Month, 1);
            return _idx.Where(_ => _.Key >= minMonth).Select(_ => _.Value);
        }
    }
}
