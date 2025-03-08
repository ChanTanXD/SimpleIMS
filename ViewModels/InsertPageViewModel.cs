using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Client = Supabase.Client;

namespace INV_MGMT_SYS.ViewModels
{
    class InsertPageViewModel : IPageViewModel
    {
        Client supabase;
        private string pageTitle;
        private string _modelVal;
        private string _brandVal;
        private string _hpVal;
        private string _seriesVal;
        private string _priceVal;
        private string _stockVal;
        private string _catLinkVal;

        public string PageTitle
        {
            get => this.pageTitle;
            set
            {
                this.pageTitle = value;
                OnPropertyChanged();
            }
        }
        public string ModelBox
        {
            get => this._modelVal;
            set
            {
                this._modelVal = value;
                OnPropertyChanged();
            }
        }
        public string BrandBox
        {
            get => this._brandVal;
            set
            {
                this._brandVal = value;
                OnPropertyChanged();
            }
        }
        public string HPBox
        {
            get => this._hpVal;
            set
            {
                this._hpVal = value;
                OnPropertyChanged();
            }
        }
        public string SeriesBox
        {
            get => this._seriesVal;
            set
            {
                this._seriesVal = value;
                OnPropertyChanged();
            }
        }
        public string PriceBox
        {
            get => this._priceVal;
            set
            {
                this._priceVal = value;
                OnPropertyChanged();
            }
        }
        public string StockBox
        {
            get => this._stockVal;
            set
            {
                this._stockVal = value;
                OnPropertyChanged();
            }
        }
        public string CatLinkBox
        {
            get => this._catLinkVal;
            set
            {
                this._catLinkVal = value;
                OnPropertyChanged();
            }
        }

        public InsertPageViewModel()
        {
            this.PageTitle = "Items insertion";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _dataInsert;
        public ICommand DataInsertCommand
        {
            get
            {
                if (_dataInsert == null)
                {
                    _dataInsert = new RelayCommand(
                        param => this.DataInsert(),
                        param => this.DataFieldCheck());
                }
                return _dataInsert;
            }
        }

        private bool DataFieldCheck()
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

        private async void DataInsert()
        {
            float _parsedHp;
            decimal _parsedPrice;
            int _parsedStock;

            if (!float.TryParse(_hpVal, out _parsedHp) ||
                !decimal.TryParse(_priceVal, out _parsedPrice) ||
                !int.TryParse(_stockVal, out _parsedStock))
            {
                TextParseError();
                return;
            }


            var model = new Aircon
            {
                model = _modelVal,
                brand = _brandVal,
                hp = _parsedHp,
                series = _seriesVal,
                price = _parsedPrice,
                stock = _parsedStock,
                catalogueLink = _catLinkVal
            };

            InitSupabase();
            await supabase.From<Aircon>().Insert(model);
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
