using BuildingShop.Data;
using BuildingShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace BuildingShop.Controllers
{
    public class AppTypeController : Controller
    {
        private readonly AppDbContext db;        

        public AppTypeController(AppDbContext db)
        {
            this.db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<AppType> objList = db.AppType;
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
        public IActionResult Create(AppType obj)
        {
            db.AppType.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
