using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace INV_MGMT_SYS
{
    /// <summary>
    /// Initiate connection with supabase.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
        }
    }
}
