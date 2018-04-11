﻿using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using CommonTools.Lib45.UIExtensions;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.BaseViewModels
{
    public abstract class MainWindowVMBase<TArg> : INotifyPropertyChanged
        where TArg : ICredentialsProvider
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        //public event EventHandler<object>        DocumentSaved   = delegate { };
        protected string RefreshingText = "Refreshing list of records...";
        protected abstract string CaptionPrefix { get; }

        private Window _win;


        public MainWindowVMBase(TArg appArguments)
        {
            AppArgs = appArguments;

            PropertyChanged += (s, e) 
                => HandleChangesByName(e.PropertyName);

            RefreshCmd     = R2Command.Async(DoRefresh, _ => !IsBusy, "Refresh");
            CloseWindowCmd = R2Command.Relay(CloseWindow, null, "Close Window");

            SetCaption($"as {AppArgs?.Credentials?.NameAndRole ?? "Anonymous"}");
        }


        public TArg        AppArgs         { get; }
        public string      Caption         { get; private set; }
        public bool        IsBusy          { get; private set; }
        public string      BusyText        { get; private set; }
        public IR2Command  RefreshCmd      { get; }
        public IR2Command  CloseWindowCmd  { get; }


        public void CloseWindow() => _win?.Close();


        protected void SetCaption(string message)
        {
            try   { Caption = $"   {CaptionPrefix}   (v{CurrentExe.GetShortVersion()})    {message}"; }
            catch { Caption = $"   {CaptionPrefix}   {message}"; }
        }


        public void StartBeingBusy(string message)
        {
            IsBusy   = true;
            BusyText = message;
        }
        public void StopBeingBusy() => IsBusy = false;


        protected void AsUI(Action action)
        {
            if (_win == null)
            {
                try   { action.Invoke(); }
                catch { }
            }
            else
                UIThread.Run(action);
        }


        private async Task DoRefresh()
        {
            StartBeingBusy(RefreshingText);
            OnRefreshClicked();

            //await OnRefreshClickedAsync();
            await Task.Run(() => OnRefreshClickedAsync());

            StopBeingBusy();
        }
        protected virtual void OnRefreshClicked     () { }
        protected virtual Task OnRefreshClickedAsync() => Task.Delay(0);
        public void ClickRefresh() => RefreshCmd.ExecuteIfItCan();


        protected virtual void HandleChangesByName(string propertyName)
        {
        }


        protected void SayPropertyChanged(string propertyName)
            => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public bool? Show<T>(bool hideWindow = false, bool showModal = false) 
            where T: Window, new()
        {
            _win = new T();
            ApplyWindowTheme(_win);
            _win.DataContext = this;
            _win.MakeDraggable();
            _win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _win.SnapsToDevicePixels = true;

            #if DEBUG
            _win.SetToCloseOnEscape();
            #endif

            if (showModal)
                return _win.ShowDialog();
            else
            {
                //_win.Show();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _win.ShowTemporarilyOnTop();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (hideWindow) _win.Hide();
                return null;
            }
        }


        protected virtual void ApplyWindowTheme(Window win)
        {
        }


        protected void ReturnDialogResult(bool? result)
        {
            if (_win == null) return;
            _win.DialogResult = result;
            CloseWindow();
        }


        //protected void RaiseDocumentSaved<T>(T document)
        //    => DocumentSaved.Invoke(this, document);


        public bool AllFieldsValid()
            => _win?.AllFieldsValid() ?? true;
    }
}
