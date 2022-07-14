using System;
using System.Collections.Generic;

namespace GraphService.Models
{
    // DirectedGraph class explained above
    [Serializable]
    public class DirectedGraph
    {
        public class Vertex
        {

            // number of the end vertex
            // weight or capacity
            // associated with the edge

            public int i;
            public int w;

            public Vertex(int i, int w)
            {
                this.i = i;
                this.w = w;
            }
        }

        readonly public List<List<Vertex>> AdjacencyList;

        public int Vertices;
        public int Id { get; set; }

        public DirectedGraph(int vertices, int id)
        {
            Id = id;

            Vertices = vertices;

            AdjacencyList = new List<List<Vertex>>(vertices);

            for (int i = 0; i < vertices; i++)
            {
                AdjacencyList.Add(new List<Vertex>());
            }
        }

        public void AddEdge(int u, int v, int weight)
        {
            AdjacencyList[u].Add(new Vertex(v, weight));
        }

        public bool HasEdge(int u, int v)
        {
            if (u >= Vertices)
                return false;

            foreach (Vertex vertex in AdjacencyList[u])
                if (vertex.i == v)
                    return true;
            return false;
        }

        // Returns null if no edge
        // is found between u and v
        public Vertex GetEdge(int u, int v)
        {
            foreach (DirectedGraph.Vertex vertex in AdjacencyList[u])
            {
                if (vertex.i == v)
                    return vertex;
            }

            return null;
        }
    }
}