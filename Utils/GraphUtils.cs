using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.TopologyItems;
using SolverLibrary.Model.Graph;
using SolverLibrary.Model.Graph.VertexTypes;
using SolverLibrary.Model.TrainInfo;
using System.Windows;

namespace railway_monitor.Utils {
    public class GraphUtils {
        private static readonly int _defaultEdgeLength = 100;
        private static readonly Point _initGraphPos = new Point(100, 100);

        /// <summary>
        /// Calculates stationGraph from topology Items.
        /// </summary>
        /// <param name="rails"></param>
        /// <returns>
        /// Created StationGraph and dictionaries 
        /// 1. ids of its vertices -> topology items
        /// 2. ids of its edges -> StraightRailTrack
        /// 3. ids of its vertices -> Vertex objects in graph
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static Tuple<StationGraph, Dictionary<int, TopologyItem>, Dictionary<int, StraightRailTrackItem>, Dictionary<int, Vertex>> CreateGraph(List<StraightRailTrackItem> rails) {
            if(rails.Count() == 0) {
                throw new ArgumentException("No rails drawn");
            }
            
            Dictionary<Port, Vertex> vertexDict = new Dictionary<Port, Vertex>();
            Dictionary<StraightRailTrackItem, Edge> edgesDict = new Dictionary<StraightRailTrackItem, Edge>();
            int vertexIdCounter = 0;
            int edgeIdCounter = 0;

            // list edges with their vertices
            Port portStart, portEnd;
            Vertex vertexStart, vertexEnd;
            TrainType edgeType;
            foreach (StraightRailTrackItem srt in rails) {
                portStart = srt.PortStart;
                portEnd = srt.PortEnd;

                // add starting port as vertex, if not present
                if (!vertexDict.ContainsKey(portStart)) {
                    vertexDict[portStart] = CreateVertexFromPort(portStart, vertexIdCounter);
                    vertexIdCounter++;
                }
                vertexStart = vertexDict[portStart];

                // add ending port as vertex, if not present
                if (!vertexDict.ContainsKey(portEnd)) {
                    vertexDict[portEnd] = CreateVertexFromPort(portEnd, vertexIdCounter);
                    vertexIdCounter++;
                }
                vertexEnd = vertexDict[portEnd];

                // map type of platform
                switch (srt.PlatformType) {
                    case StraightRailTrackItem.RailPlatformType.PASSENGER:
                        edgeType = TrainType.PASSENGER;
                        break;
                    case StraightRailTrackItem.RailPlatformType.CARGO:
                        edgeType = TrainType.CARGO;
                        break;
                    case StraightRailTrackItem.RailPlatformType.NONE:
                    default:
                        edgeType = TrainType.NONE;
                        break;
                }

                edgesDict[srt] = new Edge(edgeIdCounter, _defaultEdgeLength, vertexStart, vertexEnd, edgeType);
                if (srt.IsBroken) edgesDict[srt].Block();
                edgeIdCounter++;
            }

            // add edges links to each vertex
            // also calculate vertex id -> topology item dictionary
            StraightRailTrackItem[] portSrts;
            TopologyItem relatedItem;
            Dictionary<int, TopologyItem> topologyDict = new Dictionary<int, TopologyItem>(vertexIdCounter); // capacity is not bigger than number of vertices (connection vertices does not map to any item)
            foreach (Port port in vertexDict.Keys) {
                portSrts = port.TopologyItems.OfType<StraightRailTrackItem>().ToArray();
                switch (vertexDict[port]) {
                    case InputVertex inputVertex:
                        inputVertex.SetEdge(edgesDict[port.TopologyItems.OfType<StraightRailTrackItem>().First()]);
                        relatedItem = port.TopologyItems.OfType<ExternalTrackItem>().First();
                        topologyDict[inputVertex.getId()] = relatedItem;
                        if (relatedItem.IsBroken) inputVertex.Block();
                        break;
                    case OutputVertex outputVertex:
                        outputVertex.SetEdge(edgesDict[port.TopologyItems.OfType<StraightRailTrackItem>().First()]);
                        relatedItem = port.TopologyItems.OfType<ExternalTrackItem>().First();
                        topologyDict[outputVertex.getId()] = relatedItem;
                        if (relatedItem.IsBroken) outputVertex.Block();
                        break;
                    case ConnectionVertex connectionVertex:
                        if(portSrts.Length != 2) {
                            throw new ArgumentException("Drawn station contains disallowed hanging vertices");
                        }
                        connectionVertex.SetEdges(edgesDict[portSrts[0]], edgesDict[portSrts[1]]);
                        if (portSrts[0].IsBroken || portSrts[1].IsBroken) connectionVertex.Block();
                        break;
                    case DeadEndVertex deadEndVertex:
                        deadEndVertex.SetEdge(edgesDict[port.TopologyItems.OfType<StraightRailTrackItem>().First()]);
                        relatedItem = port.TopologyItems.OfType<DeadendItem>().First();
                        topologyDict[deadEndVertex.getId()] = relatedItem;
                        if (relatedItem.IsBroken) deadEndVertex.Block();
                        break;
                    case TrafficLightVertex trafficLightVertex:
                        trafficLightVertex.SetEdges(edgesDict[portSrts[0]], edgesDict[portSrts[1]]);
                        relatedItem = port.TopologyItems.OfType<SignalItem>().First();
                        topologyDict[trafficLightVertex.getId()] = relatedItem;
                        if (relatedItem.IsBroken) trafficLightVertex.Block();
                        break;
                    case SwitchVertex switchVertex:
                        SwitchItem switchItem = port.TopologyItems.OfType<SwitchItem>().First();

                        switchVertex.SetEdges(edgesDict[switchItem.SrcTrack], edgesDict[switchItem.DstOneTrack], edgesDict[switchItem.DstTwoTrack]);
                        relatedItem = port.TopologyItems.OfType<SwitchItem>().First();
                        topologyDict[switchVertex.getId()] = relatedItem;
                        if (relatedItem.IsBroken && switchVertex.GetWorkCondition() == SwitchWorkCondition.WORKING) switchVertex.ChangeWorkCondition();
                        break;
                }
            }

            StationGraph graph = new StationGraph();
            foreach (Vertex v in vertexDict.Values) {
                graph.TryAddVerticeWithEdges(v);
            }
            return Tuple.Create(graph, topologyDict, edgesDict.ToDictionary(x => x.Value.getId(), x => x.Key), vertexDict.ToDictionary(x => x.Value.getId(), x => x.Value)); // reverse edgesDict to map Edges to SRTs
        }

