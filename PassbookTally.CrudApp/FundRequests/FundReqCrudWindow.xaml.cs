using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PassbookTally.CrudApp.FundRequests
{
    /// <summary>
    /// Interaction logic for FundReqCrudWindow.xaml
    /// </summary>
    public partial class FundReqCrudWindow : Window
    {
        public FundReqCrudWindow()
        {
            InitializeComponent();
            //Loaded += (a, b) =>
            //{
            //    System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            //};
        }
    }
}
