﻿using GraphService.Models;
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
        [Route("/api/graph")]
        public ActionResult<Graph> GetGraph([FromQuery] int id)
        {
            Graph graph = _graphService.getGraph(id);
            if (graph != null)
            {
                return graph;
            }

            return NotFound();
        }
        
        
    }
}