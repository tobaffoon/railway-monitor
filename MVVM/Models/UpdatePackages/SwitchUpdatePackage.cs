using railway_monitor.Components.TopologyItems;

namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct SwitchUpdatePackage {
        public readonly int vertexId;
        public readonly bool isBroken;
        public readonly SwitchItem.SwitchDirection direction;
        
        public SwitchUpdatePackage(int vertexId, bool isBroken, SwitchItem.SwitchDirection direction) {
            this.vertexId = vertexId;
            this.isBroken = isBroken;
            this.direction = direction;
        }
    }
}
