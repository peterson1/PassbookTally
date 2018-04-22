using System;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.LicenseTools;
using PassbookTally.DatabaseLib;
using PassbookTally.DatabaseLib.MonthlyShardedSoA;

namespace PassbookTally.DomainLib45.Configuration
{
    public class AppArguments : ICredentialsProvider
    {
        public PassbookDB           PassbookDB   { get; private set; }
        public string               DbFilePath   { get; internal set; }
        public bool                 IsValidUser  { get; private set; }
        public FirebaseCredentials  Credentials  { get; private set; }


        public void SetCredentials(string key)
        {
            IsValidUser = SeatLicenser.TryGetCredentials(key, 
                out FirebaseCredentials creds, out string err);

            Credentials = creds;
        }


        internal void InitializeDatabases(int bankAcctId = 1)
        {
            PassbookDB = GetPassbookDB(bankAcctId);
        }


        public PassbookDB GetPassbookDB(int bankAcctId)
        {
            var usr = IsValidUser ? Credentials.HumanName : "Anonymous";
            return new MonthShardPassbookDB(bankAcctId, DbFilePath, usr);
        }
    }
}
