using CommonTools.Lib45.ThreadTools;
using Mono.Options;
using System;

namespace PassbookTally.DomainLib45.Configuration
{
    public class CommandlineOptions
    {
        public static AppArguments Parse()
        {
            var args    = new AppArguments();
            var options = new OptionSet
            {
                {   "db|database="  , "Database file path", db   => args.DbFilePath = db    },
                {  "key|publickey=" , "Public key"        , key  => args.SetCredentials(key)}
            };
            try
            {
                options.Parse(Environment.GetCommandLineArgs());
                args.InitializeDatabases();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }
            return args;
        }
    }
}
