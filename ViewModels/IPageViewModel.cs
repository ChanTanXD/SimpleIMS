using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INV_MGMT_SYS.ViewModels
{
    // Base type for all pages
    interface IPageViewModel : INotifyPropertyChanged
    {
        public string PageTitle { get; set; }
    }
}
