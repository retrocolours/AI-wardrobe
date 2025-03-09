using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class UserVm
    {

        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string? LastName { get; set; }

        [Display(Name = "Address 1")]
        [Required]
        public string? Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }

        [Display(Name = "City")]
        [Required]
        public string? City { get; set; }

        [Display(Name = "Province")]
        [Required]
        public string? Province { get; set; }

        [Display(Name = "PostalCode")]
        [Required]
        public string? PostalCode { get; set; }

        [Display(Name = "Phone")]
        public int? Phone { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string? Email { get; set; }
    }
}