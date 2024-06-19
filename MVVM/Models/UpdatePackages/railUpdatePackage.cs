namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct RailUpdatePackage {
        public readonly int vertexId;
        public readonly bool isBroken;

        public RailUpdatePackage(int vertexId, bool isBroken) {
            this.vertexId = vertexId;
            this.isBroken = isBroken;
        }
    }
}
