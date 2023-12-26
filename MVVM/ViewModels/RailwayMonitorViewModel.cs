using System.Windows;
using railway_monitor.Tools;

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
