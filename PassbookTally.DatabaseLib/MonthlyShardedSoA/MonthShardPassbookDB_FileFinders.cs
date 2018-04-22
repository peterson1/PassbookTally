using PassbookTally.DatabaseLib.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PassbookTally.DatabaseLib.MonthlyShardedSoA
{
    internal static class MonthShardPassbookDB_FileFinders
    {
        private const string SHARDS_DIR = "Transactions";
        private const string DB_SUFFIX  = "_Txns_Shard.ldb";


        internal static Dictionary<DateTime, SoaRowsRepo1> CreateFileBasedIndex(this MonthShardPassbookDB monthDb)
            => GetMatchingFiles(monthDb)
                .Select      (_ => CreateMonthlyRepo(_, monthDb))
                .ToDictionary(_ => _.FirstDay, 
                              _ => _.Repo);


        private static (DateTime FirstDay, SoaRowsRepo1 Repo) CreateMonthlyRepo(string fileName, MonthShardPassbookDB monthDb)
        {
            var day1 = ToFirstDay(fileName);
            var repo = monthDb.CreateFileBasedShard(day1);
            return (day1, repo);
        }


        private static DateTime ToFirstDay(string fileName)
        {
            var yrStr = fileName.Substring(0, 4);
            var moStr = fileName.Substring(5, 2);
            return new DateTime(int.Parse(yrStr), int.Parse(moStr), 1);
        }


        private static IEnumerable<string> GetMatchingFiles(MonthShardPassbookDB monthDb) 
            => Directory.EnumerateFiles(monthDb.GetShardsDir(), "*" + DB_SUFFIX);


        internal static string GetShardsDir(this MonthShardPassbookDB monthDb)
        {
            var dir = Path.Combine(Path.GetDirectoryName(monthDb.DbPath), SHARDS_DIR);
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static string GetFilename(DateTime date)
            => date.ToString("yyyy-MM") + DB_SUFFIX;


        internal static SoaRowsRepo1 CreateFileBasedShard(this MonthShardPassbookDB monthDb, DateTime day1)
        {
            var moRepo = monthDb.GetSoaRepo();
            var dbPath = Path.Combine(monthDb.GetShardsDir(), GetFilename(day1));
            var pbkDB  = new PassbookDB(monthDb.BankAccountId, dbPath, monthDb.CurrentUser);
            return new SoaRowsRepo1(moRepo.BaseBalance, moRepo.BaseDate, pbkDB);
        }
    }
}
