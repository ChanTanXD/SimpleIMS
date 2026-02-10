using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INV_MGMT_SYS
{
    public interface IWindowFactory
    {
        void ShowWindow(object dataContext);
    }
}
