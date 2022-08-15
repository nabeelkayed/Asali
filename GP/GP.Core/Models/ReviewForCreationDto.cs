using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Core.Models
{
    public class ReviewForCreationDto 
    {
        public string ReviewText { get; set; }
        public int Rate { get; set; }
        public List<string> Photos { get; set; }
    }
}
