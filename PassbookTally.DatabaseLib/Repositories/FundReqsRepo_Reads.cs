using System;
using System.Collections.Generic;
using System.Linq;

namespace PassbookTally.DatabaseLib.Repositories
{
    public static class FundReqsRepo_Reads
    {
        public static DateTime NextRequestDate(this PassbookDB db)
        {
            var repo = db.ActiveRequests;
            if (!repo.Any()) return DateTime.Now;
            return repo.GetAll().Max(_ => _.RequestDate);
        }


        public static int NextRequestSerial(this PassbookDB db)
        {
            var activesMax  = db.ActiveRequests.GetMaxSerial();
            var inactivsMax = db.InactiveRequests.GetMaxSerial();
            return Math.Max(activesMax, inactivsMax) + 1;
        }


        public static bool HasRequestSerial(this PassbookDB db, int serialNum)
            => db.ActiveRequests  .HasRequestSerial(serialNum)
            || db.InactiveRequests.HasRequestSerial(serialNum);


        public static IEnumerable<string> GetPayees(this PassbookDB db)
            => db.ActiveRequests.GetPayees()
                 .Concat(db.InactiveRequests.GetPayees())
                 .GroupBy(_ => _)
                 .Select(g => g.First());
    }
}
