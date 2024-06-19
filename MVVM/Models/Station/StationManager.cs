using railway_monitor.Bases;
using railway_monitor.Components;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Utils;
using SolverLibrary.Model.Graph;
using SolverLibrary.Model.Graph.VertexTypes;

namespace railway_monitor.MVVM.Models.Station
{
    public class StationManager
    {
        private RailwayCanvasViewModel canvas;
        private StationGraph stationGraph;
        private Dictionary<Vertex, TopologyItem> topologyVertexDict;
        private Dictionary<Edge, StraightRailTrackItem> topologyEdgeDict;
        private Dictionary<int, TrainItem> trains;

        public StationManager(RailwayCanvasViewModel canvas)
        {
            this.canvas = canvas;
            Tuple<StationGraph, Dictionary<Vertex, TopologyItem>, Dictionary<Edge, StraightRailTrackItem>> graphInfo = GraphUtils.CreateGraph(canvas.Rails);
            stationGraph = graphInfo.Item1;
            topologyVertexDict = graphInfo.Item2;
            topologyEdgeDict = graphInfo.Item3;
            trains = new Dictionary<int, TrainItem>();
        }
    }
}
