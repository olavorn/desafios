using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Validations
{
    public class JsonNatural : RangeAttribute
    {
        private const int _minimum = 0;
        private const int _maximum = 480;

        public JsonNatural() : base(_minimum, _maximum)
        {
            ErrorMessage = $"{0}: Between {_minimum} and {_maximum}.";
        }
    }
}
