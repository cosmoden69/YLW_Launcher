using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Ionic.Zip;

namespace YLWService.AutoUpdater
{
    public partial class frmLauncher : Form
    {
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr SetFocus(HandleRef hWnd);

        string INIfileName = Application.StartupPath + "\\" + "AutoLauncher.INI";
        string _executeProgram = "";
        string _executeParam = "";
        string _executeShell = "";
        string[] _args = null;

        public frmLauncher(string[] args)
        {
            _args = args;

            // 실행할 Project
            _executeParam = ""; //(args.Length > 0 ? args[0] : "");
            _executeProgram = "WebClient.exe";
            _executeShell = "ScriptRun.bat";

            InitializeComponent();

            KillProcess("WebClient");

            autoUpdater.RootUri = CommonUtil.INIFileRead(INIfileName, "LAUNCHER", "AUTOUPDATER_URL");

            autoUpdater.LocalRoot = Application.StartupPath;
            autoUpdater.UpdateListFileName = "updatinglist.xml";

            autoUpdater.LastUpdateFileName = CommonUtil.INIFileRead(INIfileName, "LAUNCHER", "LastUpdateFileName");
            if (autoUpdater.LastUpdateFileName == "")
            {
                autoUpdater.LastUpdateFileName = "19000101.01.ZIP";
                CommonUtil.SetIniValue("LAUNCHER", "LastUpdateFileName", "19000101.01.ZIP", INIfileName);
            }

            string[] fileList = Directory.GetFiles(autoUpdater.LocalRoot, "*.zip");
            foreach (string f in fileList)
            {
                File.Delete(f);
            }
            File.Delete(autoUpdater.LocalRoot + _executeShell);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_args.Length > 0 && _args[0].ToLower() == "silent")
            {
                Visible = false; // Hide form window.
                ShowInTaskbar = false; // Remove from taskbar.
                Opacity = 0;
            }

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // AutoUpdater의 속성을 설정 파일에서 읽는 예
//            autoUpdater.RootUri = Settings.Default.RootUri;

            autoUpdater.Run();
        }

        private void autoUpdater_FileTransfered(object sender, AutoUpdater.FileTransferedEventArgs e)
        {
            lsbFiles.Items.Add(e.RemoteFile.LocalPath);
            lsbFiles.SelectedIndex = lsbFiles.Items.Count - 1;

            if (new System.IO.FileInfo(e.RemoteFile.LocalPath).Exists)
            {
                bool bExtractOk = false;
                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(e.RemoteFile.LocalPath))
                {
                    zip.ExtractAll(autoUpdater.LocalRoot, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);

                    string file = Path.GetFileName(e.RemoteFile.LocalPath);
                    CommonUtil.SetIniValue("LAUNCHER", "LastUpdateFileName", file, INIfileName);
                    autoUpdater.LastUpdateFileName = file;
                    bExtractOk = true;
                }
                if (bExtractOk) System.IO.File.Delete(e.RemoteFile.LocalPath);

                if (File.Exists(autoUpdater.LocalRoot + _executeShell))
                {
                    ProcessStartInfo cmd = new ProcessStartInfo(autoUpdater.LocalRoot + _executeShell);
                    cmd.WorkingDirectory = autoUpdater.LocalRoot;
                    cmd.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(cmd);
                    Thread.Sleep(500);
                    File.Delete(autoUpdater.LocalRoot + _executeShell);
                }
            }
            else
            {
                MessageBox.Show("Problem with download. File does not exist.");
            }
        }

        private void autoUpdater_FileTransfering(object sender, AutoUpdater.FileTransferingEventArgs e)
        {
            lblFile.Text = string.Format("{0} ({1:N0} / {2:N0}) ", e.RemoteFile.LocalPath, e.TransferingInfo.TransferedFileCount + 1, e.TransferingInfo.TotalFileCount);
        }

        private void autoUpdater_UpdatableListFound(object sender, AutoUpdater.UpdatableListFoundEventArgs e)
        {
            lblUpdatableList.Text = string.Format("업데이트할 파일이 {0}개 있습니다.", e.RemoteFiles.Count);
        }

        private void autoUpdater_UpdateProgressChanged(object sender, AutoUpdater.UpdateProgressChangedEventArgs e)
        {
            int percent = e.TransferingInfo.LengthPercent;
            prbProgress.Value = percent;
            lblLength.Text = string.Format("{0:N0} / {1:N0} ({2}%), 남은 시간 : {3:N0} 초", e.TransferingInfo.TotalTransferedLength, e.TransferingInfo.TotalLength, percent, e.TransferingInfo.RemainingSeconds);
        }

        private void autoUpdater_UpdateCompleted(object sender, AutoUpdater.UpdateCompletedEventArgs e)
        {
            try
            {
                Thread.Sleep(500);
                Process p = Process.Start(autoUpdater.LocalRoot + _executeProgram, _executeParam);
                SetFocus(new HandleRef(null, p.MainWindowHandle));
            }
            catch (Exception xe)
            {
                MessageBox.Show("Process Start Error : " + xe.Message);
            }
            finally
            {
                Application.Exit();
            }
        }

        private void KillProcess(string pname)
        {
            try
            {
                Process[] plist = Process.GetProcessesByName(pname);
                for (int ii = 0; ii < plist.Length; ii++) plist[ii].Kill();
            }
            catch { }
        }
    }
}