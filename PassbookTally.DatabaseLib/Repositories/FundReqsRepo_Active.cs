using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DomainLib.DTOs;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class ActiveFundReqsRepo : NamedCollectionBase<FundRequestDTO>
    {
        public ActiveFundReqsRepo(SharedLiteDB sharedLiteDB) : base("FundReqs_Active", sharedLiteDB)
        {
        }


        public override void Validate(FundRequestDTO model, SharedLiteDB db) { }
    }
}
