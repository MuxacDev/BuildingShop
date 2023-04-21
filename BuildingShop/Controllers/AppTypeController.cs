using BuildingShop_DataAccess;
using BuildingShop_Models;
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

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppType obj)
        {
            if (ModelState.IsValid)
            {
                db.AppType.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = db.AppType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppType obj)
        {
            if (ModelState.IsValid)
            {
                db.AppType.Update(obj);
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
            var obj = db.AppType.Find(id);
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
            var obj = db.AppType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            db.AppType.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
