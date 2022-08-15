using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Core.Models
{
    public class BusinessProfileForUpdateDto 
    {
        public string BusinessName { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string Website { get; set; }
        public string MenuWebsite { get; set; }
        public string Category { get; set; }
        public string Map { get; set; }
        public string PhoneNumber { get; set; }
    }
}