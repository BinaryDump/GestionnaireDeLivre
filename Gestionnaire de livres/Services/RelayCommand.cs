using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gestionnaire_de_livres.Services
{
    class RelayCommand : ICommand
    {
        readonly Action<object> _action;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {

        }

        public RelayCommand(Action<object> action, Predicate<object> canExecute)
        {
            if (action == null) throw new ArgumentNullException("action");

            _action = action;
            _canExecute = canExecute;

        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
                _action(parameter);
        }

        #endregion
    }
}
