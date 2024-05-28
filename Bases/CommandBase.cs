using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualBasic.CompilerServices;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Bases
{
    public class CommandBase<T> : ICommand
    {
        public Action<T> ExecuteDelegate { get; set; } = (param) => { };

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

        public virtual void Execute(object? parameter)
        {
            if (parameter == null) return;

            if (parameter is not T)
            {
                throw new NotImplementedException("Command takes " + typeof(T).Name + ". " + parameter.GetType() + " got instead");
            }

            ExecuteDelegate.Invoke((T)parameter);
        }

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
