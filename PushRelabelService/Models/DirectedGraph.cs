using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PushRelabelService.Models
{
    // DirectedGraph class explained above
    [Serializable]
    public class DirectedGraph
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("vertices")]
        public int Vertices;
        
        [JsonPropertyName("adjacencyList")]
        public readonly List<List<Vertex>> AdjacencyList;
        
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
            foreach (Vertex vertex in AdjacencyList[u])
            {
                if (vertex.i == v)
                    return vertex;
            }

            return null;
        }
    }
}