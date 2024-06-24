using railway_monitor.MVVM.ViewModels;
using SolverLibrary.Model.Graph;
using System.Windows.Controls;

namespace railway_monitor.MVVM.Views {
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl {
        private StartViewModel Context => (StartViewModel)DataContext;

        public StartView() {
            InitializeComponent();
        }

        private void StartMonitoring(object sender, System.Windows.RoutedEventArgs e) {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "station";
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
            StationGraph graph = SolverLibrary.JsonDoc.JsonParser.LoadJsonStationGraph(filename);

            Context.StartMonitoring(graph);
        }
    }
}
