using railway_monitor.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace railway_monitor.MVVM.Views {
    /// <summary>
    /// Interaction logic for RailwayMonitorView.xaml
    /// </summary>
    public partial class RailwayMonitorView : UserControl {
        private RailwayMonitorViewModel Context => (RailwayMonitorViewModel)DataContext;

        public RailwayMonitorView() {
            InitializeComponent();
        }

        private void ExitMonitor(object sender, RoutedEventArgs e) {
            Context.FinishMonitoring();
        }

        private void StartSimulation(object sender, RoutedEventArgs e) {

        }

        private void OnCanvasKeyDown(object sender, KeyEventArgs e) {
            
        }
    }
}
