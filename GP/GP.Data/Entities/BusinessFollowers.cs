using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class BusinessFollowers
    {
        public Guid BusinessId { get; set; }
        public Guid UserId { get; set; }

        public Business Business { get; set; }
        public User User { get; set; }
    }
}