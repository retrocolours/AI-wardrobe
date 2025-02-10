using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AI_Wardrobe.Repositories
{
    public class ProductRepo
    {
        private SelectListItem _optionDefault = new SelectListItem
        {
            Text = "Please select one from the list.",
            Value = "-1",
        };

        private readonly AiwardrobeContext _aiWardrobeContext;

        public ProductRepo(AiwardrobeContext aiwardrobeContext)
        {
            _aiWardrobeContext = aiwardrobeContext;
        }

        public IEnumerable<ProductVM> GetAll()
        {

            return from i in _aiWardrobeContext.Items
                   join s in _aiWardrobeContext.Sizes
                   on i.Fksizeid equals s.Sizeid
                   join g in _aiWardrobeContext.ItemGenders
                   on i.Fkitemgenderid equals g.Itemgenderid
                   join t in _aiWardrobeContext.ItemTypes
                   on i.Fktypeid equals t.Itemtypeid
                   select new ProductVM
                   {
                       Id = i.Itemid,
                       Name = i.ItemName,
                       Description = i.Itemdescription,
                       Price = i.Itemprice,
                       GenderId = i.Fkitemgenderid,
                       Gender = g.Itemgenderdescription,
                       SizeId = i.Fksizeid,
                       Size = s.Sizedescription,
                       TypeId = i.Fktypeid,
                       Type = t.Itemtypedescription
                   };

        }

        public Item? GetItem(int id)
        {
            return _aiWardrobeContext.Items.Where(i => i.Itemid == id).FirstOrDefault();
        }

        public string AddItem(Item item)
        {
            try
            {
                _aiWardrobeContext.Items.Add(item);
                _aiWardrobeContext.SaveChanges();
                return $"success,Successfully created: {item.Itemid}.";
            }
            catch (Exception ex)
            {
                return $"error,Error encountered creating.\n" +
                    $"Error message: {ex.Message}";
            }
        }

        public string UpdateItem(Item item)
        {
            try
            {
                _aiWardrobeContext.Items.Update(item);
                _aiWardrobeContext.SaveChanges();
                return $"success,Successfully updated: {item.Itemid}.";
            }
            catch (Exception ex)
            {
                return $"error,Error encountered updating.\n" +
                    $"Error message: {ex.Message}";
            }
        }

        public List<SelectListItem> GetSizeOptions()
        {
            List<SelectListItem> sizeOptions = new List<SelectListItem>();
            sizeOptions.Add(_optionDefault);

            sizeOptions.AddRange(_aiWardrobeContext.Sizes.Select(size => new SelectListItem
            {
                Text = size.Sizedescription,
                Value = size.Sizeid.ToString()
            }).ToList());

            return sizeOptions;
        }

        public List<SelectListItem> GetTypeOptions()
        {
            List<SelectListItem> typeOptions = new List<SelectListItem>();
            typeOptions.Add(_optionDefault);

            typeOptions.AddRange(_aiWardrobeContext.ItemTypes.Select(type => new SelectListItem
            {
                Text = type.Itemtypedescription,
                Value = type.Itemtypeid.ToString()
            }).ToList());

            return typeOptions;
        }

        public List<SelectListItem> GetGenderOptions()
        {
            List<SelectListItem> genderOptions = new List<SelectListItem>();
            genderOptions.Add(_optionDefault);

            genderOptions.AddRange(_aiWardrobeContext.ItemGenders.Select(gender => new SelectListItem
            {
                Text = gender.Itemgenderdescription,
                Value = gender.Itemgenderid.ToString()
            }).ToList());

            return genderOptions;
        }
    }
}