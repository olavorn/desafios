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
    public class AdvanceEvaluationModel : AdvanceDetailsModel
    {
        [Display(Name = "approve"), JsonRequired]
        public bool IsApproved { get; set; }

        public override Advance Map()
        {
            return base.Map();
        }
    }
}
