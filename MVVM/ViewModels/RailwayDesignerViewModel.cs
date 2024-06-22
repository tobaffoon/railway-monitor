using railway_monitor.Components.ToolButtons;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace railway_monitor.MVVM.ViewModels {
    public class RailwayDesignerViewModel : RailwayBaseViewModel {
        private void PreprocessButtonChecked() {
            RailwayCanvas.DeleteLatestTopologyItem();
        }

        private void AddPreprocessButtonChecked(IEnumerable<RadioButton> buttons) {
            foreach (RadioButton button in buttons) {
                button.Checked += (object sender, RoutedEventArgs e) => PreprocessButtonChecked();
            }
        }

        public ToolButtonsViewModel ToolButtons { get; private set; }


        public RailwayDesignerViewModel(MainViewModel mainViewModel) : base (mainViewModel){
            #region Initialize ViewModels
            ToolButtons = new ToolButtonsViewModel();
            AddPreprocessButtonChecked(ToolButtons.ToolButtonsList);
            #endregion
            #region Bind commands
            CanvasKeyboardCommand = new KeyboardCommand(KeyboardActions.CanvasKeyDown);

            var leftClickBinding = new Binding("LeftClickCommand") {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, LeftClickCommandProperty, leftClickBinding);
            var rightClickBinding = new Binding("RightClickCommand") {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, RightClickCommandProperty, rightClickBinding);
            var moveBinding = new Binding("MoveCommand") {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, MoveCommandProperty, moveBinding);
            var leftReleaseBinding = new Binding("LeftReleaseCommand") {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, LeftReleaseCommandProperty, leftReleaseBinding);
            var wheelBinding = new Binding("WheelCommand") {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, WheelCommandProperty, wheelBinding);
            var arrowsBinding = new Binding("ArrowsCommand") {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, ArrowsCommandProperty, arrowsBinding);
            #endregion
        }

        internal void FinishDesigning() {
            mainViewModel.SelectView(MainViewModel.ViewModelName.RailwayDesigner);
            if (mainViewModel.SelectedViewModel is not RailwayDesignerViewModel monitor) {
                mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
                return;
            }
            monitor.RailwayCanvas.Clear();

            mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
        }
    }
}
