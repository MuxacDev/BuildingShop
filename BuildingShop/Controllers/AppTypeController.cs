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
    public class AppTypeController : Controller
    {
        private readonly IAppTypeRepository appTypeRepo;        

        public AppTypeController(IAppTypeRepository appTypeRepo)
        {
            this.appTypeRepo = appTypeRepo;
        }


        public IActionResult Index()
        {
            IEnumerable<AppType> objList = appTypeRepo.GetAll();
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
                appTypeRepo.Add(obj);
                appTypeRepo.Save();
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
            var obj = appTypeRepo.Find(id.GetValueOrDefault());
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
                appTypeRepo.Update(obj);
                appTypeRepo.Save();
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
            var obj = appTypeRepo.Find(id.GetValueOrDefault());
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
            var obj = appTypeRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            appTypeRepo.Remove(obj);
            appTypeRepo.Save();
            return RedirectToAction("Index");

        }
    }
}
