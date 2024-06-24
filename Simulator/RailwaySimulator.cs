using SolverLibrary.Model;
using railway_monitor.MVVM.Models.Server;
using railway_monitor.MVVM.Models.UpdatePackages;
using SolverLibrary.Model.TrainInfo;
using railway_monitor.Simulator.TimedEvents;
using railway_monitor.Components.GraphicItems;
using railway_monitor.MVVM.ViewModels;
using System.Windows.Threading;
using SolverLibrary.Model.Graph.VertexTypes;
using railway_monitor.Components.TopologyItems;
using SolverLibrary.Model.PlanUnit;

namespace railway_monitor.Simulator {
    public class RailwaySimulator {
        public static readonly int defaultUpdatesPerSec = 10;

        private object _planLock = new object();
        private StationWorkPlan? _plan;
        public StationWorkPlan? Plan {
            get {
                lock (_planLock) {
                    return _plan;
                }
            }
            set {
                lock (_planLock) {
                    _plan = value;
                }
            }
        }

        private TrainSchedule? _trainSchedule;
        public TrainSchedule? TrainSchedule {
            get { return _trainSchedule; }
            set { _trainSchedule = value; }
        }

        public SimulatorUpdatesListener? UpdatesListener;

        // time -> train id
        private Dictionary<int, List<TimedEvent>> timedEvents;
        public Dictionary<int, TrainItem> trainItems;
        private Dispatcher mainDispatcher;

        private readonly HashSet<int> _simulatedTrains;

        private int updateTimerPeriod; // ms
        private int _currentTime;
        private readonly object _currentTimeLock = new object();
        private int CurrentTime {
            get {
                lock (_currentTimeLock) {
                    return _currentTime;
                }
            }
            set {
                lock (_currentTimeLock) {
                    _currentTime = value;
                }
            }
        }

        private System.Timers.Timer _timer;

        public RailwaySimulator(int updatesPerSec) {
            CurrentTime = 0;
            updateTimerPeriod = 1000 / updatesPerSec;
            _timer = new System.Timers.Timer(updateTimerPeriod);
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
            _simulatedTrains = [];
            mainDispatcher = Dispatcher.CurrentDispatcher;
        }
        public RailwaySimulator() : this(defaultUpdatesPerSec) { }

        public void Start(StationWorkPlan plan, TrainSchedule schedule, SimulatorUpdatesListener updatesListener, Dictionary<Train, int> trainIdDict) {
            _trainSchedule = schedule;
            timedEvents = [];

            // add arrival events
            foreach (var entry in schedule.GetSchedule().GroupBy(x => x.Value.GetTimeArrival())) {
                int arrivalTime = entry.Key;
                var events = entry.Select(pair => new TrainArriveEvent(trainIdDict[pair.Key], pair.Value.GetVertexIn().getId()));
                if (!timedEvents.ContainsKey(arrivalTime)) {
                    timedEvents[arrivalTime] = new List<TimedEvent>(events);
                }
                else {
                    timedEvents[arrivalTime].AddRange(events);
                }
            }

            // add switch events
            foreach (var entry in plan.GetSwitchPlanUnits().GroupBy(x => x.GetBeginTime())) {
                int time = entry.Key >= 0 ? entry.Key : 0;
                var events = entry.Select(x => new SwitchEvent(x.GetVertex().getId(), x.GetStatus()));
                var inverseEvents = entry.Select(x => new SwitchEvent(x.GetVertex().getId(), ReverseSwitchStatus(x)));
                if (!timedEvents.ContainsKey(time)) {
                    timedEvents[time] = new List<TimedEvent>(events);
                }
                else {
                    timedEvents[time].AddRange(events);
                }
                timedEvents[time].AddRange(inverseEvents);
            }

            // add signal events
            foreach (var entry in plan.GetTrafficLightPlanUnits().GroupBy(x => x.GetBeginTime())) {
                int time = entry.Key >= 0 ? entry.Key : 0;
                var events = entry.Select(x => new SignalEvent(x.GetVertex().getId(), x.GetStatus()));
                var inverseEvents = entry.Select(x => new SignalEvent(x.GetVertex().getId(), ReverseTrafficLightStatus(x)));
                if (!timedEvents.ContainsKey(time)) {
                    timedEvents[time] = new List<TimedEvent>(events);
                }
                else {
                    timedEvents[time].AddRange(events);
                }
                timedEvents[time].AddRange(inverseEvents);
            }

            UpdatesListener = updatesListener;
            UpdatesListener.Listen();
            OnTimerElapsed(null, null);
            _timer.Start();
        }

