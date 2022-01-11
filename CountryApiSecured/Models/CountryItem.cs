using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountryApi.Models
{
    public class CountryItem
    {
        [Key] 
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [StringLength(80, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Capital City")]
        public string CapitalCity { get; set; }

        [Required]
        public string Language { get; set; }

        [Display(Name = "Currency (ISO)")]
        [StringLength(3)]
        [Required]
        public string Currency { get; set; }
    }
}
