namespace railway_monitor.Bases {
    public abstract class TopologyItem : GraphicItem {
        public abstract void Reassign_OnPortMerged(object? sender, Port oldPort);

        private bool _isBroken = false;
        public bool IsBroken {
            get {
                return _isBroken;
            }
            set {
                _isBroken = value;
                Render();
            }
        }
    }
}
