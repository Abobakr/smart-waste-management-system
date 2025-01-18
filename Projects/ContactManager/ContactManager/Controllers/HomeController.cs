using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
         [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Fandfps, Selçuk Üniversitesi Teknoloji Geliştirme Bölgesi";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Fandfps, Selçuk Üniversitesi Teknoloji Geliştirme Bölgesi";

            return View();
        }
    }
}