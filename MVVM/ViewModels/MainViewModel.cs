using railway_monitor.Bases;
using railway_monitor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace railway_monitor.MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public enum ViewModelName
        {
            Undefined = 0, RailwayMonitor
        }

        public ICommand SelectViewCommand => new CommandBase<ViewModelName>(SelectView);

        private Dictionary<ViewModelName, ViewModelBase> ViewModels { get; }

        public ViewModelBase SelectedViewModel { get; set; }

        public MainViewModel()
        {
            ViewModels = new Dictionary<ViewModelName, ViewModelBase>
            {
                { ViewModelName.RailwayMonitor, new RailwayMonitorViewModel() }
            };

            SelectedViewModel = ViewModels.First().Value;
        }

        public void SelectView(ViewModelName viewModelName)
        {
            if (ViewModels.TryGetValue(viewModelName, out ViewModelBase selectedViewModel))
            {
                this.SelectedViewModel = selectedViewModel;
            }
        }
    }
}
