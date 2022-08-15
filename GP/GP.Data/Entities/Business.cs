using GP.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data.Entities
{
    public class Business
    {
        public Business()
        {
            Followers = new List<BusinessFollowers>();
            //Tags = new List<BusinessTags>();

            Reviews = new List<Review>();
            //Features = new List<Feature>();
            Photos = new List<Photo>();
           // Services = new List<Service>();
            OpenDays = new List<OpenDay>();
        }

        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Location { get; set; } ////////////////////////
        public string Bio { get; set; }
        public string Website { get; set; }
        public string MenuWebsite { get; set; }
        public string Category { get; set; }
        //public string Map { get; set; }////////////////////////
        public string PhoneNumber { get; set; }

        public List<BusinessFollowers> Followers { get; set; }
        //public List<BusinessTags> Tags { get; set; }

        public List<Review> Reviews { get; set; }
        //public List<Feature> Features { get; set; }
        public List<Photo> Photos { get; set; }
        //public List<Service> Services { get; set; }
        public List<OpenDay> OpenDays { get; set; }

        public BusinessOwner BusinessOwner { get; set; }
    }
}