using api.Models.EntityModel;
using api.Models.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.ViewModel
{
    public class AccountLoginModel
    {
        [Display(Name = "login")]
        public string Login { get; set; }

        [Display(Name = "password")]
        public string Password { get; set; }

        public Customer Map() => new Customer
        {
            Id = Guid.Empty,
            Name = null,
            Email = Login,
            IsActive = true
        };
    }
}