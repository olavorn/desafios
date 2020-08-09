using System.ComponentModel.DataAnnotations;

namespace api.Models.ViewModel
{
    public class PagedModel
    {
        [Display( Name = "index" )]
        public int? Index { get; set; }

        [Display(Name = "length")]
        public int? Length { get; set; }
    }
}