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
    public class DraftCartsController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();

        // GET: DraftCarts
        public ActionResult Index()
        {
            int xx = (int)Session["user_id"];
      var drafts =db.DraftCarts.Where(d => d.UserID == xx).ToList();
            var sum = 0;
            var discountSum = 0;

            foreach(var y in drafts)
            {

                var pp = db.Items.Where(z => z.ItemID == y.ItemID).FirstOrDefault();

                var disc = db.Discounts.Where(d => d.ItemID == y.ItemID).ToList();
                var totalDiscount = 0;
                foreach (var dd in disc)
                {

                    totalDiscount += dd.DiscountPercent;
                }


                sum +=(int) pp.ItemPrice *(int) y.Quantity;
                int x =(int)Math.Ceiling((int)pp.ItemPrice * (totalDiscount / 100.0));
                discountSum += ((int)pp.ItemPrice-x) * (int)y.Quantity;



            }
            int subtotal = sum;

            int total = discountSum + 100;
            ViewData["sub_sum"] = subtotal;
            ViewData["disc_sum"] = discountSum;
            ViewData["sum"] = total;

            Session["cur_sum"] = discountSum;

//var draftCarts = db.DraftCarts.Include(d => d.Item).Include(d => d.User);
            return View(drafts.ToList());
        }

        // GET: DraftCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DraftCart draftCart = db.DraftCarts.Find(id);
            if (draftCart == null)
            {
                return HttpNotFound();
            }
            return View(draftCart);
        }

        // GET: DraftCarts/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail");
            return View();
        }

        // POST: DraftCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartID,UserID,ItemID,Quantity,Cart_Status")] DraftCart draftCart)
        {
            if (ModelState.IsValid)
            {
                db.DraftCarts.Add(draftCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", draftCart.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", draftCart.UserID);
            return View(draftCart);
        }

        // GET: DraftCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DraftCart draftCart = db.DraftCarts.Find(id);
            if (draftCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", draftCart.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", draftCart.UserID);
           
            return View(draftCart);
        }

        // POST: DraftCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartID,UserID,ItemID,Quantity,Cart_Status")] DraftCart draftCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(draftCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", draftCart.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserEmail", draftCart.UserID);
            return View(draftCart);
        }

        // GET: DraftCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DraftCart draftCart = db.DraftCarts.Find(id);
            if (draftCart == null)
            {
                return HttpNotFound();
            }
            return View(draftCart);
        }

        // POST: DraftCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DraftCart draftCart = db.DraftCarts.Find(id);
            db.DraftCarts.Remove(draftCart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCartItem(int id)
        {

            DraftCart draftCart = db.DraftCarts.Find(id);
            db.DraftCarts.Remove(draftCart);
            db.SaveChanges();

            return RedirectToAction("Index","DraftCarts");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
