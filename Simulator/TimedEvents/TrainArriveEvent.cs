namespace railway_monitor.Simulator.TimedEvents {
    public class TrainArriveEvent : TimedEvent {
        public readonly int trainId;
        public readonly int inputVertexId;
        public TrainArriveEvent(int trainId, int inputVertexId) {
            this.trainId = trainId;
            this.inputVertexId = inputVertexId;
        }
    }
}
