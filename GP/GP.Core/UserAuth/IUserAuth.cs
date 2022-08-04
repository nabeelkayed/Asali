using RealWord.Data.Entities;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Core.Auth
{
    public interface IUserAuth
    {
        string Generate(User user);
    }
}
