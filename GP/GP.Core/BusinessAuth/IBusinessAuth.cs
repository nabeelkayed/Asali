using RealWord.Data.Entities;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Data.Entities;

namespace RealWord.Core.Auth
{
    public interface IBusinessAuth
    {
        string Generate(BusinessOwner businessOwner);
    }
}
