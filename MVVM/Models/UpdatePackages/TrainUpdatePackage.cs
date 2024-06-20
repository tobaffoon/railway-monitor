namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct TrainUpdatePackage {
        public readonly int trainId;
        public readonly int edgeId;
        public readonly double trackProgress;
        public readonly bool isBroken;

        public TrainUpdatePackage(int trainId, int edgeId, double trackProgress, bool isBroken) {
            this.trainId = trainId;
            this.edgeId = edgeId;
            this.trackProgress = trackProgress;
            this.isBroken = isBroken;
        }
    }
}
