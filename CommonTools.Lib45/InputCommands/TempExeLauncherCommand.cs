﻿using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.InputCommands
{
    [AddINotifyPropertyChangedInterface]
    public class TempExeLauncherCommand : ExeLauncherCommand
    {
        private const string TMP_DIR = "Temp_Exe_Copies";

        public TempExeLauncherCommand(string label, string origExeFilepath, string arguments = null, int afterCopyDelayMS = 200) : base(label, origExeFilepath, arguments)
        {
            AfterCopyDelayMS = afterCopyDelayMS;
            TempExePath      = GetTempExePath();
        }


        public int     AfterCopyDelayMS  { get; }
        public string  TempExePath       { get; }


        private string GetTempExePath() 
            => Path.Combine(GetTempExeDir(), ExeName);


        private string GetTempExeDir()
        {
            var dir = Path.Combine(Path.GetTempPath(), TMP_DIR);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }



        public override async void Execute(object parameter)
        {
            var splash = new SplashScreen(Assembly.GetAssembly(typeof(TempExeLauncherCommand)), "loading.png");
            splash.Show(false, true);

            KillProcess.ByName(ExeName, true);

            if (await CopyOrigToTemp())
                StartExeProcess(TempExePath);

            UpdateVersionInfo();
            splash.Close(TimeSpan.FromSeconds(2));
        }


        private async Task<bool> CopyOrigToTemp()
        {
            try
            {
                if (File.Exists(TempExePath))
                    File.Delete(TempExePath);

                File.Copy(ExePath, TempExePath, true);
                await Task.Delay(AfterCopyDelayMS);

                return true;
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "Copying orig exe to temp");
                return false;
            }
        }


        //private bool IsFileLocked(FileInfo file)
        //{
        //    FileStream stream = null;
        //    try
        //    {
        //        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        //    }
        //    catch (IOException)
        //    {
        //        //the file is unavailable because it is:
        //        //still being written to
        //        //or being processed by another thread
        //        //or does not exist (has already been processed)
        //        return true;
        //    }
        //    finally
        //    {
        //        if (stream != null)
        //            stream.Close();
        //    }
        //    //file is not locked
        //    return false;
        //}
    }
}
