﻿using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using SolverLibrary.Model.Graph;
using SolverLibrary.Model.Graph.VertexTypes;
using SolverLibrary.Model.TrainInfo;

namespace railway_monitor.Utils {
    public class GraphUtils {
        private static int _defaultEdgeLength = 1;

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

                edgesDict[srt] = new Edge(edgeIdCounter, _defaultEdgeLength, vertexStart, vertexEnd, edgeType);
                edgeIdCounter++;
            }

            // add edges links to each vertex
            StraightRailTrackItem[] portSrts;
            foreach (Port port in vertexDict.Keys) {
                portSrts = port.GraphicItems.OfType<StraightRailTrackItem>().ToArray();
                switch (vertexDict[port]) {
                    case InputVertex inputVertex:
                        inputVertex.SetEdge(edgesDict[port.GraphicItems.OfType<StraightRailTrackItem>().First()]);
                        break;
                    case OutputVertex outputVertex:
                        outputVertex.SetEdge(edgesDict[port.GraphicItems.OfType<StraightRailTrackItem>().First()]);
                        break;
                    case ConnectionVertex connectionVertex:
                        #region Set edgeConnection considering srt direction
                        if (portSrts[0].StartsFromStart) {
                            if (portSrts[0].PortStart == port) {
                                // the port is start to 0th edge. Thus this edge is outgoing
                                connectionVertex.SetEdges(edgesDict[portSrts[1]], // incoming
                                                          edgesDict[portSrts[0]]);// outgoing
                            }
                            else
                            {
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
                        break;
                    case DeadEndVertex deadEndVertex:
                        deadEndVertex.SetEdge(edgesDict[port.GraphicItems.OfType<StraightRailTrackItem>().First()]);
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
                        break;
                    case SwitchVertex switchVertex:
                        SwitchItem switchItem = port.GraphicItems.OfType<SwitchItem>().First();
                        StraightRailTrackItem? sourceRail = portSrts.Where(srt => srt.PortStart == switchItem.PortSrc || srt.PortEnd == switchItem.PortSrc).FirstOrDefault();
                        StraightRailTrackItem? dstOneRail = portSrts.Where(srt => srt.PortStart == switchItem.PortDstOne || srt.PortEnd == switchItem.PortDstOne).FirstOrDefault();
                        StraightRailTrackItem? dstTwoRail = portSrts.Where(srt => srt.PortStart == switchItem.PortDstTwo || srt.PortEnd == switchItem.PortDstTwo).FirstOrDefault();
                        if (sourceRail == null || dstOneRail == null || dstTwoRail == null) {
                            throw new ArgumentException("Error retrieving srts connected to a switch");
                        }

                        switchVertex.SetEdges(edgesDict[sourceRail], edgesDict[dstOneRail], edgesDict[dstTwoRail]);
                        break;
                }
            }

            StationGraph graph = new StationGraph();
            foreach (Vertex v in vertexDict.Values) {
                graph.TryAddVerticeWithEdges(v);
            }
            return graph;
        }
        #region Port types
        private static void SetEdgesOfConnection(Vertex vertex, Port port, Dictionary<StraightRailTrackItem, Edge> edgesDict) {

        }
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
