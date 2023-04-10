using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuildingShop.Models
{
    public class AppType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }        
        
    }
}
