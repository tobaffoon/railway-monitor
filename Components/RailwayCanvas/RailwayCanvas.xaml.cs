using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.ToolButtons;
using railway_monitor.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
