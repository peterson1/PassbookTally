using PassbookTally.DomainLib45.Configuration;
using System.Windows;

namespace PassbookTally.CrudApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize(arg
                => new MainWindowVM(arg).Show<MainWindow>());
        }
    }
}
