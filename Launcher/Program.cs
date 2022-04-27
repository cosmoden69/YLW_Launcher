using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace YLWService.AutoUpdater
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
			try
			{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                while (true)
                {
                    if (CommonUtil.IsInternetConnected()) break;
                    CommonUtil.Delay(5 * 1000);
                }

                Application.Run(new frmLauncher(args));
            }
            catch (Exception xe)
            {
                MessageBox.Show("Laucher Start Error : " + xe.Message);
            }
        }
    }
}
