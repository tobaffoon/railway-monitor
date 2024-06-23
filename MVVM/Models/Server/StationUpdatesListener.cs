using railway_monitor.MVVM.Models.Station;

namespace railway_monitor.MVVM.Models.Server {
    public abstract class StationUpdatesListener {
        protected StationManager Manager;
        public abstract void Listen();

        public StationUpdatesListener(StationManager Manager) {
            this.Manager = Manager;
        }
    }
}
