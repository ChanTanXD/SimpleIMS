using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace INV_MGMT_SYS
{
    /// <summary>
    /// Interaction logic for OptionWin.xaml
    /// </summary>
    public partial class OptionWin : Window
    {
        public OptionWin()
        {
            InitializeComponent();
        }
        private void ModOnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            Application.Current.MainWindow = new DisplayWin();
        }
        private void InsertOnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            InsertWin win = new InsertWin();
            win.Show();
            Close();
        }
    }
}
