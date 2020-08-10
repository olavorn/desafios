using api.Model;
using api.Models.EntityModel;
using api.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ResultModel
{
    public class AdvanceJson : IActionResult
    {
        public AdvanceJson() { }

        public AdvanceJson(Advance advance)
        {
            Id = advance.Id;
            AdvanceDue = advance.AdvanceDue;
            CustomerId = advance.CustomerId;
            EvaluationDateEnd = advance.EvaluationDateEnd;
            EvaluationDateStart = advance.EvaluationDateStart;
            EvaluationBy = advance.EvaluationBy;
            IsApproved = advance.IsApproved;
            Payments = advance.Payments;
            RequestDate = advance.RequestDate;
            GrossAmount = advance.GrossAmount;
            NetAmount = advance.NetAmount;
            FixedTaxes = advance.FixedTaxes;
            AdvanceTaxes = advance.AdvanceTaxes;
        }

        public long Id { get; set; }
        public decimal AdvanceDue { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? EvaluationDateEnd { get; set; }
        public DateTime? EvaluationDateStart { get; set; }
        public bool? IsApproved { get; set; }
        public List<Payment> Payments { get; set; }
        public DateTime RequestDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal FixedTaxes { get; set; }
        public decimal AdvanceTaxes { get; set; }
        public Guid? EvaluationBy { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
