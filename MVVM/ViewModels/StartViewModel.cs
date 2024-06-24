using railway_monitor.Bases;
using railway_monitor.Utils;
using SolverLibrary.Model.Graph;
using System.Windows.Input;

namespace railway_monitor.MVVM.ViewModels {
    public class StartViewModel : ViewModelBase {
        private MainViewModel _mainViewModel;
        public ICommand DesignCommand => new RelayCommand(StartDesigning);

        public StartViewModel(MainViewModel mainViewModel) {
            _mainViewModel = mainViewModel;
        }

        private void StartDesigning() {
            _mainViewModel.SelectView(MainViewModel.ViewModelName.RailwayDesigner);
        }

        internal void StartMonitoring(StationGraph graph) {
            _mainViewModel.SelectView(MainViewModel.ViewModelName.RailwayMonitor);
            if (_mainViewModel.SelectedViewModel is not RailwayMonitorViewModel monitor) {
                _mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
                return;
            }

            GraphUtils.AddTopologyFromGraph(monitor.RailwayCanvas, graph);
            monitor.Graph = graph;
        }
    }
}
