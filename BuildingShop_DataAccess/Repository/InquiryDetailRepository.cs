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
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly AppDbContext db;

        public InquiryDetailRepository(AppDbContext db):base(db)
        {
            this.db = db;
        }        

        public void Update(InquiryDetail obj)
        {
            this.db.InquiryDetail.Update(obj);
        }
    }
}
