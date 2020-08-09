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

        [HttpPost, Route("request-advance")]
        public async Task<AdvanceJson> RequestForAdvance([FromBody] AdvanceListModel model)
        {
            var advProcessing = new AdvanceProcessing(_context, _acquirerApi, _accountApi, _log);
            var advance = model.Map();

            try
            {
                advance = await advProcessing.Request(advance, model.AuthToken);
            }
            catch(Exception)
            {
                return new AdvanceErrorJson(advProcessing);
            }

            await _context.SaveChangesAsync();
            return new AdvanceJson(advance);
        }

        [HttpGet, Route("available-for-advance")]
        public async Task<PaymentListJson> ListAvailableForAdvance([FromQuery] PaymentListModel model)
        {
            var wai = await _accountApi.WhoAmI(model.AuthToken);

            var paymentQuery = _context.Payments
                 .WhereDateFrom(model.StartDate)
                 .WhereDateUntil(model.EndDate)
                 .WherePayerName(model.CardName)
                 .WherePaidDateFrom(model.StartPaidDate)
                 .WherePaidDateFrom(model.EndPaidDate)
                 .WhereStatus(model.Status)
                 .Where( q=> q.AdvanceId == null && q.CustomerId == wai.CustomerId);

            paymentQuery = paymentQuery
                .OrderByDescending(q => q.CreatedAt).AsQueryable();

            if (model.Index.HasValue)
                paymentQuery = paymentQuery.Skip(model.Index.Value);

            if (model.Length.HasValue)
                paymentQuery = paymentQuery.Take(model.Length.Value);

            var payments = await paymentQuery.ToListAsync();

            //var periodAmount = await paymentQuery.SumAsync(payment => payment.Amount);
            var count = await paymentQuery.LongCountAsync();

            return new PaymentListJson(payments, count);
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
