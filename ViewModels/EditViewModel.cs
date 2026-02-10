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
    public class EditViewModel : INotifyPropertyChanged
    {
        Client supabase;
        public string pageTitle;
        public string isUpdated;

        private string id;
        private string _modelVal;
        private string _brandVal;
        private string _hpVal;
        private string _seriesVal;
        private string _priceVal;
        private string _stockVal;
        private string _catLinkVal;

        public EditViewModel(Client supabase, Aircon ac)
        {
            this.PageTitle = "Items edit";
            isUpdated = "";
            this.supabase = supabase;

            this.id = ac.id;
            this._modelVal = ac.model;
            this._brandVal = ac.brand;
            this._hpVal = ac.hp.ToString();
            this._seriesVal = ac.series;
            this._priceVal = ac.price.ToString();
            this._stockVal = ac.stock.ToString();
            this._catLinkVal = ac.catalogueLink;
        }

        #region INotifyPropertyChanged Impl
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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
        public string IsUpdated
        {
            get => this.isUpdated;
            set
            {
                this.isUpdated = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ICommand
        private ICommand _dataUpdate;
        public ICommand DataUpdateCommand
        {
            get
            {
                if (_dataUpdate == null)
                {
                    _dataUpdate = new RelayCommand(
                        param => this.DataUpdate());
                }
                return _dataUpdate;
            }
        }

        private ICommand _return;
        public ICommand ReturnCommand
        {
            get
            {
                if (_return == null)
                {
                    _return = new RelayCommand(
                        param => this.Return());
                }
                return _return;
            }
        }
        #endregion

        #region Update execution
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

        private async void DataUpdate()
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

            var model = await supabase
                .From<Aircon>()
                .Where(x => x.id == this.id)
                .Single();

            model.model = _modelVal;
            model.brand = _brandVal;
            model.hp = _parsedHp;
            model.series = _seriesVal;
            model.price = _parsedPrice;
            model.stock = _parsedStock;
            model.catalogueLink = _catLinkVal;

            await model.Update<Aircon>();
            Return();
        }
        #endregion

        public void Return()
        {
            //close the window
            //wip

            //temporary solution below
            IsUpdated = "Record updated";
        }
    }
}
