using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.ThreadTools;
using System;
//using Serilog;
using System.Threading.Tasks;
using System.Windows;

namespace PassbookTally.DomainLib45.Configuration
{
    public static class AppInitializer
    {
        public static void Initialize(this Application app, Action<AppArguments> onStartup)
        {
            HandleGlobalErrors();

            ThisThread.SetShortDateFormat("d MMM yyyy");

            var args = CommandlineOptions.Parse();
            //Log.Logger = LogConfigs.ToLoggly(args);

            app.Exit += (s, e) => RunOnExit(args);

            //Log.Information("{User} launched {Exe}", args.UserName, args.ExeName);
            SafeExecute(onStartup, args);
        }


        private static void SafeExecute(Action<AppArguments> action, AppArguments args)
        {
            try
            {
                action.Invoke(args);
            }
            catch (Exception ex)
            {
                //Log.Error(ex.Info(true, true));
                Alert.Show(ex, "Initialize -> SafeExecute");
                CurrentExe.Shutdown();
            }
        }


        private static void RunOnExit(AppArguments args)
        {
            //try
            //{
            //    Log.Information("{User} closed {Exe}", args.UserName, args.ExeName);
            //    Log.CloseAndFlush();
            //}
            //catch { }
        }


        private static void HandleGlobalErrors()
        {
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                OnError(e.Exception, "Application.Current");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                OnError(e.Exception, "TaskScheduler");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                OnError(e.ExceptionObject as Exception, "AppDomain.CurrentDomain");
                // application terminates after above
            };
        }


        private static void OnError(Exception ex, string context)
        {
            Alert.Show(ex, context);
            //File.WriteAllText(@"C:\err.txt", ex?.ToString());
            //MessageBox.Show(ex?.ToString());
        }
    }
}
