namespace CommonTools.Lib45.InputDialogs
{
    internal class PopUpInputInt : PopUpInput<int, PopUpInputIntWindow>
    {
        public PopUpInputInt(string caption, string message, int? defaultVal) : base(caption, message)
        {
            Draft = defaultVal;
        }


        public int? Draft { get; set; }


        public override bool TryParseValue(out int parsed)
        {
            if (Draft.HasValue)
            {
                parsed = Draft.Value;
                return true;
            }
            else
            {
                parsed = 0;
                return false;
            }
        }
    }
}
