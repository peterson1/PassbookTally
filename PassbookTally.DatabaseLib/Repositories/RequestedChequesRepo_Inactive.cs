using CommonTools.Lib45.LiteDbTools;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class InactiveChequesRepo : ActiveChequesRepo
    {
        public InactiveChequesRepo(SharedLiteDB sharedLiteDB, string collectionName = "RequestedCheques_Inactive") : base(sharedLiteDB, collectionName)
        {
        }
    }
}
