using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ExceptionTools;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract partial class SharedCollectionBase<T>
        where T : IDocumentDTO
    {
        public T Find(int recordId, bool errorIfMissing = true)
        {
            using (var db = _db.OpenRead())
            {
                var rec = GetCollection(db).FindById(recordId);

                if (rec == null && errorIfMissing)
                    throw RecordNotFoundException.For<T>("Id", recordId);

                return rec;
            }
        }


        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            using (var db = _db.OpenRead())
                return GetCollection(db).Find(predicate);
        }


        protected IEnumerable<T> Find(Query query)
        {
            using (var db = _db.OpenRead())
                return GetCollection(db).Find(query);
        }


        public IEnumerable<T> GetAll()
        {
            using (var db = _db.OpenRead())
                return GetCollection(db).FindAll();
        }


        public Dictionary<int, T> ToDictionary()
            => GetAll().ToDictionary(_ => _.Id);



        public bool Any() => CountAll() != 0;


        public T Latest()
        {
            using (var db = _db.OpenRead())
            {
                var colxn = GetCollection(db);
                var maxId = colxn.Max();
                return colxn.FindById(maxId);
            }
        }


        public int CountAll()
        {
            using (var db = _db.OpenRead())
                return GetCollection(db).Count();
        }


        private T Single(string field, BsonValue value, bool errorIfMissing)
        {
            using (var db = _db.OpenRead())
            {
                var matches = GetCollection(db).Find(Query.EQ(field, value));

                if (!matches.Any())
                {
                    if (errorIfMissing)
                        throw RecordNotFoundException.For<T>(field, value);
                    else
                        return default(T);
                }

                if (matches.Count() > 1)
                    throw DuplicateRecordsException.For(matches, field, value);

                return matches.Single();
            }
        }


        public T ByName(string recordName, bool errorIfMissing = true, string field = "Name")
            => Single(field, recordName, errorIfMissing);


        public bool HasName(string recordName, string field = "Name") 
            => ByName(recordName, false, field) != null;


        public bool TryGetName(string recordName, out T record, string field = "Name")
        {
            record = Single(field, recordName, false);
            return record != null;
        }


        public bool AnotherHasName(int recId, string recordName, string field = "Name")
        {
            var matches = Find(Query.EQ(field, recordName));
            if (!matches.Any()) return false;
            if (matches.Count() == 1 && matches.Single().Id == recId) return false;
            return true;
        }
    }
}
