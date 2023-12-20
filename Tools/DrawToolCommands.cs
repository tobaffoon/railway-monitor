using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using railway_monitor.Tools.DrawCommands;

namespace railway_monitor.Tools
{
    public static class DrawToolCommands
    {
        public static readonly DrawCommand DrawStraightRailTrack = new DrawStraightRailTrackCommand();
        public static readonly DrawCommand AddSwitch = new AddSwitchCommand();
        public static readonly DrawCommand DrawDeadend = new DrawDeadendCommand();
        public static readonly DrawCommand DrawSignal = new DrawSignalCommand();
        public static readonly DrawCommand DrawExternalTrack = new DrawExternalTrackCommand();
    }
}
