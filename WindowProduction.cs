using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace INV_MGMT_SYS
{
    public class WindowProduction : IWindowFactory
    {
        public void ShowWindow(object viewModel)
        {
            var win = new Window();
            win.Content = viewModel;
            win.Show();
        }
    }
}
