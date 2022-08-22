using Microsoft.AspNetCore.Mvc;

namespace EdmondsKarpService.Controllers
{
    [ApiController]
    public class EdmondsKarpController : ControllerBase
    {

        public EdmondsKarpController()
        {
            
        }
        
        [Route("api/edmondsKarp")]
        [HttpGet]
        public ActionResult<int> GetEdmondsKarpMaxGraphFlow()
        {
            return 5;
        }

    }
}