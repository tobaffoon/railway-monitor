using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualBasic.CompilerServices;

namespace railway_monitor.Tools
{
    public abstract class CommandBase<T> : ICommand
    {
        Action<T> _executeDelegate;

        public Action<T> ExecuteDelegate
        {
            get => (Action<T>)_executeDelegate;
            set => _executeDelegate = value;
        }

        public CommandBase(Action<T> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }
        public CommandBase()
        {
            _executeDelegate = (x => throw new IncompleteInitialization());
        }

        public event EventHandler? CanExecuteChanged;
        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public abstract void Execute(object? parameter);

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
