using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class ProductVM
    {
        [Display(Name = "Product ID")]
        public int Id { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string? Description { get; set; }

        [Display(Name = "Price")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression(/*@"^-?\d+(\.\d{1,2})?$"*/  @"^\d+(\.\d{1,2})?$", ErrorMessage = "Balance must be a positive number with 2 decimals at most.")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public int? GenderId { get; set; }

        [Display(Name = "Size")]
        [Required]
        public int? SizeId { get; set; }

        [Display(Name = "Type")]
        [Required]
        public int? TypeId { get; set; }

        public List<SelectListItem> GenderOptions = new List<SelectListItem>();

        public List<SelectListItem> SizeOptions = new List<SelectListItem>();
        
        public List<SelectListItem> TypeOptions = new List<SelectListItem>();

    }
}