        public void Stop() {
            _timer.Stop();
            CurrentTime = 0;
        }

        private async void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            if (timedEvents.ContainsKey(CurrentTime)) {
                foreach (TimedEvent tevent in timedEvents[CurrentTime]) {
                    switch (tevent) {
                        case TrainArriveEvent arriveEvent:
                            _simulatedTrains.Add(arriveEvent.trainId);
                            await Task.Run(() =>
                                mainDispatcher.Invoke(() =>
                                    UpdatesListener.SendTrainArrivalPackage(new TrainArrivalPackage(arriveEvent.trainId, arriveEvent.inputVertexId))
                                )
                            );
                            break;
                        case SwitchEvent switchEvent:
                            SwitchItem.SwitchDirection switchStatus = SwitchItem.SwitchDirection.FIRST;
                            switch (switchEvent.status) {
                                case SwitchStatus.PASSINGCON1:
                                    switchStatus = SwitchItem.SwitchDirection.FIRST;
                                    break;
                                case SwitchStatus.PASSINGCON2:
                                    switchStatus = SwitchItem.SwitchDirection.SECOND;
                                    break;
                            }
                            QueueTask(() => UpdatesListener.SendSwitchUpdatePackage(new SwitchUpdatePackage(switchEvent.switchId, false, switchStatus)));
                            break;
                        case SignalEvent signalEvent:
                            SignalItem.SignalLightStatus signalStatus = SignalItem.SignalLightStatus.PASS;
                            switch (signalEvent.status) {
                                case TrafficLightStatus.STOP:
                                    signalStatus = SignalItem.SignalLightStatus.STOP;
                                    break;
                                case TrafficLightStatus.PASSING:
                                    signalStatus = SignalItem.SignalLightStatus.PASS;
                                    break;
                            }
                            QueueTask(() => UpdatesListener.SendSignalUpdatePackage(new SignalUpdatePackage(signalEvent.signalId, false, signalStatus)));
                            break;
                    }
                }
            }

            // move trains
            foreach (int trainId in _simulatedTrains) {
                var nextTrainPos = RailwayMonitorViewModel.GetAdvancedTrainPos(trainItems[trainId], true);
                bool trainDeparted = nextTrainPos.Item4;
                if (trainDeparted) {
                    QueueTask(() => UpdatesListener.SendTrainDeparturePackage(new TrainDeparturePackage(trainId)));
                    _simulatedTrains.Remove(trainId);
                }
                else {
                    QueueTask(() => UpdatesListener.SendTrainUpdatePackage(new TrainUpdatePackage(trainId, nextTrainPos.Item1, nextTrainPos.Item2, nextTrainPos.Item3, false)));
                }
            }

            CurrentTime++;
            QueueTask(() => UpdatesListener.UpdateTime(CurrentTime));
        }

        private async void QueueTask(Action action) {
            await Task.Run(() =>
                mainDispatcher.Invoke(action)
            );
        }

        private TrafficLightStatus ReverseTrafficLightStatus(TrafficLightPlanUnit unit) {
            return unit.GetStatus() == TrafficLightStatus.STOP ? TrafficLightStatus.PASSING : TrafficLightStatus.STOP;
        }

        private SwitchStatus ReverseSwitchStatus(SwitchPlanUnit unit) {
            return unit.GetStatus() == SwitchStatus.PASSINGCON1 ? SwitchStatus.PASSINGCON2 : SwitchStatus.PASSINGCON1;
        }
    }
}