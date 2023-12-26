using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using railway_monitor.Components.ToolButtons;
using railway_monitor.Tools;
using railway_monitor.Tools.DrawCommands;

namespace railway_monitor.MVVM.ViewModels
{
    public class RailwayMonitorViewModel : ViewModelBase
    {
        public static readonly DependencyProperty CurrentToolCommandProperty = DependencyProperty.Register(
            "CurrentToolCommandProperty",
            typeof(CommandBase),
            typeof(RailwayMonitorViewModel),
            new FrameworkPropertyMetadata(DrawToolCommands.DrawStraightRailTrack,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CommandBase CurrentToolCommand => (CommandBase)(GetValue(CurrentToolCommandProperty));

        public RailwayMonitorViewModel()
        {
        }
    }
}
