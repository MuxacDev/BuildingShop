using BuildingShop_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BuildingShop.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository inqHRepo;
        private readonly IInquiryDetailRepository inqDRepo;

        public InquiryController(IInquiryHeaderRepository inqHRepo, IInquiryDetailRepository inqDRepo)
        {
            this.inqHRepo = inqHRepo;
            this.inqDRepo = inqDRepo;
        }

        public IActionResult Index() 
        {
            return View();
        }
    }
}
