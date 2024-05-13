using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using railway_monitor.Utils;
using railway_monitor.MVVM.ViewModels;
using System.Windows;
using System.Windows.Diagnostics;
using System.Windows.Media;
using railway_monitor.Components.GraphicItems;
using System.Diagnostics;

namespace railway_monitor.Components.RailwayCanvas
{
    public class RailwayCanvasViewModel : ViewModelBase
    {
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

        public ObservableHashSet<Shape> GraphicItems { get; } = [];
        public Shape? LatestShape { get; set; }
        public int Len { get { return GraphicItems.Count; } }

        public RailwayCanvasViewModel()
        {
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

        public void ResetLatestShape()
        {
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

        public Point TryFindRailConnection(Point mousePos)
        {
            // circle in which new srt tries to connect to an old srt
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(mousePos, _connectRadius, _connectRadius);
            foreach (StraightRailTrackItem srt in GraphicItems.OfType<StraightRailTrackItem>())
            {
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
                        return ConnectionTrack.End;
                    }
                    ((EllipseGeometry)HighlightConnection.Data).Center = ConnectionTrack.Start;
                    return ConnectionTrack.Start;
                }
            }

            HighlightConnection.Visibility = Visibility.Collapsed;
            return mousePos;
        }
    }
}
