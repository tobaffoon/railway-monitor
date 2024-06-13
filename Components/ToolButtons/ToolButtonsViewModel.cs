using railway_monitor.Bases;
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
            srtButton.Checked += (object sender, RoutedEventArgs e) => LeftClickCommand.ExecuteDelegate = LeftClickToolActions.PlaceStraightRailTrack;
            srtButton.Checked += (object sender, RoutedEventArgs e) => RightClickCommand.ExecuteDelegate = UtilToolActions.NoAction;
            srtButton.Checked += (object sender, RoutedEventArgs e) => MoveCommand.ExecuteDelegate = MoveToolActions.MoveStraightRailTrack;
            srtButton.Checked += (object sender, RoutedEventArgs e) => LeftReleaseCommand.ExecuteDelegate = UtilToolActions.NoAction;

            RadioButton switchButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => LeftClickCommand.ExecuteDelegate = LeftClickToolActions.PlaceSwitch;
            switchButton.Checked += (object sender, RoutedEventArgs e) => RightClickCommand.ExecuteDelegate = UtilToolActions.NoAction;
            switchButton.Checked += (object sender, RoutedEventArgs e) => MoveCommand.ExecuteDelegate = MoveToolActions.MoveSwitch;
            switchButton.Checked += (object sender, RoutedEventArgs e) => LeftReleaseCommand.ExecuteDelegate = UtilToolActions.NoAction;

            RadioButton signalButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => LeftClickCommand.ExecuteDelegate = LeftClickToolActions.PlaceSignal;
            signalButton.Checked += (object sender, RoutedEventArgs e) => RightClickCommand.ExecuteDelegate = UtilToolActions.NoAction;
            signalButton.Checked += (object sender, RoutedEventArgs e) => MoveCommand.ExecuteDelegate = MoveToolActions.MoveSignal;
            signalButton.Checked += (object sender, RoutedEventArgs e) => LeftReleaseCommand.ExecuteDelegate = UtilToolActions.NoAction;

            RadioButton deadEndButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => LeftClickCommand.ExecuteDelegate = LeftClickToolActions.PlaceDeadend;
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => RightClickCommand.ExecuteDelegate = UtilToolActions.NoAction;
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => MoveCommand.ExecuteDelegate = MoveToolActions.MoveDeadend;
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => LeftReleaseCommand.ExecuteDelegate = UtilToolActions.NoAction;

            RadioButton externalTrackButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => LeftClickCommand.ExecuteDelegate = LeftClickToolActions.PlaceExternalTrack;
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => RightClickCommand.ExecuteDelegate = RightClickToolActions.ToggleExternalTrackType;
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => MoveCommand.ExecuteDelegate = MoveToolActions.MoveExternalTrack;
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => LeftReleaseCommand.ExecuteDelegate = UtilToolActions.NoAction;

            RadioButton dragButton = new RadioButton {
                GroupName = ToolsGroupName,
                Content = "Drag",
            };
            dragButton.Checked += (object sender, RoutedEventArgs e) => LeftClickCommand.ExecuteDelegate = LeftClickToolActions.CaptureDrag;
            dragButton.Checked += (object sender, RoutedEventArgs e) => RightClickCommand.ExecuteDelegate = UtilToolActions.NoAction;
            dragButton.Checked += (object sender, RoutedEventArgs e) => MoveCommand.ExecuteDelegate = MoveToolActions.MoveDrag;
            dragButton.Checked += (object sender, RoutedEventArgs e) => LeftReleaseCommand.ExecuteDelegate = LeftReleaseToolActions.ReleaseDrag;

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
