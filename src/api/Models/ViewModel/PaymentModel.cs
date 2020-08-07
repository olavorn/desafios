using api.Model;
using api.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ViewModel
{
    public class PaymentModel
    {
        [Display(Name = "amount"), JsonRequired, JsonCurrency]
        public decimal? Amount { get; set; }

        [Display(Name = "customer"), JsonRequired]
        public ICollection<CustomerModel> Customer { get; set; }

        public Payment Map() => new Payment
        {
            Amount = Amount.Value,
            Customer = Customer.Map(),
            CreatedAt = DateTime.Now
        };
    }
}
