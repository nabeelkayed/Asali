using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class ReviewCool
    {
        public Guid ReviewId { get; set; }
        public Guid UserId { get; set; }

        public Review Review { get; set; }
        public User User { get; set; }
    }
}