using CommonModels;
using GraphService.Repositories;


namespace GraphService.Services
{
    public interface IGraphService
    {
        public Graph getGraph(int id);

        public DirectedGraph getDirectedGraph(int id);

    }
    public class GraphService: IGraphService
    {
        public GraphRepository _graphRepository;

        public GraphService(GraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public Graph getGraph(int id)
        {
            var graph = _graphRepository.Graphs.Find(obj => obj.graph.Id == id);
            if (graph != null)
            {
                return graph.graph;
            }

            return null;
        }
        
        public DirectedGraph getDirectedGraph(int id)
        {
            var graph = _graphRepository.Graphs.Find(obj => obj.directedGraph.Id == id);
            if (graph != null)
            {
                return graph.directedGraph;
            }

            return null;
        }
        
    }
}