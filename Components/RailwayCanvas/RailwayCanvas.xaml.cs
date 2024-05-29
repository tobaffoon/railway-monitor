using railway_monitor.Componenusing railway_monitor.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace railway_monitor.Components.RailwayCanvas
{
    /// <summary>
    /// Interaction logic for RailwayCanvas.xaml
    /// </summary>
    public partial class RailwayCanvas : Canvas
    {
        public RailwayCanvas()
        {
            InitializeComponent();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            contex.MoveCommand.Execute(Tuple.Create(contex.RailwayCanvas, cursor));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            Point cursor = e.MouseDevice.GetPosition(this);
            contex.ClickCommand.Execute(Tuple.Create(contex.RailwayCanvas, cursor));
        }
    }
}
