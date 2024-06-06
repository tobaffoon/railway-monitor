using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.ToolButtons;
using railway_monitor.Tools;
using railway_monitor.Tools.Actions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace railway_monitor.MVVM.ViewModels
{
    public class RailwayMonitorViewModel : ViewModelBase
    {
        //private static void PreprocessButtonChecked(RailwayCanvasViewModel canvas)
        //{
        //    canvas.DeleteLatestShape();
        //}

        //private void AddPreprocessButtonChecked(IEnumerable<RadioButton> buttons)
        //{
        //    RailwayCanvasViewModel canvas = ((RailwayMonitorViewModel)DataContext).RailwayCanvas;
        //    foreach (RadioButton button in buttons)
        //    {
        //        button.Checked += (object sender, RoutedEventArgs e) => PreprocessButtonChecked(canvas);
        //    }
        //}

        //private void InitializeViewModels()
        //{
        //    AddPreprocessButtonChecked(this.ToolButtons.ToolButtonsList);
        //}

        public static readonly DependencyProperty MoveCommandProperty =
            DependencyProperty.Register(
            "MoveCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand MoveCommand
        {
            get { return (UseToolCommand)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandProperty = 
            DependencyProperty.Register(
            "ClickCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand ClickCommand
        {
            get { return (UseToolCommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly DependencyProperty EscapeCommandProperty = 
            DependencyProperty.Register(
            "EscapeCommand", typeof(UseToolCommand),
            typeof(RailwayMonitorViewModel));
        public UseToolCommand EscapeCommand
        {
            get { return (UseToolCommand)GetValue(EscapeCommandProperty); }
            set { SetValue(EscapeCommandProperty, value); }
        }

        public ToolButtonsViewModel ToolButtons { get; }
        public RailwayCanvasViewModel RailwayCanvas { get; }

        public RailwayMonitorViewModel()
        {
            ToolButtons = new ToolButtonsViewModel();
            RailwayCanvas = new RailwayCanvasViewModel();
            EscapeCommand = new UseToolCommand(KeyboardActions.RemoveLatestShape);
            
            var clickBinding = new Binding("ClickCommand")
            {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, ClickCommandProperty, clickBinding);
            var moveBinding = new Binding("MoveCommand")
            {
                Source = ToolButtons
            };
            BindingOperations.SetBinding(this, MoveCommandProperty, moveBinding);
        }
    }
}
