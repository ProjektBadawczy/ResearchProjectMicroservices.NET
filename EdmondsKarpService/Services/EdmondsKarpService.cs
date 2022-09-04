using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BFSService.Models;
using EdmondsKarpService.Models;

namespace EdmondsKarpService.Services
{
    public interface IEdmondsKarpService
    {
        public Task<int> CalculateMaxFlow(int id, int source, int destination);

    }
    
    public class EdmondsKarpService: IEdmondsKarpService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        
        public EdmondsKarpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> CalculateMaxFlow(int id, int source, int destination)
        {
            var client = _httpClientFactory.CreateClient();
            var requestGraph = new HttpRequestMessage(HttpMethod.Get, $"http://graph-service:80/graph?id={id}");
            var responseGraph = await client.SendAsync(requestGraph);

            if (responseGraph.IsSuccessStatusCode)
            {
                using var contentStreamGraph = await responseGraph.Content.ReadAsStreamAsync();
                Graph? graph = await JsonSerializer.DeserializeAsync<Graph>(contentStreamGraph);
                GraphForBFS graphForBfs = new GraphForBFS(graph.Id, graph.NumberOfVertices, graph.AdjacencyMatrix,
                    source, destination);

                var requestBFS = "http://bfs-service:80/bfs";
                var httpContent = new StringContent(JsonSerializer.Serialize(graphForBfs), Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                var responseBFS = await client.PostAsync(requestBFS, httpContent);

                if (responseBFS.IsSuccessStatusCode)
                {
                    using var contentStreamBFS = await responseBFS.Content.ReadAsStreamAsync();
                    BFSResult bfsResult = await JsonSerializer.DeserializeAsync<BFSResult>(contentStreamBFS);

                    int u, v;
                    GraphForBFS residualGraph = (GraphForBFS) graphForBfs.Clone();
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
                        responseBFS = await client.PostAsync(requestBFS, httpContent);
                        if (responseBFS.IsSuccessStatusCode)
                        {
                            using var newContentStream = await responseBFS.Content.ReadAsStreamAsync();
                            bfsResult = await JsonSerializer.DeserializeAsync<BFSResult>(newContentStream);
                        }
                    }

                    return maxFlow;
                }
            }
            return default;
        }
    }
}