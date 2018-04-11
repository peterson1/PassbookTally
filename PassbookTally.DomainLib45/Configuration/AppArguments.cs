using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.LicenseTools;
using PassbookTally.DatabaseLib;
using System;

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


        internal void InitializeDatabases()
        {
            var usr = IsValidUser ? Credentials.HumanName : "Anonymous";
            PassbookDB = new PassbookDB(DbFilePath, usr);
        }
    }
}
