using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareThings.Models;
using ShareThings.Content;
using System.Data.Objects;
using ShareThings.ActionFilters;

namespace ShareThings.Controllers
{
    public class HomeController : Controller
    {
        private ShareStuffEntities db = new ShareStuffEntities();

        public ActionResult Index()
        {
            return RedirectToAction("Login", "User");
        }


        [AuthorizeFilter]
        public ActionResult Panel(string filter = "local")
        {
            ViewBag.Filter = filter;
            SessionThings.IsInAdminPanel = false;
         
            //Get proper requests
            if (filter == "local")
            {
                IQueryable<Request> qrequests = db.Requests.Where(r => r.isAccepted == false && r.location_id == SessionThings.LoggedInUser.location_id);
                List<Request> requests = qrequests.ToList();
                ViewBag.RequestList = requests.ToArray();
            }
            else if (filter == "all")
            {
                Request[] requests = db.Requests.Where(r => r.isAccepted == false).ToArray();
                ViewBag.RequestList = requests;
            }

            return View();
        }

    }
}
