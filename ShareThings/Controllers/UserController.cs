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
    public class UserController : Controller
    {
        private ShareStuffEntities db = new ShareStuffEntities();

        //
        // GET: /User/

        [AuthorizeFilter(Admin=true)]
        public ActionResult Index()
        {
            SessionThings.IsInAdminPanel = true;
            var users = db.Users.Include("Location");
            return View(users.ToList());
        }

        //
        // GET: /User/Details/5

        [AuthorizeFilter(Admin = true)]
        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Single(u => u.id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Register()
        {
            if (!SessionThings.UserLoggedIn() || (SessionThings.UserLoggedIn(true)))
            {
                ViewBag.location_id = new SelectList(db.Locations, "id", "name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Where<User>(u => u.email == user.email).Count() == 0)
                {
                    user.password = Security.HashString(user.password);
                    //Add defaults
                    if (!SessionThings.IsUserAdmin)
                    {
                        user.isAdmin = false;
                        user.isDeleted = false;
                        user.isDisabled = false;
                        user.isFlagged = false;
                        user.reputation = 0;
                    }
                    db.Users.AddObject(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists.");
                }
            }

            ViewBag.location_id = new SelectList(db.Locations, "id", "name", user.location_id);
            return View();
        }

        //
        // GET: /User/Edit/5

        [AuthorizeFilter(Admin = true)]
        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Single(u => u.id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.location_id = new SelectList(db.Locations, "id", "name", user.location_id);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [AuthorizeFilter(Admin = true)]
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.location_id = new SelectList(db.Locations, "id", "name", user.location_id);
            return View(user);
        }


        public ActionResult Login(string redirectTo = "~/Home/Panel")
        {
            ViewBag.RedirectTo = redirectTo;
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string redirectTo = "~/Home/Panel")
        {
            if (ModelState.IsValid)
            {
                model.password = Security.HashString(model.password);
                //Check that the user is correct
                User loginUsr = db.Users.SingleOrDefault(a => a.email == model.email && a.password == model.password);

                if (loginUsr != null)
                {

                    SessionThings.LoggedInUser = loginUsr;
                    return Redirect(redirectTo); ;
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View();
        }

        public ActionResult Logout() {
            if (SessionThings.UserLoggedIn()){
                SessionThings.LoggedInUser = null;
            }
            return RedirectToAction("Login");
        }

    }
}