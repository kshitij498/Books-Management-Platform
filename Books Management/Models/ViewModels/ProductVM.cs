using Microsoft.AspNetCore.Mvc.Rendering;

namespace Books_Management.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> categoryList { get; set; }
    }
}
