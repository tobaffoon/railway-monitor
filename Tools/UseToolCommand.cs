using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace railway_monitor.Tools
{
    public class UseToolCommand : CommandBase
    {
        public void Execute(UIElement canvas)
        {
            ExecuteDelegate.Invoke(canvas);
        }
        public override void Execute(object? parameter)
        {
            throw new NotImplementedException("Command takes exactly one UIElement argument");
        }

        public UseToolCommand(Action<object> executeDelegate) : base(executeDelegate)
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
