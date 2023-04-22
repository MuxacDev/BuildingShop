using BuildingShop_DataAccess;
using BuildingShop_DataAccess.Repository.IRepository;
using BuildingShop_Models;
using BuildingShop_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BuildingShop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository catRepo;        

        public CategoryController(ICategoryRepository catRepo)
        {
            this.catRepo = catRepo;
        }


        public IActionResult Index()
        {
            IEnumerable<Category> objList = catRepo.GetAll();
            return View(objList);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(ModelState.IsValid)
            {
                catRepo.Add(obj);
                catRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            var obj = catRepo.Find(id.GetValueOrDefault());
            if(obj==null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                catRepo.Update(obj);
                catRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = catRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = catRepo.Find(id.GetValueOrDefault());
            if (obj==null)
            {
                return NotFound(); 
            }
            catRepo.Remove(obj);
            catRepo.Save();
            return RedirectToAction("Index");

        }
    }
}
