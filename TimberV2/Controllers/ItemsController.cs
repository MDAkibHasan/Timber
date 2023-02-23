using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimberV2.Models;

namespace TimberV2.Controllers
{
    public class ItemsController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();

        // GET: Items
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Category).Include(i => i.Supplier);
            return View(items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SName");

          
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemVM item)
        {
            if (ModelState.IsValid)
            {

                string path = Path.Combine(Server.MapPath("~/Content/up"), Path.GetFileName(item.Picture.FileName));

                string shortPath = Path.GetFileName(item.Picture.FileName);
                item.Picture.SaveAs(path);
            

                Item i = new Item
                { ItemID = item.ItemID,
                    ItemName = item.ItemName,
                    ItemArt = shortPath,
                    ItemPrice =item.ItemPrice,
                    ItemDesc =item.ItemDesc,
                    CategoryID=item.CategoryID,
                    SupplierID =item.SupplierID,
                    ItemQuantity=item.ItemQuantity


                };
            
                db.Items.Add(i);
                db.SaveChanges();
             
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SName", item.SupplierID);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SName", item.SupplierID);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,ItemName,ItemArt,ItemPrice,ItemDesc,CategoryID,SupplierID,ItemQuantity")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SName", item.SupplierID);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
             Item item = db.Items.Find(id);
            db.Database.ExecuteSqlCommand("Delete From DraftCarts Where ItemID=" + id);
            db.Database.ExecuteSqlCommand("Delete From OrderedItems Where ItemID=" + id);
            db.Items.Remove(item);
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
        public ActionResult ViewProductDetail(int id)
        {
            var item = db.Items.Find(id);
            Session["cur_item"] = id;
            ViewData["img_src"] = item.ItemArt;
            ViewData["item_quantity"] = item.ItemQuantity;
            ViewData["item_id"] =id.ToString();
            ViewData["user_id"] = Session["user_id"];


            return View(item);
        }

        [HttpPost]
        public ActionResult ViewProductDetail(FormCollection collection)
        {
            if (Session["user_id"] != null)
            {
                int a = Convert.ToInt32(collection["user_id"]);
                int b = Convert.ToInt32(collection["item_id"]);
                int c = Convert.ToInt32(collection["quantity"]);
                DraftCart d = new DraftCart
                {
                    UserID = a,
                    ItemID = b,
                    Quantity = c,
                    Cart_Status = "Unordered"
                };

                var drafts = db.DraftCarts.Where(x => x.ItemID == b && x.UserID == a && x.Cart_Status == "Unordered").FirstOrDefault();


                if (drafts == null)
                {
                    db.DraftCarts.Add(d);

                    db.SaveChanges();
                }
                else
                {

                    d.CartID = drafts.CartID;

                    d.Quantity = d.Quantity + drafts.Quantity;





                    db.Database.ExecuteSqlCommand("Update DraftCarts Set Quantity='" + d.Quantity + "' Where CartID=" + d.CartID);

                }

                return RedirectToAction("Index", "DraftCarts");
            }
            else
            {

                return RedirectToAction("custom_error", "User", new { @error = "You must register and Sing In to Add to Cart" });
            }


        }

            public ActionResult DataListing()
        {
            cat_item u = new cat_item();
            u.ItemView = db.Items.ToList();
            u.CategoryView= db.Categories.ToList();
         
            return View(u);
        }

        public ActionResult ShowDiscount(int id)
        {
            var xx = db.Discounts.Where(d => d.ItemID == id).ToList();
            var disc = 0;
            foreach(var x in xx)
            {

                disc += x.DiscountPercent;
            }

            ViewData["disc"] = disc;
            return PartialView();
        }

    }
}
