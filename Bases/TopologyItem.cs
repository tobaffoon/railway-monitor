namespace railway_monitor.Bases {
    public abstract class TopologyItem : GraphicItem {
        public abstract void Reassign_OnPortMerged(object? sender, Port oldPort);
    }
}
