using RealWord.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Core.Models
{
    public class BusinessDto
    {
        public Guid BusinessId { get; set; }
        public string Token { get; set; }       
        public bool Setup { get; set; }
    }
}