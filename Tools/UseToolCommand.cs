using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace railway_monitor.Tools
{
    public class UseToolCommand : CommandBase<UIElement>
    {
        public override void Execute(object? parameter)
        {
            if(parameter == null) return;

            if (parameter.GetType() != typeof(UIElement))
            {
                throw new NotImplementedException("Command takes exactly one UIElement argument");
            }

            ExecuteDelegate.Invoke((UIElement)parameter);
        }

        public UseToolCommand(Action<UIElement> executeDelegate) : base(executeDelegate)
        {
            if (executeDelegate.GetType() != typeof(Action<UIElement>))
            {
                throw new NotImplementedException("Command takes exactly one function with one UIElement argument");
            }
        }
        public UseToolCommand()
        {
        }
    }
}
