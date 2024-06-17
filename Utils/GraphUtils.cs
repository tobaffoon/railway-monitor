using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using SolverLibrary.Model.Graph;
using SolverLibrary.Model.Graph.VertexTypes;
using SolverLibrary.Model.TrainInfo;

namespace railway_monitor.Utils {
    public class GraphUtils {
        public static StationGraph CreateGraph(List<StraightRailTrackItem> rails) {
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
                if (vertexDict.ContainsKey(portStart)) {
                    vertexStart = vertexDict[portStart];
                }
                else {
                    vertexStart = CreateVertexFromPort(portStart, vertexIdCounter);
                    vertexIdCounter++;
                }
                // add ending port as vertex, if not present
                if (vertexDict.ContainsKey(portEnd)) {
                    vertexEnd = vertexDict[portEnd];
                }
                else {
                    vertexEnd = CreateVertexFromPort(portEnd, vertexIdCounter);
                    vertexIdCounter++;
                }

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

                edgesDict[srt] = new Edge(edgeIdCounter, 1, vertexStart, vertexEnd, edgeType);
                edgeIdCounter++;
            }

            // add edges links to each vertex
            Edge curEdge;
            foreach (Port port in vertexDict.Keys) {
                switch (vertexDict[port]) {
                    case InputVertex inputVertex:
                        inputVertex.SetEdge(edgesDict[port.GraphicItems.OfType<StraightRailTrackItem>().First()]);
                        break;
                    case OutputVertex outputVertex:
                        outputVertex.SetEdge(edgesDict[port.GraphicItems.OfType<StraightRailTrackItem>().First()]);
                        break;
                    case ConnectionVertex connectionVertex:
                        break;
                }
            }
        }
        #region Port types
        private static bool IsPortInput(Port port) {
            return port.GraphicItems.OfType<ExternalTrackItem>().Where(externalItem  => externalItem.Type == ExternalTrackItem.ExternalTrackType.IN).Any();
        }
        private static bool IsPortOutput(Port port) {
            return port.GraphicItems.OfType<ExternalTrackItem>().Where(externalItem  => externalItem.Type == ExternalTrackItem.ExternalTrackType.OUT).Any();
        }
        private static bool IsPortConnection(Port port) {
            return port.GraphicItems.OfType<StraightRailTrackItem>().Count() == port.GraphicItems.Count;
        }
        private static bool IsPortDeadend(Port port) {
            return port.GraphicItems.OfType<DeadendItem>().Any();
        }
        private static bool IsPortSwitch(Port port) {
            return port.GraphicItems.OfType<SwitchItem>().Any();
        }
        private static bool IsPortTrafficLight(Port port) {
            return port.GraphicItems.OfType<SignalItem>().Any();
        }
        #endregion

        private static Vertex CreateVertexFromPort(Port port, int vertexId) {
            if (IsPortInput(port)) {
                return new InputVertex(vertexId);
            }
            else if (IsPortOutput(port)) {
                return new OutputVertex(vertexId);
            }
            else if (IsPortDeadend(port)) {
                return new DeadEndVertex(vertexId);
            }
            else if (IsPortTrafficLight(port)) {
                return new TrafficLightVertex(vertexId);
            }
            else if (IsPortConnection(port)) {
                return new ConnectionVertex(vertexId);
            }
            else if (IsPortSwitch(port)) {
                return new SwitchVertex(vertexId);
            }

            throw new ArgumentException("A port " + port.GetHashCode() + " could not be mapped to vertex, because it is not an Input, Output, Deadend, Traffic light, Connection or Switch port. Thus it cannote be included in station graph");
        }
    }
}
