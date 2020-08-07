using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Validations
{
    public class JsonCurrency : RangeAttribute
    {
        private const double _minimum = 0;
        private const double _maximum = 999999.99;

        public JsonCurrency() : base(_minimum, _maximum)
        {
            ErrorMessage = $"{0}: Between {_minimum} and {_maximum}.";
        }
    }
}
