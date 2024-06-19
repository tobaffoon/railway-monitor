namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct TrainArrivePackage {
        public readonly int trainId;
        public readonly int inputVertexId;

        public TrainArrivePackage(int trainId, int inputVertexId) {
            this.trainId = trainId;
            this.inputVertexId = inputVertexId;
        }
    }
}
