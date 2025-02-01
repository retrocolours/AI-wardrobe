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

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Gender")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Select a gender.")]
        public int? GenderId { get; set; }

        [Display(Name = "Size")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Select a size.")]
        public int? SizeId { get; set; }

        [Display(Name = "Type")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Select a type.")]
        public int? TypeId { get; set; }

        //String placeholder of gender, type, size, exclude from page validation.
        [ValidateNever]
        public string? Gender { get; set; }
        [ValidateNever]
        public string? Size { get; set; }
        [ValidateNever]
        public string? Type { get; set; }


        //Drop down options
        public List<SelectListItem>? GenderOptions;
        public List<SelectListItem>? SizeOptions;
        public List<SelectListItem>? TypeOptions;

    }
}
