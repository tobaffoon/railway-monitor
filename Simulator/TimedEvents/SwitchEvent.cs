using SolverLibrary.Model.Graph.VertexTypes;

namespace railway_monitor.Simulator.TimedEvents {
    public class SwitchEvent : TimedEvent {
        public readonly SwitchStatus status;
        public readonly int switchId;

        public SwitchEvent(int switchId, SwitchStatus status) {
            this.switchId = switchId;
            this.status = status;
        }
    }
}
