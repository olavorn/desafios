using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ToCurrency(this decimal amount)
        {
            return Math.Round(amount, 2);
        }
    }
}
