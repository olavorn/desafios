using api.Models.EntityModel;
using api.Models.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.ViewModel
{
    public class CustomerModel
    {
        [Display(Name = "id"), JsonRequired]
        public Guid Id { get; set; }

        [Display(Name = "name")]
        public string Name { get; set; }

        [Display(Name = "email")]
        public string Email { get; set; }

        public Customer Map() => new Customer
        {
            Id = Id,
            Name = Name,
            Email = Email,
            IsActive = true
        };
    }
}