using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CommonModels;

namespace EdmondsKarpService.Services
{
    public interface IEdmondsKarpService
    {
        public int CalculateMaxFlow(int id, int source, int destination);

    }
    
    public class EdmondsKarpService: IEdmondsKarpService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        
        public EdmondsKarpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public int CalculateMaxFlow(int id, int source, int destination)
        {
            var client = _httpClientFactory.CreateClient();
            var requestGraph = new HttpRequestMessage(HttpMethod.Get, $"http://graph-service:80/graph?id={id}");
            var responseGraph = client.Send(requestGraph);

            if (responseGraph.IsSuccessStatusCode)
            {
                using var contentStreamGraph = responseGraph.Content.ReadAsStream();
                Graph? graph = JsonSerializer.Deserialize<Graph>(contentStreamGraph);
                GraphForBFS graphForBfs = new GraphForBFS(graph.Id, graph.NumberOfVertices, graph.AdjacencyMatrix,
                    source, destination);

                var requestBFS = "http://bfs-service:80/bfs";
                var httpContent = new StringContent(JsonSerializer.Serialize(graphForBfs), Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                var responseBFS = client.PostAsync(requestBFS, httpContent);

                if (responseBFS.Result.IsSuccessStatusCode)
                {
                    using var contentStreamBFS = responseBFS.Result.Content.ReadAsStream();
                    BFSResult bfsResult = JsonSerializer.Deserialize<BFSResult>(contentStreamBFS);

                    int u, v;
                    GraphForBFS residualGraph = (GraphForBFS) graphForBfs.DeepCopy();
                    int maxFlow = 0;

                    while (bfsResult.Success)
                    {
                        int pathFlow = Int32.MaxValue;
                        for (v = destination; v != source; v = bfsResult.Parents[v])
                        {
                            u = bfsResult.Parents[v];
                            pathFlow = Math.Min(pathFlow, residualGraph.AdjacencyMatrix[u][v]);
                        }

                        for (v = destination; v != source; v = bfsResult.Parents[v])
                        {
                            u = bfsResult.Parents[v];
                            residualGraph.AdjacencyMatrix[u][v] -= pathFlow;
                            residualGraph.AdjacencyMatrix[v][u] += pathFlow;
                        }

                        maxFlow += pathFlow;
                        httpContent = new StringContent(JsonSerializer.Serialize(residualGraph), Encoding.UTF8,
                            MediaTypeNames.Application.Json);
                        responseBFS = client.PostAsync(requestBFS, httpContent);
                        if (responseBFS.Result.IsSuccessStatusCode)
                        {
                            using var newContentStream = responseBFS.Result.Content.ReadAsStream();
                            bfsResult = JsonSerializer.Deserialize<BFSResult>(newContentStream);
                        }
                    }

                    return maxFlow;
                }
            }
            return default;
        }
    }
}