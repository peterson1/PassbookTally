using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.LicenseTools;

namespace PassbookTally.DomainLib45.Configuration
{
    public class AppArguments
    {
        public string               DbFilePath   { get; internal set; }
        public bool                 IsValidUser  { get; private set; }
        public FirebaseCredentials  Credentials  { get; private set; }


        public void GetCredentials(string key)
        {
            IsValidUser = SeatLicenser.TryGetCredentials(key, 
                out FirebaseCredentials creds, out string err);

            Credentials = creds;
        }
    }
}
