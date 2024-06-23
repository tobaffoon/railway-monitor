using SolverLibrary.Model;
using railway_monitor.MVVM.Models.Server;

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

        private int updateTimerPeriod; // ms
        private int CurrentTime;
        private System.Timers.Timer _timer;

        public RailwaySimulator() {
            CurrentTime = 0;
            updateTimerPeriod = 1000 / defaultUpdatesPerSec;
            _timer = new System.Timers.Timer(updateTimerPeriod);
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
        }

        public void Start(StationWorkPlan plan, TrainSchedule schedule, SimulatorUpdatesListener updatesListener) {
            _trainSchedule = schedule;
            UpdatesListener = updatesListener;
            UpdatesListener.Listen();
            _timer.Start();
        }

        public void Stop() {
            _timer.Stop();
            CurrentTime = 0;
        }

        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            CurrentTime++;
            UpdatesListener.UpdateTime(CurrentTime);
        }
    }
}
