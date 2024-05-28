using System.Windows.Shapes;
using railway_monitor.MVVM.ViewModels;
using System.Windows;
using System.Windows.Media;
using railway_monitor.Components.GraphicItems;
using System.Collections.ObjectModel;
using railway_monitor.Bases;

namespace railway_monitor.Components.RailwayCanvas
{
    public class RailwayCanvasViewModel : ViewModelBase
    {
        # region HighlightConnection
        private static readonly double _connectRadius = 15;
        private static readonly Brush _highlightBrush = new SolidColorBrush(Color.FromArgb(100, 51, 153, 255));
        private StraightRailTrackItem? ConnectionTrack { get; set; }
        private Path HighlightConnection { get; set; } = new Path{
            Fill = _highlightBrush,
            Visibility = Visibility.Collapsed,
            Data = new EllipseGeometry
            {
                RadiusX = _connectRadius,
                RadiusY = _connectRadius
            }
        };
        #endregion

        public ObservableCollection<Shape> GraphicItems { get; }
        public Shape? LatestShape { get; set; }
        public int Len { get { return GraphicItems.Count; } }

        public RailwayCanvasViewModel()
        {
            GraphicItems = [];
            GraphicItems.Add(HighlightConnection);
        }

        public void AddShape(Shape shape)
        {
            GraphicItems.Add(shape);
            LatestShape = shape;
        }

        public void DeleteShape(Shape shape)
        {
            GraphicItems.Remove(shape);
            if(shape == LatestShape)
            {
                LatestShape = null;
            }
        }
        public void DeleteStraightRailTrack(StraightRailTrackItem srt)
        {
            DeleteShape(srt);
            srt.PortStart.RemoveItem(srt);
            srt.PortEnd.RemoveItem(srt);
        }
        public void DeleteSwitch(SwitchItem swtch)
        {
            DeleteShape(swtch);
            swtch.Port.RemoveItem(swtch);
        }

        public void DeleteLatestShape()
        {
            if(LatestShape != null)
            {
                DeleteShape(LatestShape);
            }
        }

        private bool RailDuplicates(StraightRailTrackItem srt)
        {
            // search among all SRTs on the same starting port excluding newly added one
            foreach (StraightRailTrackItem item in srt.PortStart.GraphicItems.OfType<StraightRailTrackItem>().Except<StraightRailTrackItem>([srt])) 
            {
                if (item.Start == srt.Start && item.End == srt.End || item.Start == srt.End && item.End == srt.Start)
                {
                    return true;
                }
            }
            return false;
        }
        
        private bool SwitchDuplicates(SwitchItem swtch)
        {
            if (swtch.Port.GraphicItems.OfType<SwitchItem>().Count() > 1) 
            {
                return true;
            }
            return false;
        }

        public void ResetLatestShape()
        {
            switch (LatestShape)
            {
                case StraightRailTrackItem srt:
                    if (RailDuplicates(srt))
                    {
                        DeleteStraightRailTrack(srt);
                    }
                    break;
                case SwitchItem swtch:
                    if (SwitchDuplicates(swtch))
                    {
                        DeleteSwitch(swtch);
                    }
                    break;
            }
            LatestShape = null;
        }

        private HitTestResultBehavior RailHitTestResult(HitTestResult result)
        {
            if (result.VisualHit != LatestShape)
            {
                ConnectionTrack = result.VisualHit as StraightRailTrackItem;
                return HitTestResultBehavior.Stop;
            }
            else
            {
                return HitTestResultBehavior.Continue;
            }
        }

        public Port? TryFindRailConnection(Point mousePos)
        {
            // circle in which new srt tries to connect to an old srt
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(mousePos, _connectRadius, _connectRadius);

            foreach (StraightRailTrackItem srt in GraphicItems.OfType<StraightRailTrackItem>())
            {
                // try finding close srt by hittesting
                ConnectionTrack = null;
                VisualTreeHelper.HitTest(srt, 
                    null,
                    new HitTestResultCallback(RailHitTestResult),
                    new GeometryHitTestParameters(expandedHitTestArea));
                if (ConnectionTrack == null) continue; 

                // determine close enough vertex or make sure that there is no such vertex
                double distance1 = (ConnectionTrack.Start - mousePos).Length;
                double distance2 = (ConnectionTrack.End - mousePos).Length;
                if (distance1 < _connectRadius || distance2 < _connectRadius)
                {
                    HighlightConnection.Visibility = Visibility.Visible;
                    if (distance2 < distance1)
                    {
                        ((EllipseGeometry)HighlightConnection.Data).Center = ConnectionTrack.End;
                        return ConnectionTrack.PortEnd;
                    }
                    ((EllipseGeometry)HighlightConnection.Data).Center = ConnectionTrack.Start;
                    return ConnectionTrack.PortStart;
                }
            }

            // hide highlighter when no track is close enough
            HighlightConnection.Visibility = Visibility.Collapsed;
            return null;
        }
    }
}
