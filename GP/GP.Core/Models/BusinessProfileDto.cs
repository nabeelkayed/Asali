using GP.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GP.Core.Models
{
    public class BusinessProfileDto
    {
        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string Website { get; set; }
        public string MenuWebsite { get; set; }
        public string Category { get; set; }
        public string Map { get; set; }
        public string PhoneNumber { get; set; }
        public BusinessOwner BusinessOwner { get; set; }
    }
}