using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BFSService.Models;
using BFSService.Services;
using EdmondsKarpService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BFSService.Controllers
{
    [ApiController]
    [Route("bfs")]
    public class BFSController : ControllerBase
    {
        private IBfService _bfSservice;
        private readonly IHttpClientFactory _httpClientFactory;

        public BFSController(IBfService bfSservice)
        {
            _bfSservice = bfSservice;
        }
        
        [HttpGet]
        public async Task<ActionResult<BFSResult>> Bfs([FromQuery] GraphParametersFlow graphParametersFlow)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://graph-service:80/graph?id={graphParametersFlow.graph.Id}");
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.SendAsync(request);

            if (responseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await responseMessage.Content.ReadAsStreamAsync();
                Graph? graph = await JsonSerializer.DeserializeAsync<Graph>(contentStream);
                if (graph == null)
                {
                    return NotFound();
                }
                if (!areSourceAndGraphParametersValid(graph, graphParametersFlow.source, graphParametersFlow.destination))
                {
                    throw new Exception($"Invalid source or destination parameter!\n " +
                                        $"Number of vertices: {graph.NumberOfVertices}\n " +
                                        $"Source: {graphParametersFlow.source}\n Destination: {graphParametersFlow.destination}\n");
                }

                return _bfSservice.Bfs(graph, graphParametersFlow.source, graphParametersFlow.destination);
            }

            return NotFound();
        }
        
        private bool areSourceAndGraphParametersValid(Graph? graph, int source, int destination) {
            return source >= 0 && source < graph.NumberOfVertices && destination >= 0 && destination < graph.NumberOfVertices;
        }
    }
}