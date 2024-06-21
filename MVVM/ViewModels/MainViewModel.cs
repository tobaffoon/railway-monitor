using railway_monitor.Bases;
using System.Windows.Input;

namespace railway_monitor.MVVM.ViewModels {
    public class MainViewModel : ViewModelBase {
        public enum ViewModelName {
            Undefined = 0, Start, RailwayMonitor
        }

        public ICommand SelectViewCommand => new CommandBase<ViewModelName>(SelectView);

        private Dictionary<ViewModelName, ViewModelBase> ViewModels { get; }

        public ViewModelBase SelectedViewModel { get; set; }

        public MainViewModel() {
            ViewModels = new Dictionary<ViewModelName, ViewModelBase>
            {
                { ViewModelName.RailwayMonitor, new RailwayMonitorViewModel() },
                { ViewModelName.Start, new StartViewModel() }
            };

            SelectedViewModel = ViewModels[ViewModelName.Start];
        }

        public void SelectView(ViewModelName viewModelName) {
            if (ViewModels.TryGetValue(viewModelName, out ViewModelBase? selectedViewModel)) {
                this.SelectedViewModel = selectedViewModel;
            }
        }
    }
}
