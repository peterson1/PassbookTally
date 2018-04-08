using FluentAssertions;
using FluentAssertions.Extensions;
using PassbookTally.DatabaseLib;
using PassbookTally.DatabaseLib.Repositories;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.Exceptions;
using PassbookTally.DomainLib.ReportRows;
using System;
using System.IO;
using Xunit;

namespace PassbookTally.TestsLib.DatabaseLibTests
{
    [Trait("Solitary", "Passbook DB")]
    public class PassbookDbFacts
    {
        [Fact(DisplayName = "Rejects predated txn")]
        public void Rejectspredatedtxn()
        {
            var sut = CreateSUT(out int acctId);

            Action act = () => sut.Insert(SoaRowDTO.Deposit
                (30.March(2018), "before base balance", "", 123.45M));

            act.Should().Throw<PredatesBaseBalanceException>();
            

            

            //sut.Insert(SoaRowDTO.Withdrawal
            //    (31.March(2018), "W/ Tax", "", 67.13M));

            //rows = sut.RowsStartingFrom(31.March(2018));
            //rows.Should().HaveCount(2);
            //rows.LastBalance().Should().Be(899_595.55M);
        }

        [Fact(DisplayName = "Deposit increases balance")]
        public void Depositincreasesbalance()
        {
            var sut = CreateSUT(out int acctId);

            //sut.Insert(SoaRowDTO.Deposit
            //    (31.March(2018), "Interest", "", 335.66M));
            sut.Deposit(31.March(2018), "Interest", 335.66M);

            var rows = sut.RowsStartingFrom(31.March(2018));
            rows.Should().HaveCount(1);
            rows.LastBalance().Should().Be(899_662.68M);
        }



        private SoaRowsRepo1 CreateSUT(out int bankAcctId)
        {
            var db  = new PassbookDB(new MemoryStream(), "");
            var sut = db.ForAccount(bankAcctId = 1);
            sut.SetBaseBalance(31.March(2018), 899_327.02M);
            return sut;
        }
    }
}
