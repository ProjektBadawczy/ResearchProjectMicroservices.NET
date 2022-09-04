using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
                 var requestBFS = new HttpRequestMessage(HttpMethod.Get, $"http://bfs-service:80/graph?" +
                                                                 $"graph={graph}" +
                                                                 $"&source={source}" +
                                                                 $"&destination={destination}");
                 
                var responseBFS = await client.SendAsync(requestBFS);

                if (responseBFS.IsSuccessStatusCode)
                {
                    using var contentStreamBFS = await responseBFS.Content.ReadAsStreamAsync();
                    BFSResult bfsResult = await JsonSerializer.DeserializeAsync<BFSResult>(contentStreamBFS);
                    
                    int u, v;
                    Graph residualGraph = (Graph)graph.Clone();
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
                        requestBFS = new HttpRequestMessage(HttpMethod.Get, $"http://bfs-service:80/graph?" +
                                                                            $"graph={residualGraph}" +
                                                                            $"&source={source}" +
                                                                            $"&destination={destination}");
                        responseBFS = await client.SendAsync(requestBFS);
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
        
        private bool areSourceAndGraphParametersValid(Graph graph, int source, int destination) {
            return source >= 0 && source < graph.NumberOfVertices && destination >= 0 && destination < graph.NumberOfVertices;
        }
    }
}