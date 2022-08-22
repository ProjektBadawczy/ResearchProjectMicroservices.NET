using System;
using System.Linq;
using System.Threading.Tasks;
using BFSService.Models;
using BFSService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFSService.Controllers
{
    [ApiController]
    public class BFSController : ControllerBase
    {
        private IBfSservice _bfSservice;

        public BFSController(IBfSservice bfSservice)
        {
            _bfSservice = bfSservice;
        }
        
        [Route("api/bfs")]
        [HttpGet]
        public ActionResult<BFSResult> Bfs()
        {
            return null;
        }

    }
}