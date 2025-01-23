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
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Client = Supabase.Client;

namespace INV_MGMT_SYS
{
    /// <summary>
    /// Interaction logic for InsertWin.xaml
    /// </summary>
    public partial class InsertWin : Window
    {
        Client supabase;
        public InsertWin()
        {
            InitializeComponent();
            //InitSupabase();
        }
        private void ReturnOnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            OptionWin win = new OptionWin();
            win.Show();
            Close();
        }
        private async void InsertOnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            var model = new Aircon
            {
                model = "AAAAAA",
                brand = "ADDIDAS",
                hp = 1.0f,
                series = "non-inverter",
                price = 88.88d,
                stock = 99,
                catalogueLink = "websiteA"
            };

            //await supabase.From<Aircon>().Insert(model);
        }

        private async void InitSupabase()
        {
            Console.WriteLine("Init started");
            NetworkStatus NetworkStatus = new();

            SupabaseOptions options = new();
            options.AutoRefreshToken = true;

            const string projURL = "https://shkofncmerdkpthgpihu.supabase.co";
            const string publicAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNoa29mbmNtZXJka3B0aGdwaWh1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzczNDEyNDQsImV4cCI6MjA1MjkxNzI0NH0.W_aqHJZuXG5bA7CzbSeCtaqK-GRJAuQqJfDte5rYXkg";

            supabase = new Client(projURL, publicAnonKey, options);

            NetworkStatus.Client = (Supabase.Gotrue.Client)supabase.Auth;

            supabase.Auth.LoadSession();

            supabase.Auth.Options.AllowUnconfirmedUserSessions = true;

            // This is a well-known URL that is used to test network connectivity.
            // We use this to determine if the network is up or down.
            string url =
                @"{SupabaseSettings.SupabaseURL}/auth/v1/settings?apikey={SupabaseSettings.SupabaseAnonKey}";
            try
            {
                // We start the network status object. This will attempt to connect to the
                // well-known URL and determine if the network is up or down.
                supabase!.Auth.Online = await NetworkStatus.StartAsync(url);
            }
            catch (NotSupportedException)
            {
                // On certain platforms, the NetworkStatus object may not be able to determine
                // the network status. In this case, we just assume the network is up.
                supabase!.Auth.Online = true;
            }
            catch (Exception e)
            {
                // If there are other kinds of error, we assume the network is down,
                // and in this case we send the error to a UI element to display to the user.
                // This PostMessage method is specific to this application - you will
                // need to adapt this to your own application.
                supabase!.Auth.Online = false;
            }
            if (supabase.Auth.Online)
            {
                // If the network is up, we initialize the Supabase client.
                await supabase.InitializeAsync();
            }
            Console.WriteLine("Init ended");
        }
    }
}
