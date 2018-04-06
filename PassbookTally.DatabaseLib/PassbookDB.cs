using System;
using System.Collections.Generic;
using System.Linq;
using PassbookTally.DatabaseLib.Persistors;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;

namespace PassbookTally.DatabaseLib
{
    public class PassbookDB
    {
        private int acctId;
        private SoaRowsRepo1 _repo;

        public PassbookDB(int acctId)
        {
            this.acctId = acctId;
            _repo = new SoaRowsRepo1();
        }


        public void SetBaseBalance(DateTime date, decimal balance)
        {
            _repo.BaseDate    = date;
            _repo.BaseBalance = balance;
        }


        public void Insert(SoaRowDTO soaRowDTO)
        {
            _repo.Add(soaRowDTO);
        }


        public IEnumerable<SoaRowVM> RowsStartingFrom(DateTime date)
        {
            var rows = _repo.OrderBy(_ => _.DateOffset)
                            .Select(_ => new SoaRowVM(_))
                            .ToList();

            for (int i = 0; i < rows.Count; i++)
            {
                var prevBal = i == 0 
                            ? _repo.BaseBalance
                            : rows[i - 1].RunningBalance;
                rows[i].RunningBalance
                    = prevBal + rows[i].DTO.Amount;
            }
            return rows;
        }
    }
}
