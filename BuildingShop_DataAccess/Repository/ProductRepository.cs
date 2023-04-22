using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using BuildingShop_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingShop_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext db;

        public ProductRepository(AppDbContext db):base(db)
        {
            this.db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if(obj==WC.CategoryName)
            {
                return db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if (obj == WC.AppTypeName)
            {
                return db.AppType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product obj)
        {
            this.db.Product.Update(obj);
        }
    }
}
