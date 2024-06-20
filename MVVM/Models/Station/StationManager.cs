using railway_monitor.Bases;
using railway_monitor.Components.TopologyItems;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.MVVM.Models.UpdatePackages;
using railway_monitor.Utils;
using SolverLibrary.Model.Graph;

namespace railway_monitor.MVVM.Models.Station
{
    public class StationManager
    {
        // the maximum number of trains that will be drawn with inbetween states (between train update packages)
        #region Confidence and flow timers
        private static readonly int maxTrainFlowRenderNumber = 100;
        private static readonly int confidenceInterval = 30000;
        private static readonly int flowUpdatesPerSec = 24;
        private static readonly int flowUpdateInterval = 1000 / flowUpdatesPerSec;

        // id -> confidence one-shot timer
        private Dictionary<int, TrainTimer> confidenceTimers;
        // id -> flow periodic timer
        private Dictionary<int, TrainTimer> flowTimers;
        #endregion

        private RailwayCanvasViewModel canvas;
        private StationGraph stationGraph;
        private Dictionary<int, TopologyItem> topologyVertexDict;
        private Dictionary<int, StraightRailTrackItem> topologyEdgeDict;
        private Dictionary<int, TrainItem> trains;

        public StationManager(RailwayCanvasViewModel canvas)
        {
            // TODO: smth-smth that takes schedule and remembers it
            int[] trainIds = { };
            // add timers for each train
            confidenceTimers = new Dictionary<int, TrainTimer>();
            flowTimers = new Dictionary<int, TrainTimer>();
            TrainTimer confidenceTimer, flowTimer;
            foreach (int id in trainIds) {
                confidenceTimer = new TrainTimer(id, confidenceInterval);
                confidenceTimer.Elapsed += OnConfidenceTimerElapsed;
                flowTimer = new TrainTimer(id, flowUpdateInterval);
                flowTimer.AutoReset = true;
                flowTimer.Elapsed += OnFlowTimerElapsed;

                confidenceTimers[id] = confidenceTimer;
                flowTimers[id] = flowTimer;
            }

            this.canvas = canvas;
            Tuple<StationGraph, Dictionary<int, TopologyItem>, Dictionary<int, StraightRailTrackItem>> graphInfo = GraphUtils.CreateGraph(canvas.Rails);
            stationGraph = graphInfo.Item1;
            topologyVertexDict = graphInfo.Item2;
            topologyEdgeDict = graphInfo.Item3;
            trains = new Dictionary<int, TrainItem>();
        }

