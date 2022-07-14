using System;

namespace GraphService.Models
{
    [Serializable]
    public class Graph : ICloneable
    {
        public int Id { get; set; }
        public int NumberOfVertices { get; set; }
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