using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using static Supabase.Postgrest.Constants;
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
    }

    //public class 
    class DisplayPageViewModel : IPageViewModel, INotifyPropertyChanged
    {
        Client supabase;
        public string pageTitle;
        public string isDeleted;

        private readonly CollectionView _searchCategoryBox;
        private string _searchCategoryVal;
        private string _searchVal;
        private Aircon _selectedRow;

        private ObservableCollection<Aircon> _airconList;

        public DisplayPageViewModel(Client supabase)
        {
            this.pageTitle = "Inventory display";
            IsDeleted = "Hidden";
            this.supabase = supabase;

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

            this._airconList = new ObservableCollection<Aircon>();
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

        public string IsDeleted
        {
            get => this.isDeleted;
            set
            {
                this.isDeleted = value;
                OnPropertyChanged();
            }
        }

        public Aircon SelectedRow
        {
            get => this._selectedRow;
            set
            {
                this._selectedRow = value;
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
                OnPropertyChanged();
            }
        }
        #endregion

        #region Fetch Data
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
            //optimization required
            switch (SearchCategoryBox.CurrentPosition)
            {
                case 0:
                    {
                        var result = await supabase
                            .From<Aircon>()
                            .Where(x => x.id == _searchVal)
                            .Limit(50)
                            .Get();

                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 1:
                    {
                        var result = await supabase
                            .From<Aircon>()
                            .Filter(x => x.model, Operator.ILike, $"%{_searchVal}%")
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 2:
                    {
                        var result = await supabase
                            .From<Aircon>()
                            .Filter(x => x.brand, Operator.ILike, $"%{_searchVal}%")
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 3:
                    {
                        decimal _parsedVal;
                        decimal.TryParse(_searchVal, out _parsedVal);
                        var result = await supabase
                            .From<Aircon>()
                            .Where(x => x.hp == _parsedVal)
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 4:
                    {
                        var result = await supabase
                            .From<Aircon>()
                            .Filter(x => x.series, Operator.ILike, $"%{_searchVal}%")
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 5:
                    {
                        decimal _parsedVal;
                        decimal.TryParse(_searchVal, out _parsedVal);
                        var result = await supabase
                            .From<Aircon>()
                            .Where(x => x.price == _parsedVal)
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 6:
                    {
                        int _parsedVal;
                        int.TryParse(_searchVal, out _parsedVal);
                        var result = await supabase
                            .From<Aircon>()
                            .Where(x => x.stock == _parsedVal)
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                case 7:
                    {
                        var result = await supabase
                            .From<Aircon>()
                            .Filter(x => x.catalogueLink, Operator.ILike, $"%{_searchVal}%")
                            .Limit(50)
                            .Get();
                        AirconList = new ObservableCollection<Aircon>(result.Models);
                        break;
                    }
                default:
                    SearchValBox = "stuff went wrong";
                    break;
            }

            IsDeleted = "Hidden";
            _airconList.CollectionChanged += airconList_CollectionChanged;
        }
        #endregion

        private void EditRow()
        {
            WindowProduction winProd = new WindowProduction();
            winProd.ShowWindow(new EditViewModel(supabase, SelectedRow));
        }

        private async void DeleteRow()
        {
            await supabase
                .From<Aircon>()
                .Where(x => x.id == SelectedRow.id)
                .Delete();

            IsDeleted = "Visible";
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


        private void airconList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged += aircon_PropertyChanged;
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged -= aircon_PropertyChanged;
                }
            }
        }

        private void aircon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var row = sender as Aircon;
            SaveData(row);
        }

        private async void SaveData(Aircon row)
        {
            await supabase
                .From<Aircon>()
                .Where (x => x.id == row.id)
                .Set(x => x, row)
                .Update();
        }

        #region ICommand Impl
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

        private ICommand _edit;
        public ICommand EditCommand
        {
            get
            {
                if (_edit == null)
                {
                    _edit = new RelayCommand(
                        param => this.EditRow());
                }
                return _edit;
            }
        }

        private ICommand _delete;
        public ICommand DeleteCommand
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new RelayCommand(
                        param => this.DeleteRow());
                }
                return _delete;
            }
        }
        #endregion
    }
}
