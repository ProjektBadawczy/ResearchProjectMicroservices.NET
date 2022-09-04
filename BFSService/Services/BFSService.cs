using System.Collections.Generic;
using System.Linq;
using BFSService.Models;

namespace BFSService.Services
{
    public interface IBfService
    {
        public BFSResult Bfs(GraphForBFS graphForBFS);

    }
    
    public class BfService: IBfService
    {
        public BFSResult Bfs(GraphForBFS graphForBFS)
        {
            int numberOfVertices = graphForBFS.NumberOfVertices;
            int[] parent = new int[numberOfVertices];
            bool[] visited = new bool[numberOfVertices];

            for (int i = 0; i < numberOfVertices; i++)
            {
                visited[i] = false;
            }

            LinkedList<int> queue = new LinkedList<int>();
            queue.AddLast(graphForBFS.Source);
            visited[graphForBFS.Source] = true;
            parent[graphForBFS.Source] = -1;

            while (queue.Count != 0)
            {
                int u = queue.First();
                queue.RemoveFirst();
                for (int v = 0; v < numberOfVertices; v++)
                {
                    if (!visited[v] && graphForBFS.AdjacencyMatrix[u][v] > 0)
                    {
                        queue.AddLast(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }
            return new BFSResult(parent, visited[graphForBFS.Destination]);
        }
        
    }
}