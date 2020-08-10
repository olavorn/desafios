using api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class User
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the User
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

        /// <summary>
        /// Advances by this user
        /// </summary>
        public List<Advance> Advances { get; set; }
    }
}
