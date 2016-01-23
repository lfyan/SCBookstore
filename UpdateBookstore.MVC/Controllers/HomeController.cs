using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpdateBookstore.MVC.Models;

namespace UpdateBookstore.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Acknowledgment()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}