using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Tools;
using System.Windows;

namespace railway_monitor.MVVM.ViewModels {
    public class RailwayBaseViewModel : ViewModelBase {
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

        public RailwayCanvasViewModel RailwayCanvas { get; private set; }
        protected MainViewModel mainViewModel;

        public RailwayBaseViewModel(MainViewModel mainViewModel) {
            RailwayCanvas = new RailwayCanvasViewModel();
            this.mainViewModel = mainViewModel;
        }
    }
}
