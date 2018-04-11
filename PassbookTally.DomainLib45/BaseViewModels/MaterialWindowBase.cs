using CommonTools.Lib45.BaseViewModels;
using PassbookTally.DomainLib45.Configuration;
using System.Windows;

namespace PassbookTally.DomainLib45.BaseViewModels
{
    public abstract class MaterialWindowBase : MainWindowVMBase<AppArguments>
    {
        protected override string CaptionPrefix => "DCDR";


        public MaterialWindowBase(AppArguments appArguments) : base(appArguments)
        {
        }


        protected override void ApplyWindowTheme(Window win) 
            => win.ApplyMaterialTheme();
    }
}
