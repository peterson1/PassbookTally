using FluentAssertions;
using FluentAssertions.Extensions;
using PassbookTally.DatabaseLib;
using PassbookTally.DomainLib.DTOs;
using PassbookTally.DomainLib.ReportRows;
using Xunit;

namespace PassbookTally.TestsLib.DatabaseLibTests
{
    [Trait("Solitary", "Passbook DB")]
    public class PassbookDbFacts
    {
        [Fact(DisplayName = "Unknown Path 1")]
        public void UnknownPath1()
        {
            var acctId = 1;
            var sut    = new PassbookDB(acctId);
            sut.SetBaseBalance(31.March(2018), 899_327.02M);

            sut.Insert(SoaRowDTO.Deposit
                (31.March(2018), "Interest", "", 335.66M));

            var rows = sut.RowsStartingFrom(31.March(2018));
            rows.Should().HaveCount(1);
            rows.LastBalance().Should().Be(899_662.68M);

            sut.Insert(SoaRowDTO.Withdrawal
                (31.March(2018), "W/ Tax", "", 67.13M));
            
            rows = sut.RowsStartingFrom(31.March(2018));
            rows.Should().HaveCount(2);
            rows.LastBalance().Should().Be(899_595.55M);
        }
    }
}
