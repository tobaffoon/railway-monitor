using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using railway_monitor.Tools;
using railway_monitor.Tools.DrawCommands;

namespace railway_monitor.MVVM.ViewModels
{
    class RailwayMonitorViewModel : ViewModelBase
    {
        public ICommand CurrentToolCommand { get; set; }

        public RailwayMonitorViewModel()
        {
            CurrentToolCommand = new DrawSRTCommand();
        }
    }
}
