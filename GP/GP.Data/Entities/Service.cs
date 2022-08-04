using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class Service
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }
}