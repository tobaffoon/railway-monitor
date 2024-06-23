using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.TopologyItems;
using railway_monitor.MVVM.Models.Server;
using railway_monitor.MVVM.Models.UpdatePackages;
using railway_monitor.Utils;
using SolverLibrary;
using SolverLibrary.Model;
using SolverLibrary.Model.Graph;
using SolverLibrary.Model.Graph.VertexTypes;
using SolverLibrary.Model.TrainInfo;

namespace railway_monitor.MVVM.Models.Station {
    public class StationManager {
        #region Emergency events
        /// <summary>
        /// Arguments are: trainId, inputVertexId
        /// </summary>
        public event Action<int, int> OnUnscheduledTrainArrive;
        /// <summary>
        /// Arguments are: trainId
        /// </summary>
        public event Action<int> OnOffscheduledTrainArrive;
        /// <summary>
        /// Arguments are: trainId
        /// </summary>
        public event Action<int> OnBrokenTrain;
        #endregion
        #region Confidence and flow timers
        private static readonly int confidenceInterval = 30000;
        private static readonly int flowUpdatesPerSec = 24;
        private static readonly int flowUpdateInterval = 1000 / flowUpdatesPerSec;

        // id -> confidence one-shot timer
        private readonly Dictionary<int, TrainTimer> confidenceTimers;
        // id -> flow periodic timer
        private readonly Dictionary<int, TrainTimer> flowTimers;
        #endregion

        private int trainIdCounter;

        private readonly RailwayCanvasViewModel canvas;
        private readonly StationGraph stationGraph;
        private readonly TrainSchedule schedule;
        private readonly int timeInaccuracy;
        private readonly Dictionary<int, TopologyItem> topologyVertexDict;
        private readonly Dictionary<int, StraightRailTrackItem> topologyEdgeDict;
        private readonly Dictionary<int, TrainItem> trainItems;
        private readonly Dictionary<int, Train> trains;
        private readonly Dictionary<int, Vertex> graphVertexDict;
        private readonly Solver solver;
        private readonly StationPlanSender planSender;

        private int _currentTime;
        private readonly object _currentTimeLock = new object();
        public int CurrentTime {
            get {
                 return _currentTime;
            }
            private set {
                lock (_currentTimeLock) {
                    _currentTime = value;
                }
            }
        }

        public StationManager(RailwayCanvasViewModel canvas, TrainSchedule schedule, int timeInaccuracy) {
            CurrentTime = 0;

            // TODO: smth-smth that takes schedule and remembers it
            trainIdCounter = 0;
            trains = new Dictionary<int, Train>();
            // Process scheduule
            confidenceTimers = new Dictionary<int, TrainTimer>();
            flowTimers = new Dictionary<int, TrainTimer>();
            TrainTimer confidenceTimer, flowTimer;
            foreach (Train train in schedule.GetSchedule().Keys) {
                // add timers for each train
                confidenceTimer = new TrainTimer(trainIdCounter, confidenceInterval);
                confidenceTimer.Elapsed += OnConfidenceTimerElapsed;
                flowTimer = new TrainTimer(trainIdCounter, flowUpdateInterval);
                flowTimer.AutoReset = true;
                flowTimer.Elapsed += OnFlowTimerElapsed;
                confidenceTimers[trainIdCounter] = confidenceTimer;
                flowTimers[trainIdCounter] = flowTimer;

                // register train
                trains[trainIdCounter] = train;

                trainIdCounter++;
            }

            this.canvas = canvas;
            Tuple<StationGraph, Dictionary<int, TopologyItem>, Dictionary<int, StraightRailTrackItem>, Dictionary<int, Vertex>> graphInfo = GraphUtils.CreateGraph(canvas);
            stationGraph = graphInfo.Item1;
            topologyVertexDict = graphInfo.Item2;
            topologyEdgeDict = graphInfo.Item3;
            graphVertexDict = graphInfo.Item4;

            this.schedule = schedule;
            this.schedule.SetStationGraph(stationGraph);
            this.timeInaccuracy = timeInaccuracy;

            trainItems = new Dictionary<int, TrainItem>();

            solver = new Solver(stationGraph, timeInaccuracy);
            planSender = new SimulatorPlanSender();
        }

