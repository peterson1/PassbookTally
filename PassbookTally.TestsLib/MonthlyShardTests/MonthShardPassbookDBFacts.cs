using FluentAssertions;
using FluentAssertions.Extensions;
using PassbookTally.DatabaseLib;
using PassbookTally.DatabaseLib.MonthlyShardedSoA;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.Exceptions;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace PassbookTally.TestsLib.MonthlyShardTests
{
    [Trait("Monthly Shard", "Solitary")]
    public class MonthShardPassbookDBFacts
    {
        [Fact(DisplayName = "Rejects predated txn")]
        public void Rejectspredatedtxn()
        {
            var sut = CreateSUT(31.March(2018), 899_327.02M);

            sut.ClosingBalanceFor(31.March(2018)).Should().Be(899_327.02M);

            Action act = () => sut.Insert(SoaRowDTO.Deposit
                (30.March(2018), "before base balance", "", 123.45M, ""));

            act.Should().Throw<PredatesBaseBalanceException>();
        }


        [Fact(DisplayName = "Deposit increases balance")]
        public void Depositincreasesbalance()
        {
            var sut = CreateSUT(31.March(2018), 899_327.02M);

            sut.Deposit(31.March(2018), "", "Interest", 335.66M, "");

            sut.ClosingBalanceFor(31.March(2018)).Should().Be(899_662.68M);

            var rows = sut.GetFrom(31.March(2018));
            rows.Should().HaveCount(1);
            rows.Last().RunningBalance.Should().Be(899_662.68M);
        }


        [Fact(DisplayName = "Withdrawal decreases balance")]
        public void Withdrawaldecreasesbalance()
        {
            var sut = CreateSUT(31.March(2018), 899_327.02M);

            sut.Deposit (31.March(2018), "", "Interest", 335.66M, "");
            sut.Withdraw(31.March(2018), "", "W/Tax"   , 67.13M , "");

            sut.ClosingBalanceFor(31.March(2018)).Should().Be(899_595.55M);
            sut.ClosingBalanceFor(10.April(2018)).Should().Be(899_595.55M);

            var rows = sut.GetFrom(31.March(2018));
            rows.Should().HaveCount(2);
            rows.Last().RunningBalance.Should().Be(899_595.55M);
        }


        [Fact(DisplayName = "Commutative 2-3-3-4")]
        public void Commutative2334()
        {
            var sut = CreateSUT(2.April(2018), 1_693_240.03M);

            sut.Withdraw(2.April(2018), "", "SixM Funds Transfer"  , 500_000    , "");
            sut.Deposit(3.April(2018) , "", "RDM Foods rent"       , 13_079     , "");
            sut.Withdraw(3.April(2018), "", "Employees loan"       , 23_132     , "");
            sut.Deposit(4.April(2018) , "", "Late Collection Apr.3", 118_226.25M, "");

            var rows = sut.GetFrom(31.March(2018));
            rows.Should().HaveCount(4);
            rows.Last().RunningBalance.Should().Be(1_301_413.28M);
        }


        [Fact(DisplayName = "Commutative 2-4-3-3")]
        public void Commutative2433()
        {
            var sut = CreateSUT(2.April(2018), 1_693_240.03M);

            sut.Withdraw(2.April(2018), "", "SixM Funds Transfer", 500_000      , "");
            sut.Deposit (4.April(2018), "", "Late Collection Apr.3", 118_226.25M, "");
            sut.Deposit (3.April(2018), "", "RDM Foods rent", 13_079            , "");
            sut.Withdraw(3.April(2018), "", "Employees loan", 23_132            , "");

            var rows = sut.GetFrom(31.March(2018));
            rows.Should().HaveCount(4);
            rows.Last().RunningBalance.Should().Be(1_301_413.28M);
        }


        [Fact(DisplayName = "Commutative 4-3-2-3")]
        public void Commutative4323()
        {
            var sut = CreateSUT(2.April(2018), 1_693_240.03M);

            sut.Deposit (4.April(2018), "", "Late Collection Apr.3", 118_226.25M, "");
            sut.Withdraw(3.April(2018), "", "Employees loan"       , 23_132     , "");
            sut.Withdraw(2.April(2018), "", "SixM Funds Transfer"  , 500_000    , "");
            sut.Deposit (3.April(2018), "", "RDM Foods rent"       , 13_079     , "");

            var rows = sut.GetFrom(31.March(2018));
            rows.Should().HaveCount(4);
            rows.Last().RunningBalance.Should().Be(1_301_413.28M);
        }


        [Fact(DisplayName = "delete sole txn")]
        public void deletetxn()
        {
            var sut = CreateSUT(31.March(2018), 1000);
            sut.Deposit(1.April(2018), "", "Interest", 500, "");
            var rows = sut.GetFrom(31.March(2018));

            sut.DeleteAndUpdateBalances(rows[0]);

            sut.ClosingBalanceFor(1.April(2018)).Should().Be(1000);
        }


        [Fact(DisplayName = "delete 2nd txn")]
        public void delete2ndtxn()
        {
            var sut = CreateSUT(31.March(2018), 1000);
            sut.Deposit(1.April(2018), "", "", 500, "");
            sut.Deposit(2.April(2018), "", "", 200, "");
            var rows = sut.GetFrom(31.March(2018));

            sut.DeleteAndUpdateBalances(rows[1]);

            sut.ClosingBalanceFor(3.April(2018)).Should().Be(1500);
        }



        private SoaRowsRepo1 CreateSUT(DateTime baseDate, decimal baseBalance)
        {
            //var db  = new MonthShardPassbookDB(1, new MemoryStream(), "");
            var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, "Monthlys.ldb");
            var db = new MonthShardPassbookDB(1, file, "");
            return db.GetSoaRepo(baseBalance, baseDate);
        }
    }
}
