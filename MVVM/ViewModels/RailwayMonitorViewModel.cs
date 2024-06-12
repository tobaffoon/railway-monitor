using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.ToolButtons;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace railway_monitor.MVVM.ViewModels {
    public class RailwayMonitorViewModel : ViewModelBase {
        private void PreprocessButtonChecked() {
            RailwayCanvas.DeleteLatestGraphicItem();
        }

        private void AddPreprocessButtonChecked(IEnumerable<RadioButton> buttons) {
            foreach (RadioButton button in buttons) {
                button.Checked += (object sender, RoutedEventArgs e) => PreprocessButtonChecked();
            }
        }

        private void InitializeViewModels() {
            ToolButtons = new ToolButtonsViewModel();
            RailwayCanvas = new RailwayCanvasViewModel();
            AddPreprocessButtonChecked(ToolButtons.ToolButtonsList);
        }

        public static readonly DependencyProperty MoveCommandProperty =
            DependencyProperty.Register(
            "MoveCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand MoveCommand {
            get { return (UseToolCommand)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }

        public static readonly DependencyProperty LeftClickCommandProperty =
            DependencyProperty.Register(
            "LeftClickCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand LeftClickCommand {
            get { return (UseToolCommand)GetValue(LeftClickCommandProperty); }
            set { SetValue(LeftClickCommandProperty, value); }
        }

        public static readonly DependencyProperty RightClickCommandProperty =
            DependencyProperty.Register(
            "RightClickCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand RightClickCommand {
            get { return (UseToolCommand)GetValue(RightClickCommandProperty); }
            set { SetValue(RightClickCommandProperty, value); }
        }

        public static readonly DependencyProperty LeftReleaseCommandProperty =
            DependencyProperty.Register(
            "LeftReleaseCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand LeftReleaseCommand {
            get { return (UseToolCommand)GetValue(LeftReleaseCommandProperty); }
            set { SetValue(LeftReleaseCommandProperty, value); }
        }

        public static readonly DependencyProperty EscapeCommandProperty =
            DependencyProperty.Register(
            "EscapeCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand EscapeCommand {
            get { return (UseToolCommand)GetValue(EscapeCommandProperty); }
            set { SetValue(EscapeCommandProperty, value); }
        }

        public ToolButtonsViewModel ToolButtons { get; private set; }
        public RailwayCanvasViewModel RailwayCanvas { get; private set; }

        public RailwayMonitorViewModel() {
            InitializeViewModels();
            EscapeCommand = new UseToolCommand(KeyboardActions.RemoveLatestShape);

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
        }
    }
}
