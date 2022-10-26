using System.Text.Json.Serialization;

namespace CommonModels;

[Serializable]
public class Graph
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

    public object DeepCopy()
    {
        Graph graph = (Graph)MemberwiseClone();
        graph.Id = Id;
        graph.NumberOfVertices = NumberOfVertices;
        graph.AdjacencyMatrix = AdjacencyMatrix.Select(a => a.ToArray()).ToArray();
        return graph;
    }
}
