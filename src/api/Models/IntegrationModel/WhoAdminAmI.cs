using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.IntegrationModel
{
    public class WhoAdminAmI : WhoAmI
    {
        public Guid AdminId { get; set; }
    }
}
