using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INV_MGMT_SYS.ViewModels
{
    class DisplayPageViewModel : IPageViewModel
    {
        private string pageTitle;
        public string PageTitle
        {
            get => this.pageTitle;
            set
            {
                this.pageTitle = value;
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get => this.message;
            set
            {
                this.message = value;
                OnPropertyChanged();
            }
        }

        public DisplayPageViewModel()
        {
            this.PageTitle = "Inventory display";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
