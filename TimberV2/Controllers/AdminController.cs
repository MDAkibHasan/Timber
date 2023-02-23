using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimberV2.Models;

namespace TimberV2.Controllers
{
    public class AdminController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();
        // GET: Admin
        public ActionResult Index()
        {
            var sql = ("SELECT COUNT(UserID) FROM Users");
            var count = db.Database.SqlQuery<int>(sql).First();

            ViewData["users_number"] = count;

             sql = ("SELECT COUNT(ItemID) FROM Items");
           count = db.Database.SqlQuery<int>(sql).First();
            ViewData["items_number"] = count;

            sql = ("SELECT COUNT(CategoryID) FROM Categories");
            count = db.Database.SqlQuery<int>(sql).First();
            ViewData["cat_number"] = count;

            sql = ("SELECT COUNT(BlogID) FROM Blogs");
            count = db.Database.SqlQuery<int>(sql).First();
            ViewData["blogs_number"] = count;

            var orders = db.Orders.ToList();
            var sum = 0;
            foreach(var o in orders)
            {
                if (o.OrderStatus == "Confirmed")
                {

                    sum +=(int)o.TkAmount;
                }

            }
            ViewData["tk_amount"] = sum;


            sql = ("SELECT COUNT(OrderID) FROM Orders");
            count = db.Database.SqlQuery<int>(sql).First();
            ViewData["orders_number"] = count;


            sql = ("SELECT COUNT(SupplierID) FROM Suppliers");
            count = db.Database.SqlQuery<int>(sql).First();
            ViewData["sup_number"] = count;

            sql = ("SELECT COUNT(OrderID) FROM Orders Where OrderStatus='Delivered'");
            count = db.Database.SqlQuery<int>(sql).First();
            ViewData["com_number"] = count;















            return View();
        }
        public ActionResult ViewUser()
        {     
            return View(db.Users.ToList());
          


        }
        public ActionResult logOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");

        }
    }
}