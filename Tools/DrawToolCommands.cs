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
        public static readonly CommandBase DrawStraightRailTrack = new DrawStraightRailTrackCommand();
        public static readonly CommandBase AddSwitch = new AddSwitchCommand();
        public static readonly CommandBase DrawDeadend = new DrawDeadendCommand();
        public static readonly CommandBase DrawSignal = new DrawSignalCommand();
        public static readonly CommandBase DrawExternalTrack = new DrawExternalTrackCommand();
    }
}
