using api.Models.EntityModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.IntegrationModel
{
    public class AccountApiOptions : IOptions<AccountApiOptions>
    {
        public string WhoAmIUrl { get; set; }
        public bool UseInternal { get; set; }
        public apiContext DbSource { get; set; }

        public AccountApiOptions Value
        {
            get => this;
        }
    }
}
