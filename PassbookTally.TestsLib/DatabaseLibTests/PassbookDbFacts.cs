using FluentAssertions;
using PassbookTally.DatabaseLib;
using System;
using System.IO;
using Xunit;

namespace PassbookTally.TestsLib.DatabaseLibTests
{
    [Trait("SoaRowsRepo1", "Solitary")]
    public class PassbookDbFacts
    {
        [Fact(DisplayName = "Creates default bank acct if no accounts yet")]
        public void Createsdefaultbankacctifnoaccountsyet()
        {
            var db   = new PassbookDB(1, new MemoryStream(), "");
            db.AccountNames.Should().HaveCount(1);
            var meta = db.Metadata;
            meta.HasName("Acct_1").Should().BeTrue();
            var repo = db.GetSoaRepo();
            meta.HasName("Acct1_BaseDate").Should().BeTrue();
            meta.HasName("Acct1_BaseBalance").Should().BeTrue();
        }


        [Fact(DisplayName = "Creates meta for new accounts")]
        public void Createsmetafornewaccounts()
        {
            var db   = new PassbookDB(2, new MemoryStream(), "");
            var meta = db.Metadata;
            db.GetSoaRepo().Deposit(DateTime.Now, "", "", 1, "");
            meta.HasName("Acct_2").Should().BeTrue();
            meta.HasName("Acct2_BaseDate").Should().BeTrue();
            meta.HasName("Acct2_BaseBalance").Should().BeTrue();
        }
    }
}
