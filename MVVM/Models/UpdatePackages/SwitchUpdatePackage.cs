namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct SwitchUpdatePackage {
        public enum SwitchDirection {
            FIRST,
            SECOND
        }
        public readonly int vertexId;
        public readonly bool isBroken;
        public readonly SwitchDirection direction;
        
        public SwitchUpdatePackage(int vertexId, bool isBroken, SwitchDirection direction) {
            this.vertexId = vertexId;
            this.isBroken = isBroken;
            this.direction = direction;
        }
    }
}
