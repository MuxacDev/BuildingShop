using BuildingShop.Data;
using BuildingShop.Models;
using BuildingShop.Models.ViewModels;
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
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        

        public ProductController(AppDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product.Include(u => u.Category).Include(u => u.AppType);

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
                CategorySelectList = db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                AppTypeSelectList = db.AppType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if(id==null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = db.Product.Find(id);
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

                    db.Product.Add(productVM.Product);
                }
                else
                {
                    //updating
                    var objFromDb = db.Product.AsNoTracking().FirstOrDefault(u=>u.Id==productVM.Product.Id);

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
                    db.Product.Update(productVM.Product);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            productVM.AppTypeSelectList = db.AppType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });


            return View(productVM);
            
        }        

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = db.Product.Include(u=>u.Category).Include(u => u.AppType).FirstOrDefault(u=>u.Id==id);
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
            var obj = db.Product.Find(id);
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

            db.Product.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
