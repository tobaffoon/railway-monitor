namespace railway_monitor.Simulator.Sync {
    public class SimpleAsyncResult : IAsyncResult {
        object _state;

        public bool IsCompleted { get; set; }

        public WaitHandle AsyncWaitHandle { get; internal set; }

        public object AsyncState {
            get {
                if (Exception != null) {
                    throw Exception;
                }
                return _state;
            }
            internal set {
                _state = value;
            }
        }

        public bool CompletedSynchronously { get { return IsCompleted; } }

        internal Exception Exception { get; set; }
    }
}
