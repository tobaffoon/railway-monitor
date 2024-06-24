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
            
            _simulator = new RailwaySimulator(50);
        }

        internal void Start(TrainSchedule trainSchedule, int timeInaccuracy) {
            if (Graph == null) {
                return;
            }

            if(StationManager == null) {
                StationManager = new StationManager(RailwayCanvas, trainSchedule, timeInaccuracy, _simulator, Graph);
            }
            else {
                StationManager.Reset(trainSchedule, Graph);
            }
            StationManager.PropertyChanged += SetTime;
            _simulator.trainItems = StationManager.trainItems;
            _simulator.Start(StationManager.GetWorkPlan(), trainSchedule, new SimulatorUpdatesListener(StationManager), StationManager.TrainIdDict);
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

        /// <summary>
        /// Function to calculate next position for train in simulation
        /// </summary>
        /// <param name="train"></param>
        /// <param name="reactsToState"></param>
        /// <returns>
        /// Tuple of: track id, destination port id, progress on the track, bool for whether train is departed
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static Tuple<int, int, double, bool> GetAdvancedTrainPos(TrainItem train, bool reactsToState = true) {
            StraightRailTrackItem trainTrack = train.FlowCurrentTrack;
            Port dstPort = train.FlowEndingPort;
            double trackProgress = train.FlowTrackProgress;
            double advancedProgress = trackProgress + train.Speed / trainTrack.Length;
            if (advancedProgress < 1) {
                return Tuple.Create(trainTrack.Id, dstPort.Id, advancedProgress, false);
            }

            // At this point we might want to change track
            if (Port.IsPortSignal(dstPort)) {
                SignalItem signalItem = dstPort.TopologyItems.OfType<SignalItem>().First();
                if (signalItem.LightStatus == SignalItem.SignalLightStatus.STOP || !reactsToState) {
                    // stop if signal status is STOP. Or if caller doesn't want to react to station's state
                    return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress, false);
                }
                StraightRailTrackItem nextSrt = dstPort.TopologyItems.OfType<StraightRailTrackItem>().First(srt => srt != trainTrack);
                return Tuple.Create(nextSrt.Id, nextSrt.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress, false);
            }

            if (Port.IsPortSwitch(dstPort)) {
                if (!reactsToState) {
                    // stop if caller doesn't want to react to station's state
                    return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress, false);
                }

                SwitchItem switchItem = dstPort.TopologyItems.OfType<SwitchItem>().First();
                if (switchItem.Direction == SwitchItem.SwitchDirection.FIRST) {
                    StraightRailTrackItem dstSrt = switchItem.SrcTrack == train.FlowCurrentTrack ? switchItem.DstOneTrack : switchItem.SrcTrack;
                    return Tuple.Create(dstSrt.Id, dstSrt.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress, false);
                }
                else {
                    StraightRailTrackItem dstSrt = switchItem.SrcTrack == train.FlowCurrentTrack ? switchItem.DstTwoTrack : switchItem.SrcTrack;
                    return Tuple.Create(dstSrt.Id, dstSrt.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress, false);
                }
            }
            if (Port.IsPortConnection(dstPort)) {
                StraightRailTrackItem nextSrt = dstPort.TopologyItems.OfType<StraightRailTrackItem>().First(srt => srt != trainTrack);
                return Tuple.Create(nextSrt.Id, nextSrt.GetOtherPort(dstPort).Id, TrainItem.minDrawableProgress, false);
            }
            if (Port.IsPortOutput(dstPort)) {
                return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress, true);
            }
            if (Port.IsPortDeadend(dstPort)) {
                return Tuple.Create(trainTrack.Id, dstPort.Id, trackProgress, false);
            }

            throw new ArgumentException("Error while getting next position of a train that heads to " + dstPort + String.Join(", ", dstPort.TopologyItems));
        }
    }
}
