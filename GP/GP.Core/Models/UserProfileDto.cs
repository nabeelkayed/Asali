using RealWord.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GP.Core.Models
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string HeadLine { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; } 
        public List<Review> Reviews { get; set; } 
        public List<Business> Businesses { get; set; }
    }
}