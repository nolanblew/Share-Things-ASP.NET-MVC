using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareThings.Models;
using ShareThings.Content;
using ShareThings.ActionFilters;

namespace ShareThings.Controllers
{
    [AuthorizeFilter(Admin=true)]
    public class LocationController : Controller
    {
        private ShareStuffEntities db = new ShareStuffEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SessionThings.UserLoggedIn(true))
            {
                RedirectToAction("Login", "User");
            }
            base.OnActionExecuting(filterContext);
        }

        //
        // GET: /Location/

        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        //
        // GET: /Location/Details/5

        public ActionResult Details(int id = 0)
        {
            Location location = db.Locations.Single(l => l.id == id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Location/Create

        [HttpPost]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.AddObject(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        //
        // GET: /Location/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Location location = db.Locations.Single(l => l.id == id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Attach(location);
                db.ObjectStateManager.ChangeObjectState(location, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        //
        // GET: /Location/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Location location = db.Locations.Single(l => l.id == id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Location/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Single(l => l.id == id);
            db.Locations.DeleteObject(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}