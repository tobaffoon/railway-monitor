using railway_monitor.Bases;
using System.Windows.Input;

namespace railway_monitor.MVVM.ViewModels {
    public class StartViewModel : ViewModelBase {
        private MainViewModel _mainViewModel;
        public ICommand DesignCommand => new RelayCommand(StartDesigning);

        public StartViewModel(MainViewModel mainViewModel) {
            _mainViewModel = mainViewModel;
        }

        private void StartDesigning() {
            _mainViewModel.SelectView(MainViewModel.ViewModelName.RailwayMonitor);
        }
    }
}
