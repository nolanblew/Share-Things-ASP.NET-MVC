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
    [AuthorizeFilter]
    public class RequestController : Controller
    {
        private ShareStuffEntities db = new ShareStuffEntities();

        //
        // GET: /Request/

        public ActionResult Index(int user = -1)
        {
            User usr = null;
            if (user == -1)
            {
                usr = ShareThings.Content.SessionThings.LoggedInUser;
            }
            else
            {
                try
                {
                    usr = db.Users.Single(u => u.id == user);
                }
                catch (Exception ex)
                {
                    usr = ShareThings.Content.SessionThings.LoggedInUser;
                }
            }
            ViewBag.User = usr;
            var requests = usr.Requests; //.Include("Item").Include("User").Include("Location");
            return View(requests.ToList());
        }

        //
        // GET: /Request/Details/5

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Details(int id = 0)
        {
            Request request = db.Requests.Single(r => r.id == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // GET: /Request/Create

        public ActionResult Create()
        {
            ViewBag.item_id = new SelectList(db.Items, "id", "name");
            ViewBag.requestor_id = new SelectList(db.Users, "id", "email");
            ViewBag.location_id = new SelectList(db.Locations, "id", "name");
            ViewBag.TopCategories = new List<Category>(db.Categories.Where(c => c.parent_id == -1));
            ViewBag.Locations = new List<Location>(db.Locations);
            return View();
        }

        //
        // POST: /Request/Create

        [HttpPost]
        public ActionResult Create(int itm)
        {
            Request request = new Request();
            Item item = db.Items.Single(i => i.id == itm);
            request.requestor_id = ShareThings.Content.SessionThings.LoggedInUser.id;
            request.location_id = item.User.location_id;
            request.item_id = item.id;
            request.itemWant = item.name;
            request.dateRequested = DateTime.Now;
            request.dateRecieved = DateTime.Now;
            request.dateReturned = DateTime.Now;
            request.dateWantedReturn = DateTime.Now;
            request.isAccepted = false;
            request.isCheckedOut = false;

            try
            {
                db.Requests.AddObject(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.item_id = new SelectList(db.Items, "id", "name", request.item_id);
                ViewBag.requestor_id = new SelectList(db.Users, "id", "email", request.requestor_id);
                ViewBag.location_id = new SelectList(db.Locations, "id", "name", request.location_id);
                ViewBag.TopCategories = new List<Category>(db.Categories.Where(c => c.parent_id == -1));
                return View(request);
            }
        }

        //
        // GET: /Request/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Request request = db.Requests.Single(r => r.id == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.item_id = new SelectList(db.Items, "id", "name", request.item_id);
            ViewBag.requestor_id = new SelectList(db.Users, "id", "email", request.requestor_id);
            ViewBag.location_id = new SelectList(db.Locations, "id", "name", request.location_id);
            return View(request);
        }

        //
        // POST: /Request/Edit/5

        [HttpPost]
        public ActionResult Edit(Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Attach(request);
                db.ObjectStateManager.ChangeObjectState(request, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item_id = new SelectList(db.Items, "id", "name", request.item_id);
            ViewBag.requestor_id = new SelectList(db.Users, "id", "email", request.requestor_id);
            ViewBag.location_id = new SelectList(db.Locations, "id", "name", request.location_id);
            return View(request);
        }

        //
        // GET: /Request/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Request request = db.Requests.Single(r => r.id == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // POST: /Request/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Single(r => r.id == id);
            db.Requests.DeleteObject(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [OutputCache(Duration = 10)]
        public JsonResult getSubCategory(int parentID)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<Category> cats = db.Categories.Where(c => c.parent_id == parentID).ToList();
            cats.Insert(0, new Category()
            {
                id = -1,
                name = "--All Subcategories--"
            });
            foreach (Category cat in cats)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = cat.name,
                    Value = cat.id.ToString()
                });
            }

            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult getItems(int location, int category)
        {
            List<Item> items = db.Items.Where(i => i.User.location_id == location && i.category_id == category).ToList();
            return Json(items);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult searchItems(int location, string search)
        {
            List<Item> items = db.Items.Where(i => i.User.location_id == location && (i.name.Contains(search) || i.description.Contains(search))).ToList();
            List<AutoCompleteSearchItem> returnItems = new List<AutoCompleteSearchItem>();
            foreach (Item itm in items)
            {
                returnItems.Add(new AutoCompleteSearchItem()
                {
                    id = itm.id,
                    name = itm.name,
                    condition = itm.condition,
                    description = itm.description
                });
            }
            return Json(returnItems);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult getRequestByCategories(int location, int category)
        {
            List<AutoCompleteSearchItem> returnItems = new List<AutoCompleteSearchItem>();
            List<Item> items = db.Items.Where(i => i.category_id == category && i.isPublic == true).ToList();
            foreach (Item itm in items)
            {
                returnItems.Add(new AutoCompleteSearchItem()
                {
                    name = itm.name,
                    condition = itm.condition,
                    description = itm.description,
                    details = itm.detail,
                    id = itm.id
                });
            }
            return Json(returnItems, JsonRequestBehavior.AllowGet);
        }
    }

    public class AutoCompleteSearchItem
    {
        public string name { get; set; }
        public string condition { get; set; }
        public string description { get; set; }
        public string details { get; set; }
        public int id { get; set; }
    }
}