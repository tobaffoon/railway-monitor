using railway_monitor.MVVM.Models.Station;
using railway_monitor.MVVM.Models.UpdatePackages;

namespace railway_monitor.MVVM.Models.Server {
    public class SimulatorUpdatesListener : StationUpdatesListener {
        private bool _listensToSimulator = false;

        public SimulatorUpdatesListener(StationManager manager) : base(manager) { }

        public override void Listen() {
            _listensToSimulator = true;
        }

        public void SendTrainUpdatePackage(TrainUpdatePackage package) {
            if (!_listensToSimulator) return;

            Manager.UpdateTrain(package);
        }
        public void SendTrainArrivalPackage(TrainArrivalPackage package) {
            if (!_listensToSimulator) return;

            Manager.ArriveTrain(package);
        }
        public void SendTrainDeparturePackage(TrainDeparturePackage package) {
            if (!_listensToSimulator) return;

            Manager.DepartTrain(package);
        }
        public void SendSwitchUpdatePackage(SwitchUpdatePackage package) {
            if (!_listensToSimulator) return;

            Manager.UpdateSwitch(package);
        }
        public void SendSignalUpdatePackage(SignalUpdatePackage package) {
            if (!_listensToSimulator) return;

            Manager.UpdateSignal(package);
        }
        public void SendExternaltrackUpdatePackage(ExternalTrackUpdatePackage package) {
            if (!_listensToSimulator) return;

            Manager.UpdateExternalTrack(package);
        }
        public void SendDeadendUpdatePackage(DeadendUpdatePackage package) {
            if (!_listensToSimulator) return;

            Manager.UpdateDeadend(package);
        }
        public void SendRailUpdatePackage(RailUpdatePackage package) {
            if (!_listensToSimulator) return;

            Manager.UpdateRail(package);
        }
        public void UpdateTime(int time) {
            if (!_listensToSimulator) return;

            Manager.CurrentTime = time;
        }
    }
}
