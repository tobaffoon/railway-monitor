using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.ToolButtons;
using railway_monitor.MVVM.Models.Station;
using railway_monitor.Tools.Actions;
using railway_monitor.Tools;
using SolverLibrary.Model;

namespace railway_monitor.MVVM.ViewModels {
    public class RailwayMonitorViewModel : RailwayBaseViewModel { 
        private static readonly int _defaultTimeInaccuracy = 5;
        public StationManager? StationManager { get; private set; }
        public string CurrentTime {
            get {
                if (StationManager == null) {
                    return "0 s";
                }
                else {
                    return StationManager.CurrentTime + " s";
                }
            }
        }

        public RailwayMonitorViewModel(MainViewModel mainViewModel) : base(mainViewModel) {
            CanvasKeyboardCommand = new KeyboardCommand(UtilToolActions.NoKeyboardAction);
            RightClickCommand = new CanvasCommand(UtilToolActions.NoCanvasAction); 
            LeftClickCommand = new UseToolCommand(LeftClickToolActions.CaptureDrag);
            MoveCommand = new UseToolCommand(MoveToolActions.MoveDrag);
            LeftReleaseCommand = new CanvasCommand(LeftReleaseToolActions.ReleaseDrag);
            WheelCommand = new WheelCommand(UtilToolActions.NoWheelAction);
            ArrowsCommand = new KeyboardCommand(UtilToolActions.NoKeyboardAction);
        }

        internal void Start(TrainSchedule trainSchedule, int timeInaccuracy) {
            StationManager = new StationManager(RailwayCanvas, trainSchedule, timeInaccuracy);
        }

        internal void FinishMonitoring() {
            if (mainViewModel.SelectedViewModel is not RailwayMonitorViewModel monitor) {
                mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
                return;
            }
            monitor.RailwayCanvas.Clear();

            mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
        }
    }
}
