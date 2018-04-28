using CommonTools.Lib45.UIExtensions;
using PassbookTally.DomainLib.DTOs;
using System.Windows.Controls;

namespace PassbookTally.CrudApp.PreparedCheques
{
    public partial class PreparedChequesTable : UserControl
    {
        public PreparedChequesTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.EnableOpenCurrent<RequestedChequeDTO>();
                dg.ScrollToEndOnChange();
            };
        }
    }
}
