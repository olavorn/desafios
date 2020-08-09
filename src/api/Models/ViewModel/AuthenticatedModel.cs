using System.ComponentModel.DataAnnotations;

namespace api.Models.ViewModel
{
    public class AuthenticatedModel : PagedModel
    {
        [Display(Name = "token")]
        public string AuthToken { get; set; }
    }
}