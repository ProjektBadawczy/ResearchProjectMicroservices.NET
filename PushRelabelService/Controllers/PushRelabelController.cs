using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PushRelabelService.Models;
using PushRelabelService.Services;

namespace PushRelabelService.Controllers
{
    
    [ApiController]
    [Route("pushRelabelMaxGraphFlow")]
    public class PushRelabelController : ControllerBase
    {
        
        private IPushRelabelService _pushRelabelService;
        
        public PushRelabelController(IPushRelabelService pushRelabelService)
        {
            _pushRelabelService = pushRelabelService;
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetPushRelabelMaxGraphFlow([FromQuery] GraphParametersFlowWithId graphParametersFlowWithId)
        {
            int maxFlow = await _pushRelabelService.CalculateMaxFlow(graphParametersFlowWithId.id, graphParametersFlowWithId.source, 
                graphParametersFlowWithId.destination);

            return maxFlow;
        }

    }
}