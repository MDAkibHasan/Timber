using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimberV2.Models;

namespace TimberV2.Controllers
{
    public class HomeController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();
        public ActionResult Index()
        {

            var x = db.Items.SqlQuery("Select TOP 9 * from Items order by ItemID Desc ").ToList();
            cat_item c = new cat_item
            {
                CategoryView = db.Categories.ToList(),
                ItemView =x

        };


       

            return View(c);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}