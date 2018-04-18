using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using PassbookTally.DomainLib.DTOs;

namespace PassbookTally.DatabaseLib.Repositories
{
    public class ActiveFundReqsRepo : NamedCollectionBase<FundRequestDTO>
    {
        public ActiveFundReqsRepo(SharedLiteDB sharedLiteDB, string collectionName = "FundReqs_Active") : base(collectionName, sharedLiteDB)
        {
        }


        public int GetMaxSerial()
        {
            using (var db = _db.OpenRead())
            {
                var coll = GetCollection(db);
                if (coll.Count() == 0) return 0;
                return coll.Max(_ => _.SerialNum);
            }
        }


        public override void Validate(FundRequestDTO model, SharedLiteDB db)
        {
        }


        internal bool HasRequestSerial(int serialNum)
        {
            using (var db = _db.OpenRead())
            {
                var coll = GetCollection(db);
                if (coll.Count() == 0) return false;
                return coll.Exists(_ => _.SerialNum == serialNum);
            }
        }


        protected override void EnsureIndeces(LiteCollection<FundRequestDTO> coll)
        {
            coll.EnsureIndex(_ => _.SerialNum, false);
        }
    }
}
