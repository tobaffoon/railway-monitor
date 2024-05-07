using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.ToolButtons;
using railway_monitor.Tools;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace railway_monitor.MVVM.ViewModels
{
    public class RailwayMonitorViewModel : ViewModelBase
    {
        public static readonly DependencyProperty MoveCommandProperty =
            DependencyProperty.Register(
            "MoveCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand MoveCommand
        {
            get { return (UseToolCommand)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandProperty = 
            DependencyProperty.Register(
            "ClickCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand ClickCommand
        {
            get { return (UseToolCommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public ToolButtonsViewModel ToolButtons { get; } =  new ToolButtonsViewModel();
        public RailwayCanvasViewModel RailwayCanvas { get; } = new RailwayCanvasViewModel();

        public RailwayMonitorViewModel()
        {
            var clickBinding = new Binding("ClickCommand")
            {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, ClickCommandProperty, clickBinding);
            var moveBinding = new Binding("MoveCommand")
            {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, MoveCommandProperty, moveBinding);
        }
    }
}