        private static Vertex CreateVertexFromPort(Port port, int vertexId) {
            if (Port.IsPortInput(port)) {
                return new InputVertex(vertexId);
            }
            else if (Port.IsPortOutput(port)) {
                return new OutputVertex(vertexId);
            }
            else if (Port.IsPortDeadend(port)) {
                return new DeadEndVertex(vertexId);
            }
            else if (Port.IsPortSignal(port)) {
                return new TrafficLightVertex(vertexId);
            }
            else if (Port.IsPortConnection(port)) {
                return new ConnectionVertex(vertexId);
            }
            else if (Port.IsPortSwitch(port)) {
                return new SwitchVertex(vertexId);
            }

            throw new ArgumentException("A port " + port.GetHashCode() + " could not be mapped to vertex, because it is not an Input, Output, Deadend, Traffic light, Connection or Switch port. Thus it cannote be included in station graph");
        }
        public static Tuple<StationGraph, Dictionary<int, TopologyItem>, Dictionary<int, StraightRailTrackItem>, Dictionary<int, Vertex>> CreateGraph(RailwayCanvasViewModel canvas) {
            return CreateGraph(canvas.Rails);
        }
        public static void AddTopologyFromGraph(RailwayCanvasViewModel canvas, StationGraph graph) {
            Point currentStart = new Point(0, 0) {
                X = _initGraphPos.X,
                Y = _initGraphPos.Y
            };
            Point currentEnd = new Point(0, 0) {
                X = _initGraphPos.X + 100,
                Y = _initGraphPos.Y
            };

            Dictionary<Edge, StraightRailTrackItem> railDict = [];
            Dictionary<int, Edge> edgesDict = graph.GetEdges().ToDictionary(x => x.getId(), x => x);
            Dictionary<int, Vertex> vertices = graph.GetVertices().ToDictionary(x => x.getId(), x => x);

            // add rails without connecting them yet
            foreach (Edge edge in edgesDict.Values) {
                int length = edge.GetLength();
                StraightRailTrackItem srtItem = new StraightRailTrackItem(_initGraphPos, length);
                switch (edge.GetEdgeType()) {
                    case TrainType.PASSENGER:
                        srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.PASSENGER;
                        break;
                    case TrainType.CARGO:
                        srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.CARGO;
                        break;
                }

                srtItem.PlaceStartPoint(currentStart);
                srtItem.PlaceEndPoint(currentEnd);
                currentStart.Y += 10;
                currentEnd.Y += 10;
                railDict[edge] = srtItem;
                srtItem.Id = edge.getId();

                canvas.AddTopologyItemBehind(srtItem);
            }

            // connect rails using info from edgeConnections
            // only non input vertices can have their starts moved
            foreach (Vertex vertex in vertices.Values) {
                Port connectionPort;
                switch (vertex.GetVertexType()) {
                    case VertexType.INPUT:
                        Edge inputEdge = vertex.GetEdgeConnections()[0].Item1 == null ? vertex.GetEdgeConnections()[0].Item2 : vertex.GetEdgeConnections()[0].Item1;
                        StraightRailTrackItem srtItem = railDict[inputEdge];

                        ExternalTrackItem externalTrackItem = new ExternalTrackItem(srtItem.Start) {
                            IsBroken = vertex.IsBlocked(),
                            Type = ExternalTrackItem.ExternalTrackType.IN
                        };
                        connectionPort = HasItems(srtItem.PortEnd) ? srtItem.PortStart : srtItem.PortEnd;
                        canvas.AddTopologyItem(externalTrackItem);
                        externalTrackItem.Place(connectionPort);
                        externalTrackItem.Id = vertex.getId();
                        break;
                    case VertexType.OUTPUT:
                        Edge outputEdge = vertex.GetEdgeConnections()[0].Item1 == null ? vertex.GetEdgeConnections()[0].Item2 : vertex.GetEdgeConnections()[0].Item1;
                        srtItem = railDict[outputEdge];

                        externalTrackItem = new ExternalTrackItem(srtItem.End) {
                            IsBroken = vertex.IsBlocked(),
                            Type = ExternalTrackItem.ExternalTrackType.OUT
                        };
                        connectionPort = HasItems(srtItem.PortEnd) ? srtItem.PortStart : srtItem.PortEnd;
                        canvas.AddTopologyItem(externalTrackItem);
                        externalTrackItem.Place(connectionPort);
                        externalTrackItem.Id = vertex.getId();
                        break;
                    case VertexType.DEADEND:
                        Edge deadendEdge = vertex.GetEdgeConnections()[0].Item1 == null ? vertex.GetEdgeConnections()[0].Item2 : vertex.GetEdgeConnections()[0].Item1;
                        srtItem = railDict[deadendEdge];

                        DeadendItem deadendItem = new DeadendItem(srtItem.End) {
                            IsBroken = vertex.IsBlocked(),
                        };
                        connectionPort = HasItems(srtItem.PortEnd) ? srtItem.PortStart : srtItem.PortEnd;
                        canvas.AddTopologyItem(deadendItem);
                        deadendItem.Place(connectionPort);
                        deadendItem.Id = vertex.getId();
                        break;
                    case VertexType.CONNECTION:
                        Edge edgeOne = vertex.GetEdgeConnections()[0].Item1;
                        Edge edgeTwo = vertex.GetEdgeConnections()[0].Item2;

                        StraightRailTrackItem srtOne = railDict[edgeOne];
                        StraightRailTrackItem srtTwo = railDict[edgeTwo];

                        connectionPort = HasItems(srtOne.PortEnd) ? srtOne.PortStart : srtOne.PortEnd;
                        Point endPos = new Point(
                            connectionPort.Pos.X + srtTwo.Length,
                            connectionPort.Pos.Y
                            );
                        ConnectRail(srtTwo, connectionPort, endPos);
                        break;
                    case VertexType.TRAFFIC:
                        #region repeat
                        edgeOne = vertex.GetEdgeConnections()[0].Item1;
                        edgeTwo = vertex.GetEdgeConnections()[0].Item2;

                        srtOne = railDict[edgeOne];
                        srtTwo = railDict[edgeTwo];

                        connectionPort = HasItems(srtOne.PortEnd) ? srtOne.PortStart : srtOne.PortEnd;
                        endPos = new Point(
                            connectionPort.Pos.X + srtTwo.Length,
                            connectionPort.Pos.Y
                            );
                        ConnectRail(srtTwo, connectionPort, endPos);
                        #endregion

                        SignalItem signalItem = new SignalItem(srtOne.End) {
                            IsBroken = vertex.IsBlocked()
                        };
                        canvas.AddTopologyItem(signalItem);
                        signalItem.Place(connectionPort);
                        signalItem.Id = vertex.getId();
                        break;
                    case VertexType.SWITCH:
                        edgeOne = vertex.GetEdgeConnections()[0].Item1;
                        edgeTwo = vertex.GetEdgeConnections()[0].Item2;
                        Edge edgeThree = vertex.GetEdgeConnections()[1].Item1;
                        Edge edgeFour = vertex.GetEdgeConnections()[1].Item2;
                        Edge edgeSrc, edgeDstOne, edgeDstTwo;
                        if(edgeOne == edgeThree || edgeOne == edgeFour) {
                            edgeSrc = edgeOne;
                            edgeDstOne = edgeTwo;
                            edgeDstTwo = edgeSrc == edgeThree ? edgeFour : edgeThree;
                        }
                        else {
                            edgeSrc = edgeTwo;
                            edgeDstOne = edgeOne;
                            edgeDstTwo = edgeSrc == edgeThree ? edgeFour : edgeThree;
                        }

                        StraightRailTrackItem srtSrc = railDict[edgeSrc];
                        srtOne = railDict[edgeDstOne];
                        srtTwo = railDict[edgeDstTwo];

                        connectionPort = HasItems(srtSrc.PortEnd) ? srtSrc.PortStart : srtSrc.PortEnd;
                        endPos = new Point(
                            connectionPort.Pos.X + srtOne.Length,
                            connectionPort.Pos.Y
                            );
                        ConnectRail(srtOne, connectionPort, endPos);

                        endPos = new Point(
                            connectionPort.Pos.X + srtTwo.Length,
                            connectionPort.Pos.Y + srtTwo.Length
                            );
                        ConnectRail(srtTwo, connectionPort, endPos);

                        SwitchItem switchItem = new SwitchItem(srtSrc.End) {
                            IsBroken = vertex.IsBlocked(),
                        };
                        canvas.AddTopologyItem(switchItem);
                        switchItem.Place(connectionPort);
                        switchItem.SetSource(srtSrc.GetOtherPort(connectionPort));
                        switchItem.Id = vertex.getId();
                        break;
                    default:
                        throw new ArgumentException("Wrong train type");
                }
                connectionPort.Id = vertex.getId();
            }

            canvas.ResetLatestTopologyItem();
        }

        private static bool HasItems(Port port) {
            int totalItems = port.TopologyItems.Count();
            return totalItems - port.TopologyItems.OfType<StraightRailTrackItem>().Count() != 0;
        }

        private static void ConnectRail(StraightRailTrackItem srt, Port port, Point otherPos) {
            if (HasItems(srt.PortStart)) {
                // start is occupied -> connect by end
                srt.PlaceEndPoint(port);
                srt.PlaceStartPoint(otherPos);
            }
            else {
                // end is occupied -> connect by start
                srt.PlaceStartPoint(port);
                srt.PlaceEndPoint(otherPos);
            }
        }
    }
}
