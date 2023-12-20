using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using railway_monitor.Tools;
using railway_monitor.Tools.DrawCommands;

namespace railway_monitor.Components.ToolButtons
{
    class ToolButtonsViewModel : DependencyObject
    {
        #region ToolButtonsList property declaration

        private static readonly DependencyPropertyKey ToolButtonsListPropertyKey;
        public static readonly DependencyProperty ToolButtonsListProperty;
        private static readonly List<RadioButton> _toolButtonsList = new List<RadioButton>();
        static ToolButtonsViewModel()
        {
            ToolButtonsListPropertyKey =
                DependencyProperty.RegisterReadOnly(
                    "ToolButtonsList",
                    typeof(List<RadioButton>),
                    typeof(ToolButtonsViewModel),
                    new FrameworkPropertyMetadata(_toolButtonsList)
                );

            ToolButtonsListProperty = ToolButtonsListPropertyKey.DependencyProperty;
        }

        public List<RadioButton> ToolButtonsList => (List<RadioButton>)GetValue(ToolButtonsListProperty);

        #endregion

        #region CurrentTool property declaration

        public static readonly DependencyProperty CurrentToolCommandProperty = DependencyProperty.Register(
            "CurrentToolCommandProperty",
            typeof(CommandBase),
            typeof(ToolButtonsViewModel),
            new PropertyMetadata(DrawToolCommands.DrawStraightRailTrack)
        );
        public CommandBase CurrentToolCommand
        {
            get => (CommandBase)GetValue(CurrentToolCommandProperty);
            set => SetValue(CurrentToolCommandProperty, value);
        }

        #endregion


        private void ToolButtonChecked(CommandBase newCurrentToolCommand, object sender, RoutedEventArgs e)
        {
            CurrentToolCommand = newCurrentToolCommand;
        }

        public ToolButtonsViewModel()
        {
            _toolButtonsList.Clear();

            RadioButton srtButton = new RadioButton
            {
                GroupName = "Tools",
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawStraightRailTrack, sender, e);

            RadioButton switchButton = new RadioButton
            {
                GroupName = "Tools",
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.AddSwitch, sender, e);

            RadioButton signalButton = new RadioButton
            {
                GroupName = "Tools",
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawSignal, sender, e);

            RadioButton deadEndButton = new RadioButton
            {
                GroupName = "Tools",
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawDeadend, sender, e);

            RadioButton externalTrackButton = new RadioButton
            {
                GroupName = "Tools",
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolCommands.DrawExternalTrack, sender, e);

            _toolButtonsList.Add(srtButton);
            _toolButtonsList.Add(switchButton);
            _toolButtonsList.Add(signalButton);
            _toolButtonsList.Add(deadEndButton);
            _toolButtonsList.Add(externalTrackButton);
        }
    }
}
