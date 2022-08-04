using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class OpenDay
    {
        public Guid OpenDayId { get; set; }
        public string Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
