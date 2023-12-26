using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using railway_monitor.Tools;

namespace railway_monitor.Components.ToolButtons
{
    class ToolButtonsViewModel : DependencyObject
    {
        private const string ToolsGroupName = "Tools";
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

        #region GraphicItemsList property declaration

        public static readonly DependencyProperty GraphicItemsListProperty = DependencyProperty.Register(
            "GraphicItemList",
            typeof(List<Shape>),
            typeof(ToolButtonsViewModel)
            );

        private readonly List<Shape> _graphicItemList = new List<Shape>();
        public List<Shape> GraphicItemList => (List<Shape>)GetValue(GraphicItemsListProperty);

        #endregion
        private readonly UseToolCommand _toolCommand;

        public UseToolCommand ToolCommand => _toolCommand;

        private void ToolButtonChecked(Action<Canvas> newCurrentToolAction, object sender, RoutedEventArgs e)
        {
            _toolCommand.ExecuteDelegate = newCurrentToolAction;
        }

        public ToolButtonsViewModel()
        {
            _toolButtonsList.Clear();
            _toolCommand = new UseToolCommand();

            RadioButton srtButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolActions.DrawStraightRailTrack, sender, e);

            RadioButton switchButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolActions.DrawSwitch, sender, e);

            RadioButton signalButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolActions.DrawSignal, sender, e);

            RadioButton deadEndButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolActions.DrawDeadend, sender, e);

            RadioButton externalTrackButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(DrawToolActions.DrawExternalTrack, sender, e);

            _toolButtonsList.Add(srtButton);
            _toolButtonsList.Add(switchButton);
            _toolButtonsList.Add(signalButton);
            _toolButtonsList.Add(deadEndButton);
            _toolButtonsList.Add(externalTrackButton);

            srtButton.IsChecked = true;
        }
    }
}
