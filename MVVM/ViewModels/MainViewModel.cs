namespace railway_monitor.MVVM.ViewModels {
    public class MainViewModel : ViewModelBase {
        public enum ViewModelName {
            Undefined = 0, Start, RailwayMonitor
        }

        private Dictionary<ViewModelName, ViewModelBase> ViewModels { get; }

        private ViewModelBase _selectedViewModel;
        public ViewModelBase SelectedViewModel { 
            get => _selectedViewModel;
            set => SetField(ref _selectedViewModel, value);
        }

        public MainViewModel() {
            ViewModels = new Dictionary<ViewModelName, ViewModelBase>
            {
                { ViewModelName.RailwayMonitor, new RailwayMonitorViewModel(this) },
                { ViewModelName.Start, new StartViewModel(this) }
            };

            SelectedViewModel = ViewModels[ViewModelName.Start];
        }

        public void SelectView(object param) {
            if (param is ViewModelName viewModelName &&
                    ViewModels.TryGetValue(viewModelName, out ViewModelBase? selectedViewModel)) {
                SelectedViewModel = selectedViewModel;
            }
        }
    }
}
