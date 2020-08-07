using api.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ResultModel
{
    public class PaymentListJson : IActionResult
    {
        public PaymentListJson() { }

        public PaymentListJson(IEnumerable<Payment> payments, long count)
        {
            Payments = payments.Select(payment => new PaymentJson(payment)).ToList();
            Count = count;
        }

        public IEnumerable<PaymentJson> Payments { get; set; }
        public long Count { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
