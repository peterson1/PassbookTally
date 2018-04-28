using CommonTools.Lib45.PrintTools;
using System.Windows;
using System.Windows.Controls;

namespace PassbookTally.CrudApp.TransactionLog
{
    public partial class TransactionLogUI : UserControl
    {
        public TransactionLogUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                tBar.btnPrint.Click += BtnPrint_Click;
            };
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var headr = $"Statement of Accounts for “{VM.AccountName}”";
            tblTxns.dg.AskToPrint(headr);
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
