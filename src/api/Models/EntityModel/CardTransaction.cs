using api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public interface ICardTransaction
    {
        string CardDigits { get; set; }
        string CardName { get; set; }
        string CardExpirationDate { get; set; }
        string CardSecurityNumber { get; set; }
        PaymentResponse Result { get; set; }
    }
}
