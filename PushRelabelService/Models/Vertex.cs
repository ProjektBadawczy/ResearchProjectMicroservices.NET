﻿namespace PushRelabelService.Models
{
    public class Vertex
    {

        // number of the end vertex
        // weight or capacity
        // associated with the edge

        public int i { get; set; }
        public int w { get; set; }

        public Vertex(int i, int w)
        {
            this.i = i;
            this.w = w;
        }
    }
}