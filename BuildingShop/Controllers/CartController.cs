using BuildingShop_DataAccess;
using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using BuildingShop_Models.ViewModels;
using BuildingShop_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BuildingShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {        
        private readonly IWebHostEnvironment env;
        private readonly IMailService emailSender;
        private readonly IAppUserRepository userRepo;
        private readonly IProductRepository prodRepo;
        private readonly IInquiryHeaderRepository inqHRepo;
        private readonly IInquiryDetailRepository inqDRepo;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(
            AppDbContext db,
            IWebHostEnvironment env,
            IMailService emailSender,
            IAppUserRepository userRepo,
            IProductRepository prodRepo,
            IInquiryHeaderRepository inqHRepo,
            IInquiryDetailRepository inqDRepo
            )
        {            
            this.env = env;
            this.emailSender = emailSender;
            this.userRepo = userRepo;
            this.prodRepo = prodRepo;
            this.inqHRepo = inqHRepo;
            this.inqDRepo = inqDRepo;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)!=null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count()>0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

            }

            List<int> prodInCart = shoppingCartList.Select(i=>i.ProductId).ToList();
            IEnumerable<Product> prodList = prodRepo.GetAll(u => prodInCart.Contains(u.Id));

            return View(prodList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            

            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = prodRepo.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                AppUser=userRepo.FirstOrDefault(u=>u.Id==claim.Value),
                ProductList=prodList.ToList()
            };



            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(ProductUserVM productUserVM)
        {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var pathToTemplate = env.WebRootPath +
                Path.DirectorySeparatorChar.ToString() +
                "templates" +
                Path.DirectorySeparatorChar.ToString() +
                "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";
            using(StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            /*Name: {0}
            Email: {1}
            Phone: {2}
            Products: {3}*/

            StringBuilder productListSB = new StringBuilder();
            foreach(var prod in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name: {prod.Name} <span style='font-size: 14px;'> (ID: {prod.Id})</span><br/>");
            }
            string messageBody = string.Format(HtmlBody,
                productUserVM.AppUser.FullName,
                productUserVM.AppUser.Email,
                productUserVM.AppUser.PhoneNumber,
                productListSB.ToString());

            emailSender.SendMessage(WC.EmailAdmin,subject,messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                AppUserId = claim.Value,
                FullName= productUserVM.AppUser.FullName,
                Email= productUserVM.AppUser.Email,
                PhoneNumber= productUserVM.AppUser.PhoneNumber,
                InquiryDate=DateTime.Now
            };

            inqHRepo.Add( inquiryHeader );
            inqHRepo.Save();

            foreach (var prod in ProductUserVM.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id
                };
                inqDRepo.Add( inquiryDetail );                
            }
            inqDRepo.Save();

            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }


        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
