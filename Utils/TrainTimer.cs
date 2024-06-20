namespace railway_monitor.Utils {
    public class TrainTimer : System.Timers.Timer {
        public int TrainId { get; }
        public TrainTimer(int trainId, int interval) : base(interval) {
            TrainId = trainId;
        }
    }
}
