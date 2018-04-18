using CommonTools.Lib45.UIExtensions;
using PassbookTally.DomainLib.ReportRows;
using System.Windows.Controls;

namespace PassbookTally.CrudApp.TransactionLog
{
    public partial class TransactionsTable : UserControl
    {
        public TransactionsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<SoaRowVM>(
                    _ => $"Are you sure you want to delete the entry for “{_.Description}”?");

                dg.EnableOpenCurrent<SoaRowVM>();
                dg.ScrollToEndOnChange();
            };
        }
    }
}
