using BFSService.Services;
using CommonModels;
using Microsoft.AspNetCore.Mvc;

namespace BFSService.Controllers
{
    [ApiController]
    [Route("bfs")]
    public class BFSController : ControllerBase
    {
        private IBfService _bfSservice;

        public BFSController(IBfService bfSservice)
        {
            _bfSservice = bfSservice;
        }
        
        [HttpPost]
        public async Task<ActionResult<BFSResult>> Bfs([FromBody] GraphForBFS graphForBFS)
        {
            if (graphForBFS == null)
            {
                return NotFound();
            }
            if (!areSourceAndGraphParametersValid(graphForBFS, graphForBFS.Source, graphForBFS.Destination))
            {
                throw new Exception($"Invalid source or destination parameter!\n " +
                                    $"Number of vertices: {graphForBFS.NumberOfVertices}\n " +
                                    $"Source: {graphForBFS.Source}\n Destination: {graphForBFS.Destination}\n");
            }
            return _bfSservice.Bfs(graphForBFS);
        }
        
        private bool areSourceAndGraphParametersValid(GraphForBFS? graph, int source, int destination) {
            return source >= 0 && source < graph.NumberOfVertices && destination >= 0 && destination < graph.NumberOfVertices;
        }
    }
}