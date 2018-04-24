using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DomainLib.DTOs;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class ActiveChequesRepo : NamedCollectionBase<RequestedChequeDTO>
    {
        public ActiveChequesRepo(SharedLiteDB sharedLiteDB, string collectionName = "RequestedCheques_Active") : base(collectionName, sharedLiteDB)
        {
        }

        public override void Validate(RequestedChequeDTO model, SharedLiteDB db)
        {
        }
    }
}
