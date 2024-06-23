using railway_monitor.MVVM.ViewModels;
using SolverLibrary.Model;
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
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "schedule";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON documents (.json)|*.json";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == false) {
                return;
            }

            // Open document
            string filename = dialog.FileName;
            TrainSchedule schedule = SolverLibrary.JsonDoc.JsonParser.LoadJsonTrainSchedule(filename, Context.Graph);

            Context.Start(schedule);
        }

        private void OnCanvasKeyDown(object sender, KeyEventArgs e) {
            
        }
    }
}
