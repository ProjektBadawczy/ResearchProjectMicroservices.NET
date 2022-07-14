namespace BFSService.Models
{
    public class BFSResult
    {
        public int[] Parents { get; set; }

        public bool Success { get; set; }

        public BFSResult(int[] parents, bool success)
        {
            Parents = parents;
            Success = success;

        }
    }
}