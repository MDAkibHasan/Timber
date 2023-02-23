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
    public class PaymentsController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();

        // GET: Payments
        public ActionResult Index()
        {
          
            return View(db.Payments.ToList());
           
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Payments, "PaymentID", "TrxID");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentID,OrderID,TrxID,IsPaid,Type,PhoneNo")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Payments, "PaymentID", "TrxID", payment.OrderID);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Payments, "PaymentID", "TrxID", payment.OrderID);
            return PartialView(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentID,OrderID,TrxID,IsPaid,Type,PhoneNo")] Payment payment)
        {
           
            if (ModelState.IsValid)
            {     
                if (payment.IsPaid.ToString().Equals("Paid"))
                {

                    var order = db.Orders.Where(o => o.OrderID == payment.OrderID).FirstOrDefault();

                    int uid = Convert.ToInt32(order.UserID);
                    var user = db.Users.Where(u => u.UserID == uid).FirstOrDefault();
                    EmailSender em = new EmailSender();
                    em.To = user.UserEmail;
                    em.Subject = "Payment Confirmation";
                    em.Body = "Your payment has been verified ";
                    em.sendEmail();



                    int id = payment.OrderID;
                    db.Database.ExecuteSqlCommand("Update Orders Set OrderStatus='Confirmed' Where OrderID="+id);
                
                }
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Payments, "PaymentID", "TrxID", payment.OrderID);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return PartialView(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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

       
        public ActionResult getTrx()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getTrx(PaymentVM vm)
        {
            int uid = Convert.ToInt32(Session["user_id"]);
            DateTime now = DateTime.Now;
          
            Order order = new Order
            {
                UserID = uid,
                OrderTime = Convert.ToString(now),
                TkAmount = Convert.ToInt32(Session["cur_sum"]),
                OrderStatus = "Pending"
            };
            db.Orders.Add(order);

            db.SaveChanges();

            var oo = db.Orders.Where(x => x.UserID == uid && x.OrderStatus == "Pending").FirstOrDefault();


            var drafts = db.DraftCarts.Where(x => x.UserID == uid).ToList();
            var flag = 0;
            foreach (var dd in drafts)
            {
                var xxx = db.Items.Where(i => i.ItemID == dd.ItemID).FirstOrDefault();

                if (xxx.ItemQuantity >= dd.Quantity)
                {
                    OrderedItem x = new OrderedItem
                    {
                        OrderID = oo.OrderID,
                        ItemID = dd.ItemID,
                        Quantity = dd.Quantity
                    };
                    var q = xxx.ItemQuantity - dd.Quantity;

                    db.Database.ExecuteSqlCommand("Update Items Set ItemQuantity='" + q + "' Where ItemID=" + xxx.ItemID);
                    db.OrderedItems.Add(x);

                    db.SaveChanges();
                    DraftCart dx = db.DraftCarts.Find(dd.CartID);
                    db.DraftCarts.Remove(dx);

                    db.SaveChanges();



                }
                else
                {
                    flag = 1;
                }
            }
            if (flag == 1)
            {
                var user = db.Users.Where(u => u.UserID == uid).FirstOrDefault();
                EmailSender em = new EmailSender();
                em.To = user.UserEmail;
                em.Subject = "Sorry For Inconvinience";
                em.Body = "Due to item shortages one or more of your cart products have not been added to order";
                em.sendEmail();
            }
            else
            {
                var user = db.Users.Where(u => u.UserID == uid).FirstOrDefault();
                EmailSender em = new EmailSender();
                em.To = user.UserEmail;
                em.Subject = "ORder Confirmation";
                em.Body = "Your order has been received without modification stay tuned for further updates";
                em.sendEmail();

            }



            var tOrder = db.Orders.Where(x => x.UserID == uid && x.OrderStatus == "Pending").FirstOrDefault();


            Payment p = new Payment
            {
                OrderID = tOrder.OrderID,
                TrxID = vm.TrxID,
                IsPaid = "Pending",
                Type=vm.Type,
                PhoneNo=vm.PhoneNo


                   

            };
            db.Payments.Add(p);
            db.SaveChanges();




            return RedirectToAction("Index","Home");
        }

        public ActionResult Customer_Payments()
        {

            string str = (string)Session["user_email"];
            var x = db.Users.Where(u => u.UserEmail.Equals(str)).FirstOrDefault();
            int uid = x.UserID;
            var orders = db.Orders.Where(u => u.UserID == uid).ToList();
            List<Payment> listPayment = new List<Payment>();
            foreach (var xx in orders)
            {
                var tt = db.Payments.Where(i => i.OrderID == xx.OrderID).FirstOrDefault();
                listPayment.Add(tt);

            }
            return View(listPayment);

        }

    }
}
