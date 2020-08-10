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
    public class AdvanceDetailsModel : AuthenticatedModel
    {
        [Display(Name = "id"), JsonRequired, JsonCurrency]
        public long Id { get; set; }

        public virtual Advance Map()
        {
            return new Advance()
            {
                Id = this.Id
            };
        }
    }
}
