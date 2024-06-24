using SolverLibrary.Model;
using railway_monitor.MVVM.Models.Server;
using railway_monitor.MVVM.Models.UpdatePackages;
using SolverLibrary.Model.TrainInfo;
using railway_monitor.Simulator.TimedEvents;
using railway_monitor.Components.GraphicItems;
using railway_monitor.MVVM.ViewModels;
using System.Windows.Threading;

namespace railway_monitor.Simulator {
    public class RailwaySimulator {
        public static readonly int defaultUpdatesPerSec = 1;

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

        public RailwaySimulator() {
            CurrentTime = 0;
            updateTimerPeriod = 200 / defaultUpdatesPerSec;
            _timer = new System.Timers.Timer(updateTimerPeriod);
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
            _simulatedTrains = [];
            mainDispatcher = Dispatcher.CurrentDispatcher;
        }

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
                    }
                }

            }

            // move trains
            foreach (int trainId in _simulatedTrains) {
                var nextTrainPos = RailwayMonitorViewModel.GetAdvancedTrainPos(trainItems[trainId], TrainItem.defaultSpeed, updateTimerPeriod, true);
                await Task.Run(() =>
                    mainDispatcher.Invoke(() => 
                        UpdatesListener.SendTrainUpdatePackage(new TrainUpdatePackage(trainId, nextTrainPos.Item1, nextTrainPos.Item2, nextTrainPos.Item3, false))
                    )
                );
            }

            CurrentTime++; 
            await Task.Run(() =>
                mainDispatcher.Invoke(() =>
                    UpdatesListener.UpdateTime(CurrentTime)
                 )
            );
        }
    }
}