        public void UpdateTrain(TrainUpdatePackage package) {
            if (!trainItems.ContainsKey(package.trainId)) {
                throw new ArgumentException("Tried updating not existing train with id " + package.trainId);
            }

            TrainItem train = trainItems[package.trainId];
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

            if (!trains.ContainsKey(package.trainId)) {
                OnUnscheduledTrainArrive?.Invoke(package.trainId, package.inputVertexId);
                return;
            }
            if (IsTrainOffscheduled(package.trainId)) {
                OnOffscheduledTrainArrive?.Invoke(package.trainId);
            }
            StraightRailTrackItem srtItem = inputTrackItem.Port.TopologyItems.OfType<StraightRailTrackItem>().First();
            TrainItem trainItem = new TrainItem(package.trainId, srtItem, inputTrackItem.Port);
            trainItems[package.trainId] = trainItem;
            canvas.AddTrainItem(trainItem);

            // start confidence and flow timers
            confidenceTimers[package.trainId].Start();
            flowTimers[package.trainId].Start();
        }
        public void DepartTrain(TrainDeparturePackage package) {
            if (!trainItems.ContainsKey(package.trainId)) {
                throw new ArgumentException("Tried updating not existing train with id " + package.trainId);
            }

            canvas.DeleteTrainItem(trainItems[package.trainId]);
            trainItems.Remove(package.trainId);

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
            trainItems[timer.TrainId].IsBroken = true;
            flowTimers[timer.TrainId].Stop();
        }
        private void OnFlowTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            if (sender is not TrainTimer timer) {
                throw new ArgumentException(nameof(OnFlowTimerElapsed) + " sent by " + sender + ". However it can be used only for " + nameof(TrainTimer));
            }

            TrainItem train = trainItems[timer.TrainId];
            Tuple<StraightRailTrackItem, double> nextPos = canvas.GetAdvancedTrainPos(train, train.Speed, flowUpdateInterval, false);
            train.FlowCurrentTrack = nextPos.Item1;
            train.FlowTrackProgress = nextPos.Item2;
        }
        #endregion
        #region Emergency handlers
        internal void HandleUnscheduledTrain(int trainId, int inputVertexId, int departureTime, ExternalTrackItem outputTrack) {
            AddUnscheduledTrainEntry(trainId, inputVertexId, departureTime, outputTrack);
        }
        internal void HandleBrokenTrain(int trainId) {

        }
        internal void HandleOffscheduledTrain(int trainId, int departureTime) {
            // remove respective entry from schedule
            schedule.RemoveTrainSchedule(trains[trainId]);

            // get data for new entry
            int inputTrackId = GetScheduleById(trainId).GetVertexIn().getId();
            int outputTrackId = GetScheduleById(trainId).GetVertexOut().getId();
            ExternalTrackItem outputTrack = (ExternalTrackItem)topologyVertexDict[outputTrackId];

            // add new entry as if the train was unscheduled
            AddUnscheduledTrainEntry(trainId, inputTrackId, departureTime, outputTrack);
        }
        #endregion

        private void AddUnscheduledTrainEntry(int trainId, int inputVertexId, int departureTime, ExternalTrackItem outputTrack) {
            int outputVertexId = topologyVertexDict.First(pair => pair.Value == outputTrack).Key;
            if (graphVertexDict[inputVertexId] is not InputVertex inputVertex) {
                throw new ArgumentException("Attempted to add a train coming from vertex " + inputVertexId + " which is not input");
            }
            if (graphVertexDict[outputVertexId] is not OutputVertex outputVertex) {
                throw new ArgumentException("Attempted to add a train coming to vertex " + outputVertexId + " which is not output");
            }

            ExternalTrackItem inputItem = topologyVertexDict[inputVertexId] as ExternalTrackItem;
            StraightRailTrackItem inputTrack = inputItem.ConnectedRail;

            // register new train
            trains[trainId] = new Train(TrainItem.defaultLength, TrainItem.defaultSpeed, TrainType.NONE);
            TrainItem trainItem = new TrainItem(trainId, inputTrack, inputItem.Port);
            trainItems[trainId] = trainItem;
            canvas.AddTrainItem(trainItem);

            schedule.TryAddTrainSchedule(
                trains[trainId],
                new SingleTrainSchedule(CurrentTime, departureTime, 0, inputVertex, outputVertex)
                );
            planSender.SendPlan(solver.CalculateWorkPlan(schedule));

            ArriveTrain(new TrainArrivalPackage(trainId, inputVertexId));
        }
        private SingleTrainSchedule GetScheduleById(int trainId) {
            return schedule.GetSchedule()[trains[trainId]];
        }
        private bool IsTrainOffscheduled(int trainId) {
            lock (_currentTimeLock) {
                int expectedArrival = GetScheduleById(trainId).GetTimeArrival() - timeInaccuracy;
                int expectedDeparture = GetScheduleById(trainId).GetTimeDeparture() + timeInaccuracy;
                return expectedArrival < CurrentTime && CurrentTime < expectedDeparture;
            }
        }
    }
}
