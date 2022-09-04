using System.Collections.Generic;
using GraphService.Models;

namespace GraphService.Services
{
    public interface IGraphService
    {
        public Graph getGraph(int id);

        public DirectedGraph getDirectedGraph(int id);

    }
    public class GraphService: IGraphService
    {
        private List<Graph> graphs;
        private List<DirectedGraph> directedGraphs;
        
        public GraphService() {
            graphs = new List<Graph>();
            directedGraphs = new List<DirectedGraph>();
            
            /*int[][] adjMatrix = {
                new [] {0, 2, 3, 4},
                new [] {1, 0, 3, 4},
                new [] {1, 2, 0, 4},
                new[]{1, 2, 3, 0}};*/
            int[][] adjMatrix =
            {
                new[] {0, 16, 13, 0, 0, 0},
                new[] {0, 0, 10, 12, 0, 0},
                new[] {0, 4, 0, 0, 14, 0},
                new[] {0, 0, 9, 0, 0, 20},
                new[] {0, 0, 0, 7, 0, 4},
                new[] {0, 0, 0, 0, 0, 0}
            };
            Graph graph = new Graph(1, 6, adjMatrix);
            graphs.Add(graph);
            
            int vertices = 6;
            int source = 0;
            int sink = 5;
 
            DirectedGraph dg = new DirectedGraph(vertices, 0);
 
            dg.AddEdge(0, 1, 16);
            dg.AddEdge(0, 2, 13);
            dg.AddEdge(1, 2, 10);
            dg.AddEdge(2, 1, 4);
            dg.AddEdge(1, 3, 12);
            dg.AddEdge(3, 2, 9);
            dg.AddEdge(2, 4, 14);
            dg.AddEdge(4, 5, 4);
            dg.AddEdge(4, 3, 7);
            dg.AddEdge(3, 5, 20);
            directedGraphs.Add(dg);
        }

        public Graph getGraph(int id)
        {
            var graph = graphs.Find(x => x.Id == id);
            return graph;
        }
        
        public DirectedGraph getDirectedGraph(int id)
        {
            var graph = directedGraphs.Find(x => x.Id == id);
            return graph;
        }
        
    }
}