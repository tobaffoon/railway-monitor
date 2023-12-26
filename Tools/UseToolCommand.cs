using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace railway_monitor.Tools
{
    public class UseToolCommand : CommandBase<Canvas>
    {
        public override void Execute(object? parameter)
        {
            if(parameter == null) return;

            if (parameter is not Canvas)
            {
                throw new NotImplementedException("Command takes exactly one UIElement argument " + parameter.GetType() + " got instead");
            }

            ExecuteDelegate.Invoke((Canvas)parameter);
        }

        public UseToolCommand(Action<Canvas> executeDelegate) : base(executeDelegate)
        {
            if (executeDelegate.GetType() != typeof(Action<Canvas>))
            {
                throw new NotImplementedException("Command takes exactly one function with one UIElement argument");
            }
        }
        public UseToolCommand()
        {
        }
    }
}
