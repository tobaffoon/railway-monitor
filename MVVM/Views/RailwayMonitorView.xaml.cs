using railway_monitor.MVVM.ViewModels;
using railway_monitor.Utils;
using SolverLibrary.Model.Graph;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace railway_monitor.MVVM.Views {
    /// <summary>
    /// Interaction logic for RailwayMonitorView.xaml
    /// </summary>
    public partial class RailwayMonitorView : UserControl {
        private RailwayMonitorViewModel Context => (RailwayMonitorViewModel) DataContext;

        public RailwayMonitorView() {
            InitializeComponent();

            Loaded += (x, y) => Keyboard.Focus(railwayCanvas);
        }

        private void FocusCanvas(object sender, KeyEventArgs e) {
            if (!railwayCanvas.IsFocused) {
                railwayCanvas.Focus();
            }
        }

        private void OnCanvasKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    Context.ArrowsCommand.Execute(Tuple.Create(Context.RailwayCanvas, e.Key));
                    break;
                default:
                    Context.CanvasKeyboardCommand.Execute(Tuple.Create(Context.RailwayCanvas, e.Key));
                    break;
            }
        }

        private void FinishDesigning(object sender, System.Windows.RoutedEventArgs e) {
            // create graph
            StationGraph graph;
            try {
                graph = GraphUtils.CreateGraph(Context.RailwayCanvas).Item1;
            }
            catch (ArgumentException exc) {
                MessageBox.Show(exc.Message);
                return;
            }

            // Configure save file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = DateTime.Today.ToString("MM-dd-yyyy") + "-station";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON documents (.json)|*.json";

            // Show save file dialog box
            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == false) {
                return;
            }

            // Save document
            string filename = dialog.FileName;
            SolverLibrary.JsonDoc.JsonParser.SaveJsonStationGraph(filename, graph);
            Context.FinishDesigning();
        }
    }
}
