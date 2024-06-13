using railway_monitor.Components.RailwayCanvas;
using railway_monitor.MVVM.ViewModels;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Windows;
using System.Windows.Controls;

namespace railway_monitor.Components.ToolButtons {
    public class ToolButtonsViewModel : ViewModelBase {
        private const string ToolsGroupName = "Tools";

        public List<RadioButton> ToolButtonsList { get; private set; }

        public UseToolCommand LeftClickCommand { get; private set; }
        public CanvasCommand RightClickCommand { get; private set; }
        public UseToolCommand MoveCommand { get; private set; }
        public CanvasCommand LeftReleaseCommand { get; private set; }

        private void SetCommands(Action<Tuple<RailwayCanvasViewModel, Point>> leftClickAction,
            Action<Tuple<RailwayCanvasViewModel, Point>> moveAction, 
            Action<RailwayCanvasViewModel> rightClickAction,
            Action<RailwayCanvasViewModel> leftReleaseAction) {
            LeftClickCommand.ExecuteDelegate = leftClickAction;
            MoveCommand.ExecuteDelegate = moveAction;
            RightClickCommand.ExecuteDelegate = rightClickAction;
            LeftReleaseCommand.ExecuteDelegate = leftReleaseAction;
        }

        public ToolButtonsViewModel() {
            ToolButtonsList = new List<RadioButton>();

            LeftClickCommand = new UseToolCommand(LeftClickToolActions.PlaceStraightRailTrack);
            RightClickCommand = new CanvasCommand(UtilToolActions.NoAction);
            MoveCommand = new UseToolCommand(MoveToolActions.MoveStraightRailTrack);
            LeftReleaseCommand = new CanvasCommand(UtilToolActions.NoAction);

            RadioButton srtButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(LeftClickToolActions.PlaceStraightRailTrack, MoveToolActions.MoveStraightRailTrack, UtilToolActions.NoAction, UtilToolActions.NoAction);

            RadioButton switchButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(LeftClickToolActions.PlaceSwitch, MoveToolActions.MoveSwitch, UtilToolActions.NoAction, UtilToolActions.NoAction);

            RadioButton signalButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(LeftClickToolActions.PlaceSignal, MoveToolActions.MoveSignal, UtilToolActions.NoAction, UtilToolActions.NoAction);

            RadioButton deadEndButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(LeftClickToolActions.PlaceDeadend, MoveToolActions.MoveDeadend, UtilToolActions.NoAction, UtilToolActions.NoAction);

            RadioButton externalTrackButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(LeftClickToolActions.PlaceExternalTrack, MoveToolActions.MoveExternalTrack, RightClickToolActions.ToggleExternalTrackType, UtilToolActions.NoAction);

            RadioButton dragButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Drag",
            };
            dragButton.Checked += (object sender, RoutedEventArgs e) => SetCommands(LeftClickToolActions.CaptureDrag, MoveToolActions.MoveDrag, UtilToolActions.NoAction, LeftReleaseToolActions.ReleaseDrag);

            ToolButtonsList.AddRange([
                srtButton,
                switchButton,
                signalButton,
                deadEndButton,
                externalTrackButton,
                dragButton
                ]);

            srtButton.IsChecked = true;
        }
    }
}
