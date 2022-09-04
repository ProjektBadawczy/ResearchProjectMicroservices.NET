using System;
using System.Text.Json.Serialization;

namespace EdmondsKarpService.Models
{
    [Serializable]
    public class BFSResult
    {
        [JsonPropertyName("parents")]
        public int[] Parents { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        public BFSResult(int[] parents, bool success)
        {
            Parents = parents;
            Success = success;

        }
    }
}