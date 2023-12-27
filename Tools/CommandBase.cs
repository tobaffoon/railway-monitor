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
        public Action<T>? ExecuteDelegate { get; set; }

        public CommandBase(Action<T> executeDelegate)
        {
            ExecuteDelegate = executeDelegate;
        }
        public CommandBase()
        {
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
