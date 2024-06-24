using railway_monitor.Components.TopologyItems;

namespace railway_monitor.MVVM.Models.UpdatePackages {
    public readonly struct SignalUpdatePackage {
        public readonly int vertexId;
        public readonly bool isBroken;
        public readonly SignalItem.SignalLightStatus lightStatus;

        public SignalUpdatePackage(int vertexId, bool isBroken, SignalItem.SignalLightStatus lightStatus) {
            this.vertexId = vertexId;
            this.isBroken = isBroken;
            this.lightStatus = lightStatus;
        }
    }
}
