using api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class Customer
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the Customer
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indicate if the Customer is Active
        /// </summary>
        public bool IsActive { get; set; }

        //ef navigation props
        public List<Instalment> Instalments { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
