using CommonModels;
using GraphService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphService.Controllers
{
    [ApiController]
    public class GraphController : ControllerBase
    {
        private IGraphService _graphService;
        
        public GraphController(IGraphService graphService)
        {
            _graphService = graphService;
        }
        
        [HttpGet]
        [Route("graph")]
        public ActionResult<Graph> GetGraph([FromQuery] int id)
        {
            Graph graph = _graphService.getGraph(id);
            if (graph != null)
            {
                return graph;
            }

            return NotFound();
        }
        
        [HttpGet]
        [Route("directedGraph")]
        public ActionResult<DirectedGraph> GetDirectedGraph([FromQuery] int id)
        {
            DirectedGraph directedGraph = _graphService.getDirectedGraph(id);
            if (directedGraph != null)
            {
                return directedGraph;
            }

            return NotFound();
        }
    }
}