using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;
using api.Models.EntityModel;
using api.Models.IntegrationModel;
using api.Models.ResultModel;
using api.Models.ServiceModel;
using api.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/v2/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IAcquirerApi _acquirerApi;
        private readonly IAccountApi _accountApi;
        private readonly ILogger _log;

        public PaymentController(apiContext context, IAcquirerApi acquirerApi, IAccountApi accountApi, ILogger log)
        {
            _context = context;
            _acquirerApi = acquirerApi;
            _accountApi = accountApi;
            _log = log;
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "transaction", "transaction2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost, Route("pay-with-card")]
        public async Task<PaymentJson> ProcessPayment([FromBody] PaymentModel model)
        {
            var payProcessing = new PaymentProcessing(_context, _acquirerApi, _accountApi, _log);
            var payment = model.Map();

            if (!await payProcessing.Process(payment, model.AuthToken ))
            {
                return new PaymentErrorJson(payProcessing);
            }

            await _context.SaveChangesAsync();
            return new PaymentJson(payment);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
