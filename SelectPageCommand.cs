using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace INV_MGMT_SYS
{
    class SelectPageCommand : ICommand
    {
        private Action<object> ExecuteDelegate { get; }
        private Predicate<object> CanExecuteDelegate { get; }

        public SelectPageCommand(Action<object> executeDelegate, Predicate<object> canExecuteDelegate)
        {
            this.ExecuteDelegate = executeDelegate;
            this.CanExecuteDelegate = canExecuteDelegate;
        }


        #region Implementation of ICommand

        public bool CanExecute(object parameter) => this.CanExecuteDelegate?.Invoke(parameter) ?? false;

        public void Execute(object parameter) => this.ExecuteDelegate?.Invoke(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion
    }
}
