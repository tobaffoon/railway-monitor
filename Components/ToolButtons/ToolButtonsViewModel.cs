using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.MVVM.ViewModels;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace railway_monitor.Components.ToolButtons
{
    public class ToolButtonsViewModel : ViewModelBase {
        private const string ToolsGroupName = "Tools";

        public List<RadioButton> ToolButtonsList { get; private set; }

        public UseToolCommand LeftClickCommand { get; private set; }
        public CanvasCommand RightClickCommand { get; private set; }
        public UseToolCommand MoveCommand { get; private set; }
        public CanvasCommand LeftReleaseCommand { get; private set; }
        public WheelCommand WheelCommand { get; private set; }
        public KeyboardCommand ArrowsCommand { get; private set; }

        private void SetCommands(Action<Tuple<RailwayCanvasViewModel, Point>> leftClickAction,
            Action<Tuple<RailwayCanvasViewModel, Point>> moveAction, 
            Action<RailwayCanvasViewModel> rightClickAction,
            Action<RailwayCanvasViewModel> leftReleaseAction,
            Action<Tuple<RailwayCanvasViewModel, bool>> wheelAction,
            Action<Tuple<RailwayCanvasViewModel, Key>> arrowsAction) {
            LeftClickCommand.ExecuteDelegate = leftClickAction;
            MoveCommand.ExecuteDelegate = moveAction;
            RightClickCommand.ExecuteDelegate = rightClickAction;
            LeftReleaseCommand.ExecuteDelegate = leftReleaseAction;
            WheelCommand.ExecuteDelegate = wheelAction;
            ArrowsCommand.ExecuteDelegate = arrowsAction;
        }

        public ToolButtonsViewModel() {
            ToolButtonsList = new List<RadioButton>();

            LeftClickCommand = new UseToolCommand(LeftClickToolActions.PlaceStraightRailTrack);
            RightClickCommand = new CanvasCommand(UtilToolActions.NoCanvasAction);
            MoveCommand = new UseToolCommand(MoveToolActions.MoveStraightRailTrack);
            LeftReleaseCommand = new CanvasCommand(UtilToolActions.NoCanvasAction);
            WheelCommand = new WheelCommand(UtilToolActions.NoWheelAction);
            ArrowsCommand = new KeyboardCommand(UtilToolActions.NoKeyboardAction);

            RadioButton srtButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.PlaceStraightRailTrack, 
                MoveToolActions.MoveStraightRailTrack, 
                RightClickToolActions.ChangeRailDirection, 
                UtilToolActions.NoCanvasAction,
                UtilToolActions.NoWheelAction,
                UtilToolActions.NoKeyboardAction);

            RadioButton platformButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Platform",
            };
            platformButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.AddPlatform,
                MoveToolActions.MovePlatform,
                RightClickToolActions.ScrollMiniPlatformType,
                UtilToolActions.NoCanvasAction,
                UtilToolActions.NoWheelAction,
                UtilToolActions.NoKeyboardAction);

            RadioButton switchButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };

            switchButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.PlaceSwitch, 
                MoveToolActions.MoveSwitch, 
                UtilToolActions.NoCanvasAction, 
                UtilToolActions.NoCanvasAction,
                UtilToolActions.NoWheelAction,
                UtilToolActions.NoKeyboardAction);

            RadioButton signalButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.PlaceSignal, 
                MoveToolActions.MoveSignal, 
                UtilToolActions.NoCanvasAction, 
                UtilToolActions.NoCanvasAction,
                UtilToolActions.NoWheelAction,
                UtilToolActions.NoKeyboardAction);

            RadioButton deadEndButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.PlaceDeadend, 
                MoveToolActions.MoveDeadend, 
                UtilToolActions.NoCanvasAction, 
                UtilToolActions.NoCanvasAction,
                WheelToolActions.RotateDeadend,
                KeyboardActions.RotateDeadend);

            RadioButton externalTrackButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.PlaceExternalTrack, 
                MoveToolActions.MoveExternalTrack, 
                RightClickToolActions.ToggleExternalTrackType,
                UtilToolActions.NoCanvasAction,
                WheelToolActions.RotateExternalTrack,
                KeyboardActions.RotateExternalTrack);

            RadioButton dragButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Drag",
            };
            dragButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.CaptureDrag, 
                MoveToolActions.MoveDrag, 
                UtilToolActions.NoCanvasAction, 
                LeftReleaseToolActions.ReleaseDrag,
                UtilToolActions.NoWheelAction,
                UtilToolActions.NoKeyboardAction);

            RadioButton testButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Test",
            };
            testButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(
                LeftClickToolActions.TestActions, 
                UtilToolActions.NoMouseAction, 
                UtilToolActions.NoCanvasAction,
                UtilToolActions.NoCanvasAction,
                UtilToolActions.NoWheelAction,
                UtilToolActions.NoKeyboardAction);

            ToolButtonsList.AddRange([
                srtButton,
                platformButton,
                switchButton,
                signalButton,
                deadEndButton,
                externalTrackButton,
                dragButton,
                testButton
                ]);

            srtButton.IsChecked = true;
        }
    }
}
