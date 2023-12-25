using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using railway_monitor.Tools.DrawCommands;
using railway_monitor.Tools;

namespace railway_monitor.Components.ToolButtons
{
    public static class ToolButtonsListContainer
    {
        private const string ToolsGroupName = "Tools";
        #region ToolButtonsList property declaration
        private static readonly List<RadioButton> _toolButtonsList = new List<RadioButton>();
        public static List<RadioButton> ToolButtonsList => _toolButtonsList;
        #endregion

        public static CommandBase CurrentToolCommand { get; private set; }

        private static void ToolButtonChecked(DrawCommand newCurrentToolCommand, object sender, RoutedEventArgs e)
        {
            CurrentToolCommand = newCurrentToolCommand;
        }

        static ToolButtonsListContainer()
        {
            _toolButtonsList.Clear();

            RadioButton srtButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawStraightRailTrack, sender, e);

            RadioButton switchButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawSwitch, sender, e);

            RadioButton signalButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawSignal, sender, e);

            RadioButton deadEndButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawDeadend, sender, e);

            RadioButton externalTrackButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawExternalTrack, sender, e);

            _toolButtonsList.Add(srtButton);
            _toolButtonsList.Add(switchButton);
            _toolButtonsList.Add(signalButton);
            _toolButtonsList.Add(deadEndButton);
            _toolButtonsList.Add(externalTrackButton);

            srtButton.IsChecked = true;
        }
    }
}
