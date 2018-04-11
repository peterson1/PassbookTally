using System.Windows;

namespace PassbookTally.DomainLib45.Configuration
{
    public static class MaterialDesignThemer
    {
        public static void ApplyMaterialTheme(this Window win)
        {
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //later: move xaml boilerplates to here
        }
    }
}
