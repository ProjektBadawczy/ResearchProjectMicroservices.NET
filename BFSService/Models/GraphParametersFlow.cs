using BFSService.Models;

namespace EdmondsKarpService.Models
{
    public class GraphParametersFlow
    {
        public Graph graph { get; set; }
        
        public int source { get; set; }
        
        public int destination { get; set; }
    }
}