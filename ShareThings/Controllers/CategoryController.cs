using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareThings.Models;
using ShareThings.ActionFilters;

namespace ShareThings.Controllers
{
    [AuthorizeFilter(Admin=true)]
    public class CategoryController : Controller
    {
        private ShareStuffEntities db = new ShareStuffEntities();

        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            Category category = db.Categories.Single(c => c.id == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            if (category.parent_id != -1)
            {
                //TODO: Fix this so that it only returns the name of the parent class
                ViewBag.ParentCategoryName = db.Categories.Single(c => c.id == category.id).name;
            }
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            //Get list of categories that are at the top
            Category[] topCategories = db.Categories.Where(c => c.parent_id == -1).ToArray();
            ViewBag.TopCategories = topCategories;
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.AddObject(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Get list of categories that are at the top
            Category[] topCategories = db.Categories.Where(c => c.parent_id == -1).ToArray();
            ViewBag.TopCategories = topCategories;
            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Category category = db.Categories.Single(c => c.id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            //Get list of categories that are at the top
            Category[] topCategories = db.Categories.Where(c => c.parent_id == -1).ToArray();
            ViewBag.TopCategories = topCategories;

            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem(){
                Text = "<No Parent Category>",
                Value = "-1",
                Selected = category.parent_id == -1
            });
            foreach (Category cat in topCategories)
            {
                lst.Add(new SelectListItem()
                {
                    Text = cat.name,
                    Value = cat.id.ToString(),
                    Selected = category.parent_id == cat.id
                });
            }
            ViewBag.CatList = lst;

            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(category);
                db.ObjectStateManager.ChangeObjectState(category, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Get list of categories that are at the top
            Category[] topCategories = db.Categories.Where(c => c.parent_id == -1).ToArray();
            ViewBag.TopCategories = topCategories;
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Category category = db.Categories.Single(c => c.id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Single(c => c.id == id);
            db.Categories.DeleteObject(category);
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