using railway_monitor.MVVM.Models.Station;
using railway_monitor.MVVM.Models.Server;
using railway_monitor.Tools.Actions;
using railway_monitor.Tools;
using SolverLibrary.Model;
using railway_monitor.Simulator;
using SolverLibrary.Model.Graph;
using System.ComponentModel;
using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.TopologyItems;

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
            _simulator.trainItems = StationManager.trainItems;
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

        public static Tuple<int, int, double> GetAdvancedTrainPos(TrainItem train, double speed, double millis, bool reactsToState = true) {
            StraightRailTrackItem trainTrack = train.FlowCurrentTrack;
            Port dstPort = train.FlowEndingPort;
            double trackProgress = train.FlowTrackProgress;
            double advancedProgress = trackProgress + speed / trainTrack.Length * millis / 1000;
            if (advancedProgress < 1) {
                return Tuple.Create(trainTrack.Id, dstPort.Id, advancedProgress);
            }

            // At this point we might want to change track
            if (Port.IsPortSignal(dstPort)) {
                SignalItem signalItem = dstPort.TopologyItems.OfType<SignalItem>().First();
                if (signalItem.LightStatus == SignalItem.SignalLightStatus.STOP || !reactsToState) {
                    // stop if signal status is STOP. Or if caller doesn't want to react to station's state
                    return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress);
                }
                StraightRailTrackItem nextSrt = dstPort.TopologyItems.OfType<StraightRailTrackItem>().First(srt => srt != trainTrack);
                return Tuple.Create(nextSrt.Id, nextSrt.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress);
            }

            if (Port.IsPortSwitch(dstPort)) {
                if (!reactsToState) {
                    // stop if caller doesn't want to react to station's state
                    return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress);
                }

                SwitchItem switchItem = dstPort.TopologyItems.OfType<SwitchItem>().First();
                if (switchItem.Direction == SwitchItem.SwitchDirection.FIRST) {
                    return Tuple.Create(switchItem.DstOneTrack.Id, switchItem.DstOneTrack.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress);
                }
                else {
                    return Tuple.Create(switchItem.DstTwoTrack.Id, switchItem.DstTwoTrack.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress);
                }
            }
            if (Port.IsPortConnection(dstPort)) {
                StraightRailTrackItem nextSrt = dstPort.TopologyItems.OfType<StraightRailTrackItem>().First(srt => srt != trainTrack);
                return Tuple.Create(nextSrt.Id, nextSrt.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress);
            }
            if (Port.IsPortOutput(dstPort)) {
                // TODO: send train departure package
                return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress);
            }
            if (Port.IsPortDeadend(dstPort)) {
                return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress);
            }
            if (Port.IsPortInput(dstPort)) {
                return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress);
            }

            throw new ArgumentException("Error while getting next position of a train that heads to " + dstPort + String.Join(", ", dstPort.TopologyItems));
        }
    }
}
