using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class Review
    {
        public Review()
        {
            Cool = new List<ReviewCool>();
            Funny = new List<ReviewFunny>();
            Useful = new List<ReviewUseful>();
        }

        public Guid ReviewId { get; set; }
        public string Body { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ReviewCool> Cool { get; set; }
        public List<ReviewFunny> Funny { get; set; }
        public List<ReviewUseful> Useful { get; set; }

        public List<Photo> Photos { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}