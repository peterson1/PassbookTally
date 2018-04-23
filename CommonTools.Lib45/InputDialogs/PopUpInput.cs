using PropertyChanged;
using System.Windows;

namespace CommonTools.Lib45.InputDialogs
{
    [AddINotifyPropertyChangedInterface]
    public abstract class PopUpInput<TOut, TWindow>
        where TWindow : Window, new()
    {
        internal PopUpInput(string caption, string message)
        {
            Caption = $"   {caption}";
            Message = message;
            Window  = new TWindow();
        }

        public Window   Window    { get; }
        public string   Caption   { get; }
        public string   Message   { get; }
        //public TOut     Input     { get; set; }


        public abstract bool TryParseValue(out TOut parsed);


        internal bool TryGetValidInput(out TOut valid)
        {
            Window.DataContext = this;
            var res = Window.ShowDialog();
            if (res != true)
            {
                valid = default(TOut);
                return false;
            }
            return TryParseValue(out valid);
        }
    }


    public static class PopUpInput
    {
        public static bool TryGetInt(string message, out int input, int? defaultVal = null, string caption = "Please enter a number")
        {
            var vm = new PopUpInputInt(caption, message, defaultVal);
            return vm.TryGetValidInput(out input);
        }
    }
}
