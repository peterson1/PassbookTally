using CommonTools.Lib45.LiteDbTools;
using PassbookTally.DomainLib.DTOs;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class GLAccountsRepo : SharedCollectionBase<GLAccountDTO>
    {
        public GLAccountsRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }


        public override void Validate(GLAccountDTO model, SharedLiteDB db)
        {
        }
    }
}
