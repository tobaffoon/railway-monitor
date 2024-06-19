namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct RailUpdatePackage {
        public readonly int edgeId;
        public readonly bool isBroken;

        public RailUpdatePackage(int edgeId, bool isBroken) {
            this.edgeId = edgeId;
            this.isBroken = isBroken;
        }
    }
}
