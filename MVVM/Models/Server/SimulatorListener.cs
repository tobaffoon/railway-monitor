using railway_monitor.MVVM.Models.Station;
using railway_monitor.MVVM.Models.UpdatePackages;

namespace railway_monitor.MVVM.Models.Server {
    public class SimulatorListener : TrainStationListener {
        private bool _listensToSimulator = false;
        private StationManager _manager;

        public SimulatorListener(StationManager manager) {
            _manager = manager;
        }

        protected override void Listen() {
            _listensToSimulator = true;
        }

        public void SendTrainUpdatePackage(TrainUpdatePackage package) {
            if (!_listensToSimulator) return;

            _manager.UpdateTrain(package);
        }
        public void SendTrainArrivalPackage(TrainArrivalPackage package) {
            if (!_listensToSimulator) return;

            _manager.ArriveTrain(package);
        }
        public void SendTrainDeparturePackage(TrainDeparturePackage package) {
            if (!_listensToSimulator) return;

            _manager.DepartTrain(package);
        }
        public void SendSwitchUpdatePackage(SwitchUpdatePackage package) {
            if (!_listensToSimulator) return;

            _manager.UpdateSwitch(package);
        }
        public void SendSignalUpdatePackage(SignalUpdatePackage package) {
            if (!_listensToSimulator) return;

            _manager.UpdateSignal(package);
        }
        public void SendExternaltrackUpdatePackage(ExternalTrackUpdatePackage package) {
            if (!_listensToSimulator) return;

            _manager.UpdateExternalTrack(package);
        }
        public void SendDeadendUpdatePackage(DeadendUpdatePackage package) {
            if (!_listensToSimulator) return;

            _manager.UpdateDeadend(package);
        }
        public void SendRailUpdatePackage(RailUpdatePackage package) {
            if (!_listensToSimulator) return;

            _manager.UpdateRail(package);
        }
    }
}
