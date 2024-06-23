using railway_monitor.MVVM.Models.Station;
using railway_monitor.MVVM.Models.Server;
using railway_monitor.Tools.Actions;
using railway_monitor.Tools;
using SolverLibrary.Model;
using railway_monitor.Simulator;
using SolverLibrary.Model.Graph;
using System.ComponentModel;

namespace railway_monitor.MVVM.ViewModels {
    public class RailwayMonitorViewModel : RailwayBaseViewModel { 
        private static readonly int _defaultTimeInaccuracy = 5;
        public StationManager? StationManager { get; private set; }

        private RailwaySimulator _simulator;
        public StationGraph Graph { get; set; }

        private int _currentTime;
        public string CurrentTime {
            get {
                if (StationManager == null) {
                    return "0 s";
                }
                else {
                    return _currentTime + " s";
                }
            }
            set {
                SetField(ref _currentTime, int.Parse(value));
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
            
            _simulator = new RailwaySimulator();
        }

        internal void Start(TrainSchedule trainSchedule, int timeInaccuracy) {
            if (Graph == null) {
                return;
            }
            StationManager = new StationManager(RailwayCanvas, trainSchedule, timeInaccuracy, _simulator, Graph);
            StationManager.PropertyChanged += SetTime;
            _simulator.Start(StationManager.GetWorkPlan(), trainSchedule, new SimulatorUpdatesListener(StationManager), StationManager.trainIdDict);
        }
        internal void Start(TrainSchedule trainSchedule) {
            Start(trainSchedule, _defaultTimeInaccuracy);
        }

        internal void FinishMonitoring() {
            if (mainViewModel.SelectedViewModel is not RailwayMonitorViewModel monitor) {
                mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
                return;
            }
            monitor.RailwayCanvas.Clear();
            _simulator.Stop();
            _currentTime = 0;

            mainViewModel.SelectView(MainViewModel.ViewModelName.Start);
        }

        private void SetTime(object? sender, PropertyChangedEventArgs args) {
            if (args.PropertyName == null || args.PropertyName != "CurrentTime") {
                return;
            }

            CurrentTime = StationManager.CurrentTime.ToString();
        }
    }
}
