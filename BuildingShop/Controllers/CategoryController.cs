using BuildingShop.Data;
using BuildingShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace BuildingShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext db;        

        public CategoryController(AppDbContext db)
        {
            this.db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<Category> objList = db.Category;
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
                db.Category.Add(obj);
                db.SaveChanges();
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
            var obj = db.Category.Find(id);
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
                db.Category.Update(obj);
                db.SaveChanges();
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
            var obj = db.Category.Find(id);
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
            var obj = db.Category.Find(id);
            if (obj==null)
            {
                return NotFound(); 
            }
            db.Category.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
