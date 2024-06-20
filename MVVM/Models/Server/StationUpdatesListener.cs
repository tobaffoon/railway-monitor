using railway_monitor.MVVM.Models.Station;

namespace railway_monitor.MVVM.Models.Server {
    public abstract class StationUpdateListener {
        protected StationManager Manager;
        protected abstract void Listen();

        public StationUpdateListener(StationManager Manager) {
            this.Manager = Manager;
        }
    }
}
