using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class Tag
    {
        public Tag()
        {
            BusinessTags = new List<BusinessTags>();
        }

        public string TagId { get; set; }

        //public Guid TagId { get; set; }
        // public string TagName { get; set; }

        public List<BusinessTags> BusinessTags { get; set; }
    }  
}
