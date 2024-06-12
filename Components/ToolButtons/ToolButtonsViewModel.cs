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
        public UseToolCommand MoveCommand { get; private set; }
        public UseToolCommand ReleaseCommand { get; private set; }

        private void ToolButtonChecked(Action<Tuple<RailwayCanvasViewModel, Point>> newClickFunc,
            Action<Tuple<RailwayCanvasViewModel, Point>> newMoveFunc) {
            LeftClickCommand.ExecuteDelegate = newClickFunc;
            MoveCommand.ExecuteDelegate = newMoveFunc;
            ReleaseCommand.ExecuteDelegate = UtilToolActions.NoAction;
        }
        private void ToolButtonChecked(Action<Tuple<RailwayCanvasViewModel, Point>> newClickFunc,
            Action<Tuple<RailwayCanvasViewModel, Point>> newMoveFunc,
            Action<Tuple<RailwayCanvasViewModel, Point>> newReleaseFunc) {
            LeftClickCommand.ExecuteDelegate = newClickFunc;
            MoveCommand.ExecuteDelegate = newMoveFunc;
            ReleaseCommand.ExecuteDelegate = newReleaseFunc;
        }

        public ToolButtonsViewModel() {
            ToolButtonsList = new List<RadioButton>();

            LeftClickCommand = new UseToolCommand(LeftClickToolActions.PlaceStraightRailTrack);
            MoveCommand = new UseToolCommand(MoveToolActions.MoveStraightRailTrack);
            ReleaseCommand = new UseToolCommand(UtilToolActions.NoAction);

            RadioButton srtButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(LeftClickToolActions.PlaceStraightRailTrack, MoveToolActions.MoveStraightRailTrack);

            RadioButton switchButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(LeftClickToolActions.PlaceSwitch, MoveToolActions.MoveSwitch);

            RadioButton signalButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(LeftClickToolActions.PlaceSignal, MoveToolActions.MoveSignal);

            RadioButton deadEndButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(LeftClickToolActions.PlaceDeadend, MoveToolActions.MoveDeadend);

            RadioButton externalTrackButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(LeftClickToolActions.PlaceExternalTrack, MoveToolActions.MoveExternalTrack);

            RadioButton dragButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Drag",
            };
            dragButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(LeftClickToolActions.CaptureDrag, MoveToolActions.MoveDrag, ReleaseToolActions.ReleaseDrag);

            ToolButtonsList.Add(srtButton);
            ToolButtonsList.Add(switchButton);
            ToolButtonsList.Add(signalButton);
            ToolButtonsList.Add(deadEndButton);
            ToolButtonsList.Add(externalTrackButton);
            ToolButtonsList.Add(dragButton);

            srtButton.IsChecked = true;
        }
    }
}
