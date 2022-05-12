using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                //this.Close();
                return;
            }

        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bwallpaper = @"/C reg add HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop /v NoChangingWallPaper /t REG_DWORD /d 1 /f ";
            Process.Start("cmd.exe",bwallpaper);
            Process.Start("cmd.exe", "/C RUNDLL32.EXE user32.dll,UpdatePerUserSystemParameters");
            System.Windows.Forms.MessageBox.Show("Wallpaper Bloqueado");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dwallpaper = @"/C reg add HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop /v NoChangingWallPaper /t REG_DWORD /d 0 /f ";
            Process.Start("cmd.exe", dwallpaper);
            Process.Start("cmd.exe", "/C RUNDLL32.EXE user32.dll,UpdatePerUserSystemParameters");
            System.Windows.Forms.MessageBox.Show("Wallpaper Desbloqueado");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files | *.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            //MessageBox.Show(fileContent, "Caminho do Arquivo: " + filePath, MessageBoxButtons.OK);
            
            string awallpaper = @"/C reg add HKEY_CURRENT_USER\Control Panel\Desktop\ /v WallPaper /t REG_SZ /d " + filePath + " /f";
            Process.Start("cmd.exe", awallpaper);
            string bwallpaper = @"/C reg add HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop /v NoChangingWallPaper /t REG_DWORD /d 1 /f ";
            Process.Start("cmd.exe", bwallpaper);
            string bregedit = @"/C reg add HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\ /v DisableRegistryTools /t REG_DWORD /d 1 /f";
            Process.Start("cmd.exe", bregedit);
            Process.Start("cmd.exe", "/C RUNDLL32.EXE user32.dll,UpdatePerUserSystemParameters");
            MessageBox.Show("Wallpaper Alterado e registro bloqueado");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
