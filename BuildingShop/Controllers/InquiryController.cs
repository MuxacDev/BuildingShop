using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using BuildingShop_Models.ViewModels;
using BuildingShop_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BuildingShop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository inqHRepo;
        private readonly IInquiryDetailRepository inqDRepo;

        [BindProperty]
        public InquiryVM InquiryVM { get; set; }

        public InquiryController(IInquiryHeaderRepository inqHRepo, IInquiryDetailRepository inqDRepo)
        {
            this.inqHRepo = inqHRepo;
            this.inqDRepo = inqDRepo;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM()
            {
                InquiryHeader = inqHRepo.FirstOrDefault(u => u.Id == id),
                InquiryDetail = inqDRepo.GetAll(u => u.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList= new List<ShoppingCart>();

            InquiryVM.InquiryDetail= inqDRepo.GetAll(u=>u.InquiryHeaderId==InquiryVM.InquiryHeader.Id);

            foreach(var detail in InquiryVM.InquiryDetail)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId
                };
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.InquiryHeader.Id);


            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = inqHRepo.FirstOrDefault(u => u.Id == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = inqDRepo.GetAll(u=>u.InquiryHeaderId==InquiryVM.InquiryHeader.Id);

            inqDRepo.RemoveRange(inquiryDetails);
            inqHRepo.Remove(inquiryHeader);
            inqHRepo.Save();

            return RedirectToAction(nameof(Index));
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList() 
        {
            return Json(new { data = inqHRepo.GetAll() });
        }
        #endregion
    }
}
