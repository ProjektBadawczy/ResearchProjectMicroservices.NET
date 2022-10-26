using System.Text.Json.Serialization;
using CommonModels;

namespace GraphService.Models;

[Serializable]
public class JSONGraphFormat
{
    [JsonPropertyName("graph")]
    public Graph graph { get; set; }
    
    [JsonPropertyName("directedGraph")]
    public DirectedGraph directedGraph { get; set; }
}