using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace INV_MGMT_SYS.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Pages = new ObservableCollection<IPageViewModel>()
            {
              new DisplayPageViewModel(),
              new InsertPageViewModel()
            };

            // Show startup page
            this.SelectedPage = this.Pages.First();
        }

        // Define the Execute and CanExecute delegates for the command
        // and pass them to the constructor
        public ICommand SelectPageFromIndexCommand => new SelectPageCommand(
          param => this.SelectedPage = this.Pages.ElementAt(int.Parse(param as string)),
          param => int.TryParse(param as string, out int index));

        // Define the Execute and CanExecute delegates for the command
        // and pass them to the constructor
        public ICommand SelectNextPageCommand => new SelectPageCommand(
          param => this.SelectedPage = this.Pages.ElementAt(this.Pages.IndexOf(this.SelectedPage) + 1),
          param => this.Pages.IndexOf(this.SelectedPage) + 1 < this.Pages.Count);

        private IPageViewModel selectedPage;
        public IPageViewModel SelectedPage
        {
            get => this.selectedPage;
            set
            {
                if (object.Equals(value, this.selectedPage))
                {
                    return;
                }

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
                {
                    return;
                }

                this.pages = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
