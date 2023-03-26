using System.Text.Json;
using GraphService.Models;

namespace GraphService.Repositories;

public class GraphRepository
{
    public List<JSONGraphFormat> Graphs { get; set; }
    
    private List<string> fileNames = new List<string>()
    {
        "random_set.json"
    };

    public GraphRepository()
    {
        string fileName = fileNames[0];
        string jsonString = File.ReadAllText("./Resources/" + fileName);
        Graphs = JsonSerializer.Deserialize<List<JSONGraphFormat>>(jsonString)!;
    }
}