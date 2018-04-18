using CommonTools.Lib45.LiteDbTools;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class InactiveFundReqsRepo : ActiveFundReqsRepo
    {
        public InactiveFundReqsRepo(SharedLiteDB sharedLiteDB, string collectionName = "FundReqs_Inactive") : base(sharedLiteDB, collectionName)
        {
        }
    }
}
