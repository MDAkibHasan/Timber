using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimberV2.Models;

namespace TimberV2.Controllers
{
    public class ReviewsController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Item).Include(r => r.User);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewID,ReviewDesc,UserID,ItemID")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", review.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", review.UserID);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", review.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", review.UserID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,ReviewDesc,UserID,ItemID")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", review.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", review.UserID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult ShowAddReview(int id)
        {
           
            var rvw = db.Reviews.Where(r => r.ItemID == id).ToList();
            int id2 = (int)Session["user_id"];
            var orderItems = db.OrderedItems.ToList();
            var orders = db.Orders.Where(u => u.UserID == id2 && u.OrderStatus == "Delivered").ToList();
         
            foreach (var o in orders)
            {
                var x = o.OrderID;
                foreach (var i in orderItems)
                {

                    if (x == i.OrderID && i.ItemID==id)
                    {

                        ViewData["can_comment"] = 1;
                    }
                }


            }



            return PartialView(rvw);
        }
        [HttpPost]
        public ActionResult ShowAddReview(FormCollection collection)
        {
            string text = collection["Review"];
            Review r = new Review
            {

                UserID =Convert.ToInt32(Session["user_id"]),
                ItemID=Convert.ToInt32(Session["cur_item"]),
                ReviewDesc=text

            };
            db.Reviews.Add(r);
            db.SaveChanges();

            return RedirectToAction("ViewProductDetail", "Items", new { @id = (int)Session["cur_item"] });
        }
    }
}