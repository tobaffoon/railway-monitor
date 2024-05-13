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
        private static readonly double _connectRadius = 10;
        private StraightRailTrack? ConnectionTrack {  get; set; }

        public ObservableHashSet<Shape> GraphicItems { get; } = new ObservableHashSet<Shape>();
        public Shape? LatestShape { get; set; }
        public int Len { get { return GraphicItems.Count; } }

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
                ConnectionTrack = result.VisualHit as StraightRailTrack;
                return HitTestResultBehavior.Stop;
            }
            else
            {
                return HitTestResultBehavior.Continue;
            }
        }

        public Point TryFindRailConnection(Point mousePos)
        {
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(mousePos, _connectRadius, _connectRadius);
            foreach (StraightRailTrack srt in GraphicItems.OfType<StraightRailTrack>())
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
                    if (distance2 < distance1) return ConnectionTrack.End;
                    return ConnectionTrack.Start;
                }
            }
            return mousePos;
        }
    }
}
