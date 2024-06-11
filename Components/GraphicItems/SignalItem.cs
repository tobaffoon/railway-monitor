using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems
{
    public class SignalItem : GraphicItem
    {
        public enum SignalPlacementStatus
        {
            ERROR,
            NOT_PLACED,
            PLACED
        }
        public SignalPlacementStatus PlacementStatus { get; set; } = SignalPlacementStatus.NOT_PLACED;

        public enum SignalLightStatus
        {
            PASS,
            STOP
        }
        public SignalLightStatus LightStatus { get; set; } = SignalLightStatus.PASS;

        private static readonly Brush _signalPoleBrush = new SolidColorBrush(Colors.DarkGray);
        private static readonly Pen _signalPolePen = new Pen(_signalPoleBrush, 4);
        private static readonly Brush _signalStopBrush = new SolidColorBrush(Colors.Red);
        private static readonly Pen _signalStopPen = new Pen(_signalStopBrush, 0);
        private static readonly Brush _signalPassBrush = new SolidColorBrush(Colors.LawnGreen);
        private static readonly Pen _signalPassPen = new Pen(_signalPassBrush, 0);

        #region Draw params
        private static readonly double _poleLength = 20;
        private static readonly double _circleRadius = 6;
        #endregion

        static SignalItem()
        {
            _signalPolePen.StartLineCap = PenLineCap.Round;
            _signalPolePen.EndLineCap = PenLineCap.Round;
        }

        private Point _poleTop = new Point(0, 0);
        private Point PoleTopPos
        {
            get
            {
                _poleTop.X = Pos.X;
                _poleTop.Y = Pos.Y - _poleLength;
                return _poleTop;
            }
        }

        public Port Port { get; private set; }
        public Point Pos
        {
            get
            {
                return Port.Pos;
            }
            set
            {
                Port.Pos.X = value.X;
                Port.Pos.Y = value.Y;
            }
        }

        public SignalItem(Point initPos) : base()
        {
            Port = new Port(this, initPos);
        }

        public void Place(Port mainPort)
        {
            mainPort.Merge(Port);
            PlacementStatus = SignalPlacementStatus.PLACED;
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort)
        {
            if (sender is not Port newPort) return;
            Port = newPort;
        }

        protected override void Render(DrawingContext dc)
        {
            dc.DrawLine(_signalPolePen, Pos, PoleTopPos);
            if (LightStatus == SignalLightStatus.PASS)
            {
                dc.DrawEllipse(_signalPassBrush, _signalPassPen, PoleTopPos, _circleRadius, _circleRadius);
            }
            else if (LightStatus == SignalLightStatus.STOP)
            {
                dc.DrawEllipse(_signalStopBrush, _signalStopPen, PoleTopPos, _circleRadius, _circleRadius);
            }
        }
    }
}
