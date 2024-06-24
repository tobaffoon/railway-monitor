namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct TrainArrivalPackage {
        public readonly int trainId;
        public readonly int inputVertexId;

        public TrainArrivalPackage(int trainId, int inputVertexId) {
            this.trainId = trainId;
            this.inputVertexId = inputVertexId;
        }
    }
}
