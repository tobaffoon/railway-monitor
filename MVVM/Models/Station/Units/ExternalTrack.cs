using railway_monitor.MVVM.Models.Station;

namespace railway_monitor.MVVM.Models.Station.Units
{
    public enum ExternalTrackType
    {
        INPUT,
        OUTPUT
    }
    public class ExternalTrack : NodeUnit
    {
        public readonly ExternalTrackType Type;
        public ExternalTrack(ExternalTrackType type)
        {
            Type = type;
        }
    }
}
