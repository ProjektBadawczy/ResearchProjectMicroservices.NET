using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PushRelabelService.Models;

namespace PushRelabelService.Services
{
    public interface IPushRelabelService
    {
        public Task<int> CalculateMaxFlow(int id, int source, int destination);

    }

    public class PushRelabelService : IPushRelabelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public PushRelabelService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        private DirectedGraph initResidualGraph(DirectedGraph graph)
        {
            DirectedGraph residualGraph = new DirectedGraph(graph.Vertices, graph.Id);
 
            // Construct residual graph
            for (int u = 0; u < graph.Vertices; u++) {
 
                foreach (Vertex v in
                    graph.AdjacencyList[u]) {
 
                    // If forward edge already
                    // exists, update its weight
                    if (residualGraph.HasEdge(u, v.i))
                        residualGraph.GetEdge(u, v.i).w
                            += v.w;
 
                    // In case it does not
                    // exist, create one
                    else
                        residualGraph.AddEdge(u, v.i, v.w);
 
                    // If backward edge does
                    // not already exist, add it
                    if (!residualGraph.HasEdge(v.i, u))
                        residualGraph.AddEdge(v.i, u, 0);
                }
            }

            return residualGraph;
        }

        public async Task<int> CalculateMaxFlow(int id, int source, int destination)
        {
            var client = _httpClientFactory.CreateClient();
            var requestGraph = new HttpRequestMessage(HttpMethod.Get, $"http://graph-service:80/directedGraph?id={id}");
            var responseGraph = await client.SendAsync(requestGraph);

            if (responseGraph.IsSuccessStatusCode)
            {
                using var contentStreamGraph = await responseGraph.Content.ReadAsStreamAsync();
                DirectedGraph? graph = await JsonSerializer.DeserializeAsync<DirectedGraph>(contentStreamGraph);
                
                DirectedGraph residualGraph = initResidualGraph(graph);
            
                List<int> queue = new List<int>();
 
                // Step 1: Initialize pre-flow
                // to store excess flow
                int[] e = new int[graph.Vertices];
 
                // to store height of vertices
                int[] h = new int[graph.Vertices];
 
                bool[] inQueue = new bool[graph.Vertices];
 
                // set the height of source to V
                h[source] = graph.Vertices;
 
                // send maximum flow possible
                // from source to all its adjacent vertices
                foreach (Vertex v in graph.AdjacencyList[source]) 
                {
                    residualGraph.GetEdge(source, v.i).w = 0;
                    residualGraph.GetEdge(v.i, source).w = v.w;
 
                    // update excess flow
                    e[v.i] = v.w;
 
                    if (v.i != destination) {
                        queue.Add(v.i);
                        inQueue[v.i] = true;
                    }
                }
 
                // Step 2: Update the pre-flow
                // while there remains an applicable
                // push or relabel operation
                while (queue.Count!=0) {
 
                    // vertex removed from
                    // queue in constant time
                    int u = queue[0];
                    queue.RemoveAt(0);
                    inQueue[u] = false;
 
                    relabel(u, h, residualGraph);
                    push(u, e, h, queue, inQueue, residualGraph, source,destination);
                }
 
                return e[destination];
            }
            return default;
        }
        
        private void relabel(int u, int[] h, DirectedGraph residualGraph)
        {
            int minHeight = int.MaxValue;
 
            foreach (Vertex v in residualGraph.AdjacencyList[u]) 
            {
                if (v.w > 0)
                {
                    minHeight = Math.Min(h[v.i], minHeight);
                }
            }
 
            h[u] = minHeight + 1;
        }
        
        private void push(int u, int[] e, int[] h, List<int> queue, bool[] inQueue, DirectedGraph residualGraph, int source, int destination)
        {
            foreach (Vertex v in residualGraph.AdjacencyList[u])
            {
 
                // after pushing flow if
                // there is no excess flow,
                // then break
                if (e[u] == 0)
                    break;
 
                // push more flow to
                // the adjacent v if possible
                if (v.w > 0 && h[v.i] < h[u]) {
                    // flow possible
                    int f = Math.Min(e[u], v.w);
 
                    v.w -= f;
                    residualGraph.GetEdge(v.i, u).w += f;
 
                    e[u] -= f;
                    e[v.i] += f;
 
                    // add the new overflowing
                    // immediate vertex to queue
                    if (!inQueue[v.i] && v.i != source && v.i != destination) {
                        queue.Add(v.i);
                        inQueue[v.i] = true;
                    }
                }
            }
 
            // if after sending flow to all the
            // intermediate vertices, the
            // vertex is still overflowing.
            // add it to queue again
            if (e[u] != 0) {
                queue.Add(u);
                inQueue[u] = true;
            }
        } 
        
    }
}