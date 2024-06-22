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
                        #region Set edgeConnection considering srt direction
                        if (portSrts[0].StartsFromStart) {
                            if (portSrts[0].PortStart == port) {
                                // the port is start to 0th edge. Thus this edge is outgoing
                                connectionVertex.SetEdges(edgesDict[portSrts[1]], // incoming
                                                          edgesDict[portSrts[0]]);// outgoing
                            }
                            else {
                                connectionVertex.SetEdges(edgesDict[portSrts[0]], // incoming
                                                          edgesDict[portSrts[1]]);// outgoing
                            }
                        }
                        else {
                            if (portSrts[0].PortStart == port) {
                                // the port is end to 0th edge. Thus this edge is incoming
                                connectionVertex.SetEdges(edgesDict[portSrts[0]], // incoming
                                                          edgesDict[portSrts[1]]);// outgoing
                            }
                            else {
                                connectionVertex.SetEdges(edgesDict[portSrts[1]], // incoming
                                                          edgesDict[portSrts[0]]);// outgoing
                            }
                        }
                        #endregion
                        if (portSrts[0].IsBroken || portSrts[1].IsBroken) connectionVertex.Block();
                        break;
                    case DeadEndVertex deadEndVertex:
                        deadEndVertex.SetEdge(edgesDict[port.TopologyItems.OfType<StraightRailTrackItem>().First()]);
                        relatedItem = port.TopologyItems.OfType<DeadendItem>().First();
                        topologyDict[deadEndVertex.getId()] = relatedItem;
                        if (relatedItem.IsBroken) deadEndVertex.Block();
                        break;
                    case TrafficLightVertex trafficLightVertex:
                        #region Set edgeConnection considering srt direction
                        if (portSrts[0].StartsFromStart) {
                            if (portSrts[0].PortStart == port) {
                                // the port is start to 0th edge. Thus this edge is outgoing
                                trafficLightVertex.SetEdges(edgesDict[portSrts[1]], // incoming
                                                          edgesDict[portSrts[0]]);// outgoing
                            }
                            else {
                                trafficLightVertex.SetEdges(edgesDict[portSrts[0]], // incoming
                                                          edgesDict[portSrts[1]]);// outgoing
                            }
                        }
                        else {
                            if (portSrts[0].PortStart == port) {
                                // the port is end to 0th edge. Thus this edge is incoming
                                trafficLightVertex.SetEdges(edgesDict[portSrts[0]], // incoming
                                                          edgesDict[portSrts[1]]);// outgoing
                            }
                            else {
                                trafficLightVertex.SetEdges(edgesDict[portSrts[1]], // incoming
                                                            edgesDict[portSrts[0]]);// outgoing
                            }
                        }
                        #endregion

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
            Edge[] edges = [.. graph.GetEdges()];

            // add rails without connecting them yet
            foreach (Edge edge in edges) {
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

                canvas.AddTopologyItemBehind(srtItem);
            }

            // connect start of rails
            // only non input vertices can have their starts moved
            foreach (Edge edge in edges) {
                if (edge.GetStart() is InputVertex) continue;
                Edge incomingEdge = edges.First(x => x.GetEnd().getId() == edge.GetStart().getId());
                Port newPort = railDict[incomingEdge].MovementPortEnd;
                StraightRailTrackItem srtItem = railDict[edge];
                
                // connect starting points
                srtItem.PlaceStartPoint(newPort);

                // place end point
                Point endPos = new Point(0, 0);
                if (newPort.TopologyItems.OfType<StraightRailTrackItem>().Count() == 2) {
                    // it's first outgoing track
                    endPos.X = newPort.Pos.X + srtItem.Length;
                    endPos.Y = newPort.Pos.Y;
                }
                else {
                    // it's second outgoing track
                    endPos.X = newPort.Pos.X + srtItem.Length;
                    endPos.Y = newPort.Pos.Y + srtItem.Length;
                }
                srtItem.PlaceEndPoint(endPos);
            }

            // register and connect units
            foreach (Edge edge in edges) {
                StraightRailTrackItem srtItem = railDict[edge];
                // Handle start vertex
                Vertex? startVertex = edge.GetStart();
                if (startVertex != null && startVertex is not ConnectionVertex && Port.IsPortConnection(srtItem.MovementPortStart)) {
                    // model-wise vertex is not simple connection, but graphic-wise it still is
                    AddTopologyItem(canvas, srtItem.MovementPortStart, startVertex);
                }
                // Handle end vertex
                Vertex? endVertex = edge.GetEnd();
                if (endVertex != null && endVertex is not ConnectionVertex && Port.IsPortConnection(srtItem.MovementPortEnd)) {
                    // model-wise vertex is not simple connection, but graphic-wise it still is
                    AddTopologyItem(canvas, srtItem.MovementPortEnd, endVertex);
                }
            }

            canvas.ResetLatestTopologyItem();
        }

        private static void AddTopologyItem(RailwayCanvasViewModel canvas, Port port, Vertex vertex) {
            switch (vertex) {
                case InputVertex inputVertex:
                    ExternalTrackItem externalTrackItem = new ExternalTrackItem(port.Pos) {
                        IsBroken = inputVertex.IsBlocked(),
                        Type = ExternalTrackItem.ExternalTrackType.IN
                    };
                    canvas.AddTopologyItem(externalTrackItem);
                    externalTrackItem.Place(port);
                    break;
                case OutputVertex outputVertex:
                    externalTrackItem = new ExternalTrackItem(port.Pos) {
                        IsBroken = outputVertex.IsBlocked(),
                        Type = ExternalTrackItem.ExternalTrackType.OUT
                    };
                    canvas.AddTopologyItem(externalTrackItem);
                    externalTrackItem.Place(port);
                    break;
                case DeadEndVertex deadEndVertex:
                    DeadendItem deadendItem = new DeadendItem(port.Pos) {
                        IsBroken = deadEndVertex.IsBlocked(),
                    };
                    canvas.AddTopologyItem(deadendItem); 
                    deadendItem.Place(port); 
                    break;
                case SwitchVertex switchVertex:
                    SwitchItem switchItem = new SwitchItem(port.Pos) {
                        IsBroken = switchVertex.IsBlocked(),
                    };
                    canvas.AddTopologyItem(switchItem);
                    switchItem.Place(port);
                    break;
                case TrafficLightVertex trafficLightVertex:
                    SignalItem signalItem = new SignalItem(port.Pos) {
                        IsBroken = trafficLightVertex.IsBlocked()
                    };
                    canvas.AddTopologyItem(signalItem);
                    signalItem.Place(port);
                    break;
            }
        }
    }
}
