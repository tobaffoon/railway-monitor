using railway_monitor.MVVM.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace railway_monitor.Components.RailwayCanvas {
    /// <summary>
    /// Interaction logic for RailwayCanvas.xaml
    /// </summary>
    public partial class RailwayCanvas : Canvas {
        public RailwayCanvas() {
            InitializeComponent();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            contex.MoveCommand.Execute(Tuple.Create(contex.RailwayCanvas, cursor));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            contex.LeftClickCommand.Execute(Tuple.Create(contex.RailwayCanvas, cursor));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            contex.LeftReleaseCommand.Execute(contex.RailwayCanvas);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e) {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            contex.RightClickCommand.Execute(contex.RailwayCanvas);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            Trace.WriteLine(e.Delta);
            //contex.RightClickCommand.Execute(Tuple.Create(contex.RailwayCanvas, cursor)); // nonono
        }
    }
}
