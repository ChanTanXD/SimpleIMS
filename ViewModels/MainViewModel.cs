using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Client = Supabase.Client;

namespace INV_MGMT_SYS.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public Client supabase;

        public MainViewModel()
        {
            InitSupabase();

            this.Pages = new ObservableCollection<IPageViewModel>()
            {
              new DisplayPageViewModel(supabase),
              new InsertPageViewModel(supabase)
            };

            // Show startup page
            this.SelectedPage = this.Pages.First();
        }

        // Define the Execute and CanExecute delegates for the command
        // and pass them to the constructor
        public ICommand SelectPageFromIndexCommand => new RelayCommand(
          param => this.SelectedPage = this.Pages.ElementAt(int.Parse(param as string)),
          param => int.TryParse(param as string, out int index));

        // Define the Execute and CanExecute delegates for the command
        // and pass them to the constructor
        public ICommand SelectNextPageCommand => new RelayCommand(
          param => this.SelectedPage = this.Pages.ElementAt(this.Pages.IndexOf(this.SelectedPage) + 1),
          param => this.Pages.IndexOf(this.SelectedPage) + 1 < this.Pages.Count);

        private IPageViewModel selectedPage;
        public IPageViewModel SelectedPage
        {
            get => this.selectedPage;
            set
            {
                if (object.Equals(value, this.selectedPage))
                    return;

                this.selectedPage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IPageViewModel> pages;
        public ObservableCollection<IPageViewModel> Pages
        {
            get => this.pages;
            set
            {
                if (object.Equals(value, this.pages))
                    return;

                this.pages = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void InitSupabase()
        {
            Console.WriteLine("Init started");
            NetworkStatus NetworkStatus = new();

            SupabaseOptions options = new();
            options.AutoRefreshToken = true;

            const string projURL = "https://hhyvnenrgtwuntghhxdn.supabase.co";
            const string publicAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhoeXZuZW5yZ3R3dW50Z2hoeGRuIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Njk1ODM0MDIsImV4cCI6MjA4NTE1OTQwMn0.gub0b8WEm2e-ROVXD2OTv3yYD-3adNGBKEUUtbsr6Ms";

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
        }
    }
}
