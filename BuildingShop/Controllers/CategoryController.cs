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

        //Post - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            db.Category.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
