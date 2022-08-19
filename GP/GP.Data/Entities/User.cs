using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.Data.Entities
{
    public class User
    {
        public User()
        {
            Followings = new List<BusinessFollowers>();
            Cool = new List<ReviewCool>();
            Funny = new List<ReviewFunny>();
            Useful = new List<ReviewUseful>();

            Reviews = new List<Review>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public string HeadLine { get; set; }
        public string Bio { get; set; }
        public string Lon { get; set; }
        public string Lat { get; set; }


        public List<BusinessFollowers> Followings { get; set; }
        public List<ReviewCool> Cool { get; set; } 
        public List<ReviewFunny> Funny { get; set; }
        public List<ReviewUseful> Useful { get; set; }

        public List<Review> Reviews { get; set; }
    }
}