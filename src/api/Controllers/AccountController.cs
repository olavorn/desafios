using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Model;
using api.Models.EntityModel;
using api.Models.IntegrationModel;
using api.Models.ResultModel;
using api.Models.ServiceModel;
using api.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/v2/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IAcquirerApi _acquirerApi;
        private readonly IAccountApi _accountApi;
        private readonly ILogger _log;

        public AccountController(apiContext context, IAcquirerApi acquirerApi, IAccountApi accountApi, ILogger log)
        {
            _context = context;
            _acquirerApi = acquirerApi;
            _accountApi = accountApi;
            _log = log;
        }

        [HttpPost, Route("generate-token")]
        public async Task<dynamic> GenerateCustomerTokenFromEmail([FromQuery]AccountLoginModel model)
        {
            var wai = await _context.Customers.FirstOrDefaultAsync(q => q.Email == model.Login);

            return new { Token = wai.MapToWhoAmI().EncryptToken() };
        }

    }
}
