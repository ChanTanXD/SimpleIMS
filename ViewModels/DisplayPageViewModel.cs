using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using static System.Reflection.Metadata.BlobBuilder;
using Client = Supabase.Client;

namespace INV_MGMT_SYS.ViewModels
{
    public class SearchCategoryVal
    {
        public string val {  get; set; }
        public SearchCategoryVal(string val)
        {
            this.val = val;
        }
        public override string ToString()
        {
            return val;
        }
    }
    //public class 
    class DisplayPageViewModel : IPageViewModel
    {
        Client supabase;
        private string pageTitle;
        private readonly CollectionView _searchCategoryBox;
        private string _searchCategoryVal;
        private string _searchVal;

        private ObservableCollection<Aircon> _airconList;
        private int _idField;

        public string PageTitle
        {
            get => this.pageTitle;
            set
            {
                this.pageTitle = value;
                OnPropertyChanged();
            }
        }
        public CollectionView SearchCategoryBox
        {
            get { return _searchCategoryBox; }
        }
        public string SearchCategoryVal
        {
            get => this._searchCategoryVal;
            set
            {
                _searchCategoryVal = value;
                OnPropertyChanged();
            }
        }
        public string SearchValBox
        {
            get => this._searchVal;
            set
            {
                this._searchVal = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Aircon> AirconList
        {
            get => this._airconList;
            set
            {
                this._airconList = value;
                OnPropertyChanged("AirconList");
            }
        }

        public DisplayPageViewModel()
        {
            this.PageTitle = "Inventory display";

            IList<SearchCategoryVal> list = new List<SearchCategoryVal>();
            list.Add(new SearchCategoryVal("ID"));
            list.Add(new SearchCategoryVal("Model"));
            list.Add(new SearchCategoryVal("Brand"));
            list.Add(new SearchCategoryVal("HP"));
            list.Add(new SearchCategoryVal("Series"));
            list.Add(new SearchCategoryVal("Price"));
            list.Add(new SearchCategoryVal("Stock"));
            list.Add(new SearchCategoryVal("Link"));
            _searchCategoryBox = new CollectionView(list);
            _airconList = new ObservableCollection<Aircon>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _search;
        public ICommand SearchCommand
        {
            get
            {
                if (_search == null)
                {
                    _search = new RelayCommand(
                        param => this.FetchData(),
                        param => this.FetchDataCheck());
                }
                return _search;
            }
        }

        private bool FetchDataCheck()
        {
            //Check if all fields are valid
            //wip
            return true;
        }
        public void TextParseError()
        {
            //wip
            return;
        }

        private async void FetchData()
        {
            InitSupabase();

            //optimization required
            switch(SearchCategoryBox.CurrentPosition)
            {
                case 0:
                {
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.id == _searchVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                case 1:
                {
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.model == _searchVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                    }
                case 2:
                {
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.brand == _searchVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                case 3:
                {
                    float _parsedVal;
                    float.TryParse(_searchVal, out _parsedVal);
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.hp == _parsedVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                case 4:
                {
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.series == _searchVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                case 5:
                {
                    decimal _parsedVal;
                    decimal.TryParse(_searchVal, out _parsedVal);
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.price == _parsedVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                case 6:
                {
                    int _parsedVal;
                    int.TryParse(_searchVal, out _parsedVal);
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.stock == _parsedVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                case 7:
                {
                    var result = await supabase
                        .From<Aircon>()
                        .Where(x => x.catalogueLink == _searchVal)
                        .Get();
                    _airconList = new ObservableCollection<Aircon>(result.Models);
                    break;
                }
                default:
                    SearchValBox = "shit went wrong";
                    break;
            }

            DisplayAirconList();
        }
        public void DisplayAirconList()
        {

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
        }
    }
}
