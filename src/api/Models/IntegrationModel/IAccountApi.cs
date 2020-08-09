using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.IntegrationModel
{
    public interface IAccountApi
    {
        Task<WhoAmI> WhoAmI(string token);
    }
}
