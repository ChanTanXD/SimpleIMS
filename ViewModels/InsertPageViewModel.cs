using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

        public InsertPageViewModel(Client supabase)
        {
            this.PageTitle = "Items insertion";
            this.supabase = supabase;
        }

        #region Properties
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ICommand _dataInsert;
        public ICommand DataInsertCommand
        {
            get
            {
                if (_dataInsert == null)
                {
                    _dataInsert = new RelayCommand(
                        param => this.DataInsert());
                }
                return _dataInsert;
            }
        }

        private ICommand _dataClear;
        public ICommand DataClearCommand
        {
            get
            {
                if (_dataClear == null)
                {
                    _dataClear = new RelayCommand(
                        param => this.DataClear());
                }
                return _dataClear;
            }
        }

        private bool DataFieldCheck()
        {
            if(!Regex.IsMatch(_modelVal, @"[^A-Za-z0-9]+") &&
            !Regex.IsMatch(_brandVal, @"[^A-Za-z0-9]+") &&
            Regex.IsMatch(_hpVal, @"^(\d{0,9}|\d{0,9}\.\d{1,3})$") &&
            !Regex.IsMatch(_seriesVal, @"[^A-Za-z0-9]+") &&
            Regex.IsMatch(_priceVal, @"^(\d{0,9}|\d{0,9}\.\d{1,3})$") &&
            Regex.IsMatch(_stockVal, @"^(\d{0,9})$") &&
            !Regex.IsMatch(_catLinkVal, @"[^A-Za-z0-9]+"))
            {
                return true;
            }
            else
                return false;
        }

        private async void DataInsert()
        {
            if (!DataFieldCheck())
                return;

            decimal _parsedHp;
            decimal _parsedPrice;
            int _parsedStock;

            if (!decimal.TryParse(_hpVal, out _parsedHp) ||
                !decimal.TryParse(_priceVal, out _parsedPrice) ||
                !int.TryParse(_stockVal, out _parsedStock))
            {
                Console.WriteLine("Parsing error on DataInsert()");
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

            await supabase.From<Aircon>().Insert(model);
            DataClear();
        }

        public void DataClear()
        {
            ModelBox = string.Empty;
            BrandBox = string.Empty;
            HPBox = string.Empty;
            SeriesBox = string.Empty;
            PriceBox = string.Empty;
            StockBox = string.Empty;
            CatLinkBox = string.Empty;
        }
    }
}
