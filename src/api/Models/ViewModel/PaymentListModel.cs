using api.Model;
using api.Models.Enums;
using api.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ViewModel
{
    public class PaymentListModel : AuthenticatedModel
    {
        [Display(Name = "amount"), JsonRequired, JsonCurrency]
        public decimal Amount { get; set; }

        [Display(Name = "installments"), JsonRequired, JsonNatural]
        public short InstalmentsCount { get; set; }

        [Display(Name = "cardNumber"), JsonRequired, JsonNatural]
        public string CardNumber { get; set; }

        [Display(Name = "expirationDate"), JsonRequired, JsonNatural]
        public string CardExpirationDate { get; set; }

        [Display(Name = "cardName"), JsonRequired, JsonNatural]
        public string CardName { get; set; }

        [Display(Name = "cardSecurityNumber"), JsonRequired, JsonNatural]
        public string CardSecurityNumber { get; set; }

        [Display(Name = "customer"), JsonRequired]
        public CustomerModel Customer { get; set; }

        [Display(Name = "startCreatedDate")]
        public DateTime? StartDate { get;  set; }

        [Display(Name = "endCreatedDate")]
        public DateTime? EndDate { get;  set; }

        [Display(Name = "status")]
        public PaymentStatus Status { get; set; }

        [Display(Name = "startPaidDate")]
        public DateTime? StartPaidDate { get;  set; }

        [Display(Name = "endPaidDate")]
        public DateTime? EndPaidDate { get;  set; }

        public Payment Map() => new Payment
        {
            Amount = Amount,
            CustomerId = Customer.Map().Id,
            InstalmentsCount = InstalmentsCount,
            CreatedAt = DateTime.UtcNow,
            CardDigits = CardNumber,
            CardName = CardName,
            CardExpirationDate = CardExpirationDate,
            CardSecurityNumber = CardSecurityNumber
        };
    }
}
