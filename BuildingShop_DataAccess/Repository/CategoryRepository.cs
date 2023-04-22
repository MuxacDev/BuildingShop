using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingShop_DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext db;

        public CategoryRepository(AppDbContext db):base(db)
        {
            this.db = db;
        }
        public void Update(Category obj)
        {
            var objFromDb = /*db.Category*/base.FirstOrDefault(u=>u.Id==obj.Id);
            if (objFromDb != null) 
            {
                objFromDb.Name= obj.Name;
                objFromDb.DisplayOrder= obj.DisplayOrder;
            }
        }
    }
}