        public void UpdateTrain(TrainUpdatePackage package) {
            if (!trains.ContainsKey(package.trainId)) {
                throw new ArgumentException("Tried updating not existing train with id " + package.trainId);
            }

            TrainItem train = trains[package.trainId];
            StraightRailTrackItem srtItem = topologyEdgeDict[package.edgeId];
            train.CurrentTrack = srtItem;
            train.TrackProgress = package.trackProgress;
            train.IsBroken = package.isBroken;
            confidenceTimers[package.trainId].Stop();
            confidenceTimers[package.trainId].Start();
        }
        public void ArriveTrain(TrainArrivalPackage package) {
            if (!topologyVertexDict.ContainsKey(package.inputVertexId)) {
                throw new ArgumentException("Tried adding train from nonexistent input track with id " + package.inputVertexId);
            }

            if (topologyVertexDict[package.inputVertexId] is not ExternalTrackItem inputTrackItem) {
                throw new ArgumentException("Tried adding train from non-input vertex with id " + package.inputVertexId);
            }

            StraightRailTrackItem srtItem = inputTrackItem.Port.TopologyItems.OfType<StraightRailTrackItem>().First();
            trains[package.trainId] = new TrainItem(package.trainId, srtItem);

            // start confidence and flow timers
            confidenceTimers[package.trainId].Start();
            flowTimers[package.trainId].Start();
        }
        public void DepartTrain(TrainDeparturePackage package) {
            if (!trains.ContainsKey(package.trainId)) {
                throw new ArgumentException("Tried updating not existing train with id " + package.trainId);
            }

            trains.Remove(package.trainId);

            // stop confidence and flow timers
            TrainTimer confidenceTimer = confidenceTimers[package.trainId];
            TrainTimer flowTimer = flowTimers[package.trainId];
            confidenceTimer.Stop();
            flowTimer.Stop();
        }
        public void UpdateSwitch(SwitchUpdatePackage package) {
            if (!topologyVertexDict.ContainsKey(package.vertexId)) {
                throw new ArgumentException("Tried updating nonexistent switch with id " + package.vertexId);
            }

            if (topologyVertexDict[package.vertexId] is not SwitchItem switchItem) {
                throw new ArgumentException("Tried updating not switch item with id " + package.vertexId + " as switch");
            }

            switchItem.Direction = package.direction;
            switchItem.IsBroken = package.isBroken;
        }
        public void UpdateSignal(SignalUpdatePackage package) {
            if (!topologyVertexDict.ContainsKey(package.vertexId)) {
                throw new ArgumentException("Tried updating nonexistent signal with id " + package.vertexId);
            }

            if (topologyVertexDict[package.vertexId] is not SignalItem signalItem) {
                throw new ArgumentException("Tried updating not signal item with id " + package.vertexId + " as signal");
            }

            signalItem.LightStatus = package.lightStatus;
            signalItem.IsBroken = package.isBroken;
        }
        public void UpdateRail(RailUpdatePackage package) {
            if (!topologyEdgeDict.ContainsKey(package.edgeId)) {
                throw new ArgumentException("Tried updating nonexistent rail with id " + package.edgeId);
            }
            StraightRailTrackItem srtItem = topologyEdgeDict[package.edgeId];

            srtItem.IsBroken = package.isBroken;
        }
        public void UpdateExternalTrack(ExternalTrackUpdatePackage package) {
            if (!topologyVertexDict.ContainsKey(package.vertexId)) {
                throw new ArgumentException("Tried updating nonexistent external track with id " + package.vertexId);
            }

            if (topologyVertexDict[package.vertexId] is not ExternalTrackItem externalTrackItem) {
                throw new ArgumentException("Tried updating not external track item with id " + package.vertexId + " as external track");
            }

            externalTrackItem.IsBroken = package.isBroken;
        }
        public void UpdateDeadend(DeadendUpdatePackage package) {
            if (!topologyVertexDict.ContainsKey(package.vertexId)) {
                throw new ArgumentException("Tried updating nonexistent deadend with id " + package.vertexId);
            }

            if (topologyVertexDict[package.vertexId] is not DeadendItem deadendItem) {
                throw new ArgumentException("Tried updating not deadend item with id " + package.vertexId + " as deadend");
            }

            deadendItem.IsBroken = package.isBroken;
        }
        #region Timer Elapsed Handlers
        private void OnConfidenceTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            if (sender is not TrainTimer timer) {
                throw new ArgumentException(nameof(OnConfidenceTimerElapsed) + " sent by " + sender + ". However it can be used only for " + nameof(TrainTimer));
            }

            // if confidence timer is not reset by trainUpdatePackage, respective train is considered broken
            trains[timer.TrainId].IsBroken = true;
            flowTimers[timer.TrainId].Stop();
        }
        private void OnFlowTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            if (sender is not TrainTimer timer) {
                throw new ArgumentException(nameof(OnFlowTimerElapsed) + " sent by " + sender + ". However it can be used only for " + nameof(TrainTimer));
            }

            TrainItem train = trains[timer.TrainId];
            Tuple<StraightRailTrackItem, double> nextPos = canvas.GetAdvancedTrainPos(train.FlowCurrentTrack, train.FlowTrackProgress, train.Speed, flowUpdateInterval);
            train.FlowCurrentTrack = nextPos.Item1;
            train.FlowTrackProgress = nextPos.Item2;
        }
        #endregion
    }
}
