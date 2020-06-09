using BikeStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BikeStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private DatabaseShopEntities1 db = new DatabaseShopEntities1();



        public ActionResult Login(string txtUserName, string txtPassword)
        {
                if ((txtUserName == "admin") && (txtPassword == "admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
        }



        public ActionResult Index()
        {
            var Items = db.Bikes;
            return View(Items);
        }
        [HttpGet]
        public ViewResult Edit(int bikeName)
        {
          var bike = db.Bikes.FirstOrDefault(b => b.Id == bikeName);
            return View(bike);
        }

        
        [HttpPost]
        public ActionResult Edit(Bikes bike)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bike).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(bike);
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Bikes bike)
        {
                db.Bikes.Add(bike);
                db.SaveChanges();
                TempData["message"] = string.Format("Велосипед \"{0}\" был сохранен", bike.Manufacturer);
                return RedirectToAction("Index");

        }

        public ActionResult Delete(int bikeid)
        {
            var b = db.Bikes.Find(bikeid);
            if (b != null)
            {
                db.Bikes.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }



    }
}