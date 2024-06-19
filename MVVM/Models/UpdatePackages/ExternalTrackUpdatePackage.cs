namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct ExternalTrackUpdatePackage {
        public readonly int vertexId;
        public readonly bool isBroken;

        public ExternalTrackUpdatePackage(int vertexId, bool isBroken) {
            this.vertexId = vertexId;
            this.isBroken = isBroken;
        }
    }
}
