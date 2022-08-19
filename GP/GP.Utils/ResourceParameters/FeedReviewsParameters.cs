using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Utils.ResourceParameters
{
    public class FeedReviewsParameters
    {
        public string Category { get; set; } = "";//search
        public int Rate { get; set; } = 0;//search
        public bool Trendy { get; set; } = false;//sort
        public bool Latest { get; set; } = true; //sort
        public string Sentement { get; set; } = ""; //search
       // public int Limit { get; set; } = 20;
        //public int Offset { get; set; } = 0;
    }
}
