using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.IntegrationModel
{
    public class WhoAmI
    {
        public Guid CustomerId { get; set; }
        public string SessionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
