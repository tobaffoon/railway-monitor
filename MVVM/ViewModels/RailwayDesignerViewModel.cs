using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.ToolButtons;
using railway_monitor.MVVM.Models.Station;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace railway_monitor.MVVM.ViewModels {
    public class RailwayDesignerViewModel : ViewModelBase {
        private static readonly int _defaultTimeInaccuracy = 5;
        private void PreprocessButtonChecked() {
            RailwayCanvas.DeleteLatestTopologyItem();
        }

        private void AddPreprocessButtonChecked(IEnumerable<RadioButton> buttons) {
            foreach (RadioButton button in buttons) {
                button.Checked += (object sender, RoutedEventArgs e) => PreprocessButtonChecked();
            }
        }

        public static readonly DependencyProperty MoveCommandProperty =
            DependencyProperty.Register(
            "MoveCommand", typeof(UseToolCommand),
            typeof(RailwayDesignerViewModel));
        public UseToolCommand MoveCommand {
            get { return (UseToolCommand)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }

        public static readonly DependencyProperty LeftClickCommandProperty =
            DependencyProperty.Register(
            "LeftClickCommand", typeof(UseToolCommand),
            typeof(RailwayDesignerViewModel));
        public UseToolCommand LeftClickCommand {
            get { return (UseToolCommand)GetValue(LeftClickCommandProperty); }
            set { SetValue(LeftClickCommandProperty, value); }
        }

        public static readonly DependencyProperty RightClickCommandProperty =
            DependencyProperty.Register(
            "RightClickCommand", typeof(CanvasCommand),
            typeof(RailwayDesignerViewModel));
        public CanvasCommand RightClickCommand {
            get { return (CanvasCommand)GetValue(RightClickCommandProperty); }
            set { SetValue(RightClickCommandProperty, value); }
        }

        public static readonly DependencyProperty LeftReleaseCommandProperty =
            DependencyProperty.Register(
            "LeftReleaseCommand", typeof(CanvasCommand),
            typeof(RailwayDesignerViewModel));
        public CanvasCommand LeftReleaseCommand {
            get { return (CanvasCommand)GetValue(LeftReleaseCommandProperty); }
            set { SetValue(LeftReleaseCommandProperty, value); }
        }


        public static readonly DependencyProperty WheelCommandProperty =
            DependencyProperty.Register(
            "WheelCommand", typeof(WheelCommand),
            typeof(RailwayDesignerViewModel));
        public WheelCommand WheelCommand {
            get { return (WheelCommand)GetValue(WheelCommandProperty); }
            set { SetValue(WheelCommandProperty, value); }
        }

        public static readonly DependencyProperty ArrowsCommandProperty =
            DependencyProperty.Register(
            "ArrowsCommand", typeof(KeyboardCommand),
            typeof(RailwayDesignerViewModel));
        public KeyboardCommand ArrowsCommand {
            get { return (KeyboardCommand)GetValue(ArrowsCommandProperty); }
            set { SetValue(ArrowsCommandProperty, value); }
        }

        public static readonly DependencyProperty CanvasKeyboardProperty =
            DependencyProperty.Register(
            "EscapeCommand", typeof(KeyboardCommand),
            typeof(RailwayDesignerViewModel));
        public KeyboardCommand CanvasKeyboardCommand {
            get { return (KeyboardCommand)GetValue(CanvasKeyboardProperty); }
            set { SetValue(CanvasKeyboardProperty, value); }
        }

        public ToolButtonsViewModel ToolButtons { get; private set; }
        public RailwayCanvasViewModel RailwayCanvas { get; private set; }
        public StationManager StationManager { get; private set; }

        private MainViewModel _mainViewModel;

        public RailwayDesignerViewModel(MainViewModel mainViewModel) {
            #region Initialize ViewModels
            _mainViewModel = mainViewModel;
            ToolButtons = new ToolButtonsViewModel();
            RailwayCanvas = new RailwayCanvasViewModel();
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
            _mainViewModel.SelectView(MainViewModel.ViewModelName.RailwayMonitor);
            if (_mainViewModel.SelectedViewModel is not RailwayDesignerViewModel monitor) {
                _mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
                return;
            }
            monitor.RailwayCanvas.Clear();

            _mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
        }
    }
}
