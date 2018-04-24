using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.IO;

namespace CommonTools.Lib45.InputCommands
{
    [AddINotifyPropertyChangedInterface]
    public class TempExeLauncherCommand : ExeLauncherCommand
    {
        private const string TMP_DIR = "Temp_Exe_Copies";

        public TempExeLauncherCommand(string label, string origExeFilepath, string arguments = null) : base(label, origExeFilepath, arguments)
        {
            TempExePath = GetTempExePath();
        }


        public string TempExePath { get; }


        private string GetTempExePath()
        {
            var dir = Path.Combine(Path.GetTempPath(), TMP_DIR);
            return Path.Combine(dir, ExeName);
        }


        public override void Execute(object parameter)
        {
            KillProcess.ByName(ExeName, true);

            if (CopyOrigToTemp())
                StartExeProcess(TempExePath);

            UpdateVersionInfo();
        }


        private bool CopyOrigToTemp()
        {
            try
            {
                File.Delete(TempExePath);
                File.Copy(ExePath, TempExePath, true);
                return true;
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "Copying orig exe to temp");
                return false;
            }
        }
    }
}
