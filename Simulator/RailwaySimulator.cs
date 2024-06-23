using SolverLibrary.Model;

namespace railway_monitor.Simulator {
    public class RailwaySimulator {
        public static readonly int defaultUpdatesPerSec = 1;

        private StationWorkPlan _plan;
        public StationWorkPlan Plan {
            get { return _plan; }
            set { _plan = value; }
        }

        private TrainSchedule _trainSchedule;
        public TrainSchedule TrainSchedule {
            get { return _trainSchedule; }
            set { _trainSchedule = value; }
        }

        private int updateTimerPeriod; // ms
        private int CurrentTime;
        private System.Timers.Timer _timer;

        public RailwaySimulator(StationWorkPlan plan, TrainSchedule schedule) {          
            _plan = plan;
            _trainSchedule = schedule;
            updateTimerPeriod = 1000 / defaultUpdatesPerSec;
            _timer = new System.Timers.Timer(updateTimerPeriod);
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
        }

        private void Start() {
        
        }

        private void Stop() {

        }

        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e) {

        }
    }
}
