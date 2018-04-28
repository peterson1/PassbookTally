using CommonTools.Lib45.UIExtensions;
using PassbookTally.DomainLib.DTOs;
using System.Windows.Controls;

namespace PassbookTally.CrudApp.IssuedCheques
{
    public partial class IssuedChequesTable : UserControl
    {
        public IssuedChequesTable()
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
