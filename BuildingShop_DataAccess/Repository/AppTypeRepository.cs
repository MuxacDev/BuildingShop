using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingShop_DataAccess.Repository
{
    public class AppTypeRepository : Repository<AppType>, IAppTypeRepository
    {
        private readonly AppDbContext db;

        public AppTypeRepository(AppDbContext db):base(db)
        {
            this.db = db;
        }
        public void Update(AppType obj)
        {
            var objFromDb = /*db.Category*/base.FirstOrDefault(u=>u.Id==obj.Id);
            if (objFromDb != null) 
            {
                objFromDb.Name= obj.Name;                
            }
        }
    }
}
