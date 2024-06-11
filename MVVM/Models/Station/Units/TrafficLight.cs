namespace railway_monitor.MVVM.Models.Station.Units {
    public enum TrafficLightStatus {
        STOP,
        PASSING
    }
    public class TrafficLight : NodeUnit {
        private TrafficLightStatus _status;
        public TrafficLightStatus Status {
            get => _status;
            set => SetField(ref _status, value);
        }

        public void Toggle() {
            Status = _status switch {
                TrafficLightStatus.STOP => TrafficLightStatus.PASSING,
                TrafficLightStatus.PASSING => TrafficLightStatus.STOP,
                _ => TrafficLightStatus.PASSING
            };
        }
    }
}
