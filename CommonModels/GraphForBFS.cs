using System.Text.Json.Serialization;

namespace CommonModels;

[Serializable]
public class GraphForBFS
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("numberOfVertices")] public int NumberOfVertices { get; set; }

    [JsonPropertyName("adjacencyMatrix")] public int[][] AdjacencyMatrix { get; set; }

    [JsonPropertyName("source")] public int Source { get; set; }

    [JsonPropertyName("destination")] public int Destination { get; set; }

    public GraphForBFS(int id, int numberOfVertices, int[][] adjacencyMatrix, int source, int destination)
    {
        Id = id;
        NumberOfVertices = numberOfVertices;
        AdjacencyMatrix = adjacencyMatrix;
        Source = source;
        Destination = destination;
    }

    public object DeepCopy()
    {
        GraphForBFS graph = (GraphForBFS)MemberwiseClone();
        graph.Id = Id;
        graph.NumberOfVertices = NumberOfVertices;
        graph.AdjacencyMatrix = AdjacencyMatrix.Select(a => a.ToArray()).ToArray();
        graph.Source = Source;
        graph.Destination = Destination;
        return graph;
    }

}