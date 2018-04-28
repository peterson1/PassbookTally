using CommonTools.Lib45.UIExtensions;
using PassbookTally.DomainLib.DTOs;
using System.Windows.Controls;

namespace PassbookTally.CrudApp.FundRequests
{
    public partial class FundReqsTable : UserControl
    {
        public FundReqsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<FundRequestDTO>(
                    _ => $"Are you sure you want to delete the entry for “{_.Purpose}”?");

                dg.EnableOpenCurrent<FundRequestDTO>();
                //dg.ScrollToEndOnChange();
            };
        }
    }
}
