namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct DeadendUpdatePackage {
        public readonly int vertexId;
        public readonly bool isBroken;

        public DeadendUpdatePackage(int vertexId, bool isBroken) {
            this.vertexId = vertexId;
            this.isBroken = isBroken;
        }
    }
}
