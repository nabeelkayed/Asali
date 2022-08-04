using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class BusinessTags
    {
        public Guid BusinessId { get; set; }
        public string TagId { get; set; }

        public Business Business { get; set; } 
        public Tag Tag { get; set; } 
    }
}
