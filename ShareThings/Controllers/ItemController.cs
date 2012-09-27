using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareThings.Models;
using ShareThings.ActionFilters;
using ShareThings.Content;

namespace ShareThings.Controllers
{
    
    [AuthorizeFilter]
    public class ItemController : Controller
    {
        private ShareStuffEntities db = new ShareStuffEntities();

        //
        // GET: /Item/

        public ActionResult Index()
        {
            var items = db.Items.Where(i => i.owner_id == SessionThings.LoggedInUser.id).Include("Category");
            return View(items.ToList());
        }

        //
        // GET: /Item/Details/5

        public ActionResult Details(int id = 0)
        {
            Item item = db.Items.Single(i => i.id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /Item/Create

        public ActionResult Create()
        {
            //Get the list of categories
            List<List<Category>> allCats = new List<List<Category>>();
            List<Category> topCats = db.Categories.Where(c => c.parent_id == -1).ToList();
            foreach (Category cat in topCats)
            {
                List<Category> tmpCatList = new List<Category>();
                tmpCatList.Add(cat);
                tmpCatList.AddRange(db.Categories.Where(c => c.parent_id == cat.id).ToList());
                allCats.Add(tmpCatList);
            }
            ViewBag.Categories = allCats;
            ViewBag.owner_id = new SelectList(db.Users, "id", "email");
            return View();
        }

        //
        // POST: /Item/Create

        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                item.owner_id = ShareThings.Content.SessionThings.LoggedInUser.id;
                db.Items.AddObject(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Get the list of categories
            List<List<Category>> allCats = new List<List<Category>>();
            List<Category> topCats = db.Categories.Where(c => c.parent_id == -1).ToList();
            foreach (Category cat in topCats)
            {
                List<Category> tmpCatList = new List<Category>();
                tmpCatList.Add(cat);
                tmpCatList.AddRange(db.Categories.Where(c => c.parent_id == cat.id).ToList());
                allCats.Add(tmpCatList);
            }
            ViewBag.Categories = allCats;
            ViewBag.owner_id = new SelectList(db.Users, "id", "email", item.owner_id);
            return View(item);
        }

        //
        // GET: /Item/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item item = db.Items.Single(i => i.id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            //Get the list of categories
            List<List<Category>> allCats = new List<List<Category>>();
            List<Category> topCats = db.Categories.Where(c => c.parent_id == -1).ToList();
            foreach (Category cat in topCats)
            {
                List<Category> tmpCatList = new List<Category>();
                tmpCatList.Add(cat);
                tmpCatList.AddRange(db.Categories.Where(c => c.parent_id == cat.id).ToList());
                allCats.Add(tmpCatList);
            }
            ViewBag.Categories = allCats;
            ViewBag.Users = db.Users.ToList();
            ViewBag.owner_id = new SelectList(db.Users, "id", "email", item.owner_id);
            return View(item);
        }

        //
        // POST: /Item/Edit/5

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Attach(item);
                db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Get the list of categories
            List<List<Category>> allCats = new List<List<Category>>();
            List<Category> topCats = db.Categories.Where(c => c.parent_id == -1).ToList();
            foreach (Category cat in topCats)
            {
                List<Category> tmpCatList = new List<Category>();
                tmpCatList.Add(cat);
                tmpCatList.AddRange(db.Categories.Where(c => c.parent_id == cat.id).ToList());
                allCats.Add(tmpCatList);
            }
            ViewBag.Categories = allCats;
            ViewBag.owner_id = new SelectList(db.Users, "id", "email", item.owner_id);
            return View(item);
        }

        //
        // GET: /Item/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item item = db.Items.Single(i => i.id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Single(i => i.id == id);
            db.Items.DeleteObject(item);
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