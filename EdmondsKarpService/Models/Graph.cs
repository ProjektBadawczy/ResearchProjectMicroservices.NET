using System;
using System.Text.Json.Serialization;

namespace EdmondsKarpService.Models
{
    [Serializable]
    public class Graph : ICloneable
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("numberOfVertices")]
        public int NumberOfVertices { get; set; }
        
        [JsonPropertyName("adjacencyMatrix")]
        public int[][] AdjacencyMatrix { get; set; }
        
        public Graph(int id, int numberOfVertices, int [][] adjacencyMatrix)
        {
            Id = id;
            NumberOfVertices = numberOfVertices;
            AdjacencyMatrix = adjacencyMatrix;

        }

        public object Clone()
        {
            return (Graph)MemberwiseClone();
        }
    }
}