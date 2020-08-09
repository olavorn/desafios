using api.Model;
using api.Models.EntityModel;
using api.Models.Enums;
using api.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ViewModel
{
    public class AdvanceListModel : AuthenticatedModel
    {
        [Display(Name = "payments"), JsonRequired, JsonCurrency]
        public IEnumerable<long> Payments { get; set; }

        internal Advance Map()
        {
            return new Advance()
            {
                Payments = Payments.Select(q => new Payment() { Id = q }).ToList()
            };
        }
    }
}
