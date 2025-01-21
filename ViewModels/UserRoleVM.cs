using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class UserRoleVM
    {
        [Display(Name = "ID")]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
