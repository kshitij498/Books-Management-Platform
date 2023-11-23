using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Books_Management.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        public int PlaceOrders { get; set; }
    }
}
