using CommonModels;
using EdmondsKarpService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EdmondsKarpService.Controllers
{
    [ApiController]
    [Route("edmondsKarpMaxGraphFlow")]
    public class EdmondsKarpController : ControllerBase
    {
        
        private IEdmondsKarpService _edmondsKarpService;
        
        public EdmondsKarpController(IEdmondsKarpService edmondsKarpService)
        {
            _edmondsKarpService = edmondsKarpService;
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetEdmondsKarpMaxGraphFlow([FromQuery] GraphParametersFlowWithId graphParametersFlowWithId)
        {
            int maxFlow = await _edmondsKarpService.CalculateMaxFlow(graphParametersFlowWithId.id, graphParametersFlowWithId.source, 
                graphParametersFlowWithId.destination);

            return maxFlow;
        }

    }
}