using System;
using System.Collections.Generic;
using System.Text;

namespace GP.Core.Models
{
    public class BusinessForUpdatePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

