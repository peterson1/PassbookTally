using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PassbookTally.DatabaseLib
{
    public class PassbookDB : SharedLiteDB
    {
        private Dictionary<string, SoaRowsRepo1> _soaReposDict = new Dictionary<string, SoaRowsRepo1>();


        public PassbookDB(string dbFilePath, string currentUser) : base(dbFilePath, currentUser)
        {
        }

        public PassbookDB(MemoryStream memoryStream, string currentUser) : base(memoryStream, currentUser)
        {
        }


        public SoaRowsRepo1 ForAccount(int bankAcctId)
        {
            var key = $"Account{bankAcctId}_SoaRows";

            if (_soaReposDict.TryGetValue(key, out SoaRowsRepo1 repo))
                return repo;

            repo = new SoaRowsRepo1(bankAcctId, key, this);
            _soaReposDict.Add(key, repo);
            return repo;
        }

        //public PassbookDB(int acctId)
        //{
        //    this.acctId = acctId;
        //    _repo = new SoaRowsRepo1();
        //}


        //public void SetBaseBalance(DateTime date, decimal balance)
        //{
        //    _repo.BaseDate    = date;
        //    _repo.BaseBalance = balance;
        //}


        //public void Insert(SoaRowDTO soaRowDTO)
        //{
        //    _repo.Add(soaRowDTO);
        //}


        //public IEnumerable<SoaRowVM> RowsStartingFrom(DateTime date)
        //{
        //    var rows = _repo.OrderBy(_ => _.DateOffset)
        //                    .Select(_ => new SoaRowVM(_))
        //                    .ToList();

        //    for (int i = 0; i < rows.Count; i++)
        //    {
        //        var prevBal = i == 0 
        //                    ? _repo.BaseBalance
        //                    : rows[i - 1].RunningBalance;
        //        rows[i].RunningBalance
        //            = prevBal + rows[i].DTO.Amount;
        //    }
        //    return rows;
        //}

        protected override void InitializeCollections()
        {
        }
    }
}
