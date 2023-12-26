using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualBasic.CompilerServices;

namespace railway_monitor.Tools
{
    public abstract class CommandBase : ICommand
    {
        Action<object> _executeDelegate;

        public Action<object> ExecuteDelegate
        {
            get => (Action<object>)_executeDelegate;
            set => _executeDelegate = value;
        }

        public CommandBase(Action<object> executeDelegate)
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
