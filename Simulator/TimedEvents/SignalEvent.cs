using SolverLibrary.Model.Graph.VertexTypes;

namespace railway_monitor.Simulator.TimedEvents {
    public class SignalEvent : TimedEvent {
        public readonly TrafficLightStatus status;
        public readonly int signalId;

        public SignalEvent(int signalId, TrafficLightStatus status) {
            this.signalId = signalId;
            this.status = status;
        }
    }
}
