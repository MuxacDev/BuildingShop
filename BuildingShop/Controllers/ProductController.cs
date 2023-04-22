using BuildingShop_DataAccess;
using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using BuildingShop_Models.ViewModels;
using BuildingShop_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildingShop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        
        private readonly IProductRepository prodRepo;
        private readonly IWebHostEnvironment env;
        

        public ProductController(IProductRepository prodRepo, IWebHostEnvironment env)
        {            
            this.prodRepo = prodRepo;
            this.env = env;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objList = prodRepo.GetAll(includeProperties:"Category,AppType");

            /*foreach(var obj in objList)
            {
                obj.Category=db.Category.FirstOrDefault(u=>u.Id==obj.CategoryId);
                obj.AppType = db.AppType.FirstOrDefault(u => u.Id == obj.AppTypeId);
            }*/



            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            /*IEnumerable<SelectListItem> CategoryDropDown = db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });            
            ViewData["CategoryDropDown"] = CategoryDropDown;*/
            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = prodRepo.GetAllDropdownList(WC.CategoryName),
                AppTypeSelectList = prodRepo.GetAllDropdownList(WC.AppTypeName)
            };

            if(id==null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = prodRepo.Find(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }            
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = env.WebRootPath;

                if(productVM.Product.Id==0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using(var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    prodRepo.Add(productVM.Product);
                }
                else
                {
                    //updating
                    var objFromDb = prodRepo.FirstOrDefault(u=>u.Id==productVM.Product.Id,isTracking:false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload,objFromDb.Image);
                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image=objFromDb.Image;
                    }
                    prodRepo.Update(productVM.Product);
                }
                prodRepo.Save();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = prodRepo.GetAllDropdownList(WC.CategoryName);
            productVM.AppTypeSelectList = prodRepo.GetAllDropdownList(WC.AppTypeName);          


            return View(productVM);
            
        }        

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = prodRepo.FirstOrDefault(u => u.Id == id,includeProperties:"Category,AppType");
            //product.Category= db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = prodRepo.Find(id.GetValueOrDefault());
            if (obj==null)
            {
                return NotFound(); 
            }
            
            string webRootPath = env.WebRootPath;            
            string upload = webRootPath + WC.ImagePath;               

            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            prodRepo.Remove(obj);
            prodRepo.Save();
            return RedirectToAction("Index");

        }
    }
}
