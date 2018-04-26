using System.Windows.Controls;
using System.Windows.Input;

namespace PassbookTally.CrudApp.FundRequests
{
    public partial class AllocationsTable : UserControl
    {
        public AllocationsTable()
        {
            InitializeComponent();
        }

        private void dg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete) return;
            if (dg.SelectedIndex == 0) return;
            VM.Items.Remove(dg.SelectedItem as AllocationVM);
        }

        private AllocationsListVM VM => DataContext as AllocationsListVM;
    }
}
