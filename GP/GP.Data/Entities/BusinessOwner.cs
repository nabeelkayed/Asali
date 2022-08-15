using RealWord.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GP.Data.Entities
{
    public class BusinessOwner
    {
        public Guid BusinessOwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public bool Setup { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
