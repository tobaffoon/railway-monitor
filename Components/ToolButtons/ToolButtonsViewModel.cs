using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Windows.Media;
using railway_monitor.MVVM.ViewModels;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Components.ToolButtons
{
    public class ToolButtonsViewModel : ViewModelBase
    {
        private const string ToolsGroupName = "Tools";

        public static List<RadioButton> ToolButtonsList { get; } = new List<RadioButton>();

        public UseToolCommand ClickCommand { get; private set; }
        public UseToolCommand MoveCommand { get; private set; }

        private void ToolButtonChecked(Action<Tuple<RailwayCanvasViewModel, Point>> newClickFunc, Action<Tuple<RailwayCanvasViewModel, Point>> newMoveFunc)
        {
            ClickCommand.ExecuteDelegate = newClickFunc;
            MoveCommand.ExecuteDelegate = newMoveFunc;
        }

        public ToolButtonsViewModel()
        {
            ToolButtonsList.Clear();

            ClickCommand = new UseToolCommand(ClickToolActions.PlaceStraightRailTrack);
            MoveCommand = new UseToolCommand(MoveToolActions.MoveStraightRailTrack);

            RadioButton srtButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceStraightRailTrack, MoveToolActions.MoveStraightRailTrack);

            RadioButton switchButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceSwitch, MoveToolActions.MoveSwitch);

            RadioButton signalButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceSignal, MoveToolActions.MoveSignal);

            RadioButton deadEndButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceDeadend, MoveToolActions.MoveDeadend);

            RadioButton externalTrackButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceExternalTrack, MoveToolActions.MoveDeadend);

            ToolButtonsList.Add(srtButton);
            ToolButtonsList.Add(switchButton);
            ToolButtonsList.Add(signalButton);
            ToolButtonsList.Add(deadEndButton);
            ToolButtonsList.Add(externalTrackButton);

            //srtButton.IsChecked = true;
            switchButton.IsChecked = true;
        }
    }
}
