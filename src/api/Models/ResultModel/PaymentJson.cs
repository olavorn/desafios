using api.Model;
using api.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ResultModel
{
    public class PaymentJson : IActionResult
    {
        public PaymentJson() { }

        public PaymentJson(Payment payment)
        {
            Id = payment.Id;
            Amount = payment.Amount;
            CardLastDigits = payment.CardLastDigits;
            InstallmentsCount = payment.InstallmentsCount;
            OperatorResponse = payment.OperatorResponse;
            PaidAt = payment.PaidAt;
            Result = payment.Result;
            Status = payment.Status;
            TransferDate = payment.TransferDate;
            TransferDue = payment.TransferDue;
            Amount = payment.Amount;
        }

        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public decimal Amount { get; set; }
        public short CardLastDigits { get; set; }
        public int InstallmentsCount { get; set; }
        public OperatorResponse OperatorResponse { get; set; }
        public DateTime? PaidAt { get; set; }
        public PaymentResponse Result { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? TransferDate { get; set; }
        public decimal? TransferDue { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
