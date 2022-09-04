using System.Collections.Generic;
using System.Linq;
using BFSService.Models;

namespace BFSService.Services
{
    public interface IBfService
    {
        public BFSResult Bfs(Graph? graph, int source, int t);

    }
    
    public class BfService: IBfService
    {
        public BFSResult Bfs(Graph? graph, int source, int t)
        {
            int numberOfVertices = graph.NumberOfVertices;
            int[] parent = new int[numberOfVertices];
            bool[] visited = new bool[numberOfVertices];

            for (int i = 0; i < numberOfVertices; i++)
            {
                visited[i] = false;
            }

            LinkedList<int> queue = new LinkedList<int>();
            queue.AddLast(source);
            visited[source] = true;
            parent[source] = -1;

            while (queue.Count != 0)
            {
                int u = queue.First();
                queue.RemoveFirst();
                for (int v = 0; v < numberOfVertices; v++)
                {
                    if (!visited[v] && graph.AdjacencyMatrix[u][v] > 0)
                    {
                        queue.AddLast(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return new BFSResult(parent, visited[t]);
        }
        
    }
}