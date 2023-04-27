using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingShop_Models
{
    public class Product
    {
        public Product()
        {
            TempQuantity= 1;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string ShortDesc { get; set; }

        [Range(1,int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }

        [Display(Name="Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Application Type")]
        public int AppTypeId { get; set; }
        [ForeignKey("AppTypeId")]
        public virtual AppType AppType { get; set; }

        [NotMapped]
        [Range(1,10000)]
        public int TempQuantity { get; set; }
    }
}
