using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace railway_monitor.Tools.Actions
{
    public sealed class MoveToolActions
    {
        public static void MoveStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Shape? shape = canvas.LatestShape;
            if (shape == null)
            {
                shape = new StraightRailTrackItem();
                canvas.AddShape(shape);
            }
            else if (shape is not StraightRailTrackItem)
            {
                shape = new StraightRailTrackItem();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            StraightRailTrackItem srt = (StraightRailTrackItem)shape;
            Port? connectionPort = canvas.TryFindRailConnection(mousePos);
            Point connectionPos = connectionPort == null ? mousePos : connectionPort.Pos;
            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED)
            {
                srt.Start = connectionPos;
            }
            else
            {
                srt.End = connectionPos;
            }

            srt.InvalidateMeasure();
        }

        private static bool IsSwitchConnectable(Port connectionPort)
        {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() == 3 || connectionPort.GraphicItems.OfType<SwitchItem>().Count() == 0;
        }

        public static void MoveSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Shape? shape = canvas.LatestShape;
            if (shape == null)
            {
                shape = new SwitchItem();
                canvas.AddShape(shape);
            }
            else if (shape is not SwitchItem)
            {
                shape = new SwitchItem();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            SwitchItem switchItem = (SwitchItem)shape;
            Port? connectionPort = canvas.TryFindRailConnection(mousePos);
            switch (switchItem.Status)
            {
                case SwitchItem.PlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null)
                    {
                        connectionPos = mousePos;
                    }
                    else 
                    {
                        if (!IsSwitchConnectable(connectionPort)) 
                        {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else
                        {
                            connectionPos = connectionPort.Pos;
                        } 
                    }

                    switchItem.Pos = connectionPos;
                    switchItem.InvalidateMeasure();
                    break;
                case SwitchItem.PlacementStatus.PLACED:
                    switchItem.InvalidateMeasure();
                    break;
                default:
                    return;
            }


            
        }
        public static void MoveSignal(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Signal");
        }
        public static void MoveDeadend(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Deadend");
        }
        public static void MoveExternalTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("External track");
        }
    }
}
