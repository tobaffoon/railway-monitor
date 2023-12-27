using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using railway_monitor.Components.RailwayCanvas;
using System.Windows.Media;

namespace railway_monitor.Components.ToolButtons
{
    class ToolButtonsViewModel : DependencyObject
    {
        private static readonly Brush RailTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
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
        private readonly AddGraphicsItemCommand _clickCommand;
        public AddGraphicsItemCommand ClickCommand => _clickCommand;

        private readonly MoveGraphicItemCommand _moveCommand;
        public MoveGraphicItemCommand MoveCommand => _moveCommand;

        private Shape? _latestShape = null;

        public Shape? LatestShape
        {
            get => _latestShape;
            set => _latestShape = value;
        }

        private void ToolButtonChecked(Action<Tuple<Canvas, Shape>> newClickFunc, Action<Tuple<Canvas, Shape>> newMoveFunc, object sender, RoutedEventArgs e)
        {
            if (ClickCommand.ExecuteDelegate != null && ClickCommand.ExecuteDelegate == ClickToolActions.StartStraightRailTrack)
            {
                SwitchToFinishRailTrackCommands();
            }
            else
            {
                ClickCommand.ExecuteDelegate = newClickFunc;
                MoveCommand.ExecuteDelegate = newMoveFunc;
            }

        }

        public void SwitchToStartRailTrackCommands()
        {
            _clickCommand.ExecuteDelegate = ClickToolActions.StartStraightRailTrack;
            _clickCommand.ExecutedHandler = SwitchToFinishRailTrackCommands;
            _moveCommand.ExecuteDelegate = MoveToolActions.MoveStraightRailTrackStart;
            LatestShape = new StraightRailTrack
            {
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = 0,
                Stroke = RailTrackBrush,
                StrokeThickness = 5
            };
        }
        public void SwitchToFinishRailTrackCommands()
        {
            _clickCommand.ExecuteDelegate = ClickToolActions.FinishStraightRailTrack;
            _clickCommand.ExecutedHandler = SwitchToStartRailTrackCommands;
            _moveCommand.ExecuteDelegate = MoveToolActions.MoveStraightRailTrackFinish;
        }

        public ToolButtonsViewModel()
        {
            _toolButtonsList.Clear();
            LatestShape = new StraightRailTrack
            {
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = 0,
                Stroke = RailTrackBrush,
                StrokeThickness = 5
            };

            _clickCommand = RailwayDesignerCommands.AddItem;
            _clickCommand.ShapeGetter = () => _latestShape;
            _clickCommand.ExecutedHandler = SwitchToFinishRailTrackCommands;

            _moveCommand = RailwayDesignerCommands.MoveItem;
            _moveCommand.ShapeGetter = () => _latestShape;

            RadioButton srtButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "SRT",
            };
            srtButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.StartStraightRailTrack, MoveToolActions.MoveStraightRailTrackStart, sender, e);

            RadioButton switchButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Switch",
            };
            switchButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceSwitch, MoveToolActions.MoveSwitch, sender, e);

            RadioButton signalButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Signal",
            };
            signalButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceSignal, MoveToolActions.MoveSignal, sender, e);

            RadioButton deadEndButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "Dead-end",
            };
            deadEndButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceDeadend, MoveToolActions.MoveDeadend, sender, e);

            RadioButton externalTrackButton = new RadioButton
            {
                GroupName = ToolsGroupName,
                Content = "External Track",
            };
            externalTrackButton.Checked += (object sender, RoutedEventArgs e) => ToolButtonChecked(ClickToolActions.PlaceExternalTrack, MoveToolActions.MoveExternalTrack, sender, e);

            _toolButtonsList.Add(srtButton);
            _toolButtonsList.Add(switchButton);
            _toolButtonsList.Add(signalButton);
            _toolButtonsList.Add(deadEndButton);
            _toolButtonsList.Add(externalTrackButton);

            srtButton.IsChecked = true;
        }
    }
}
