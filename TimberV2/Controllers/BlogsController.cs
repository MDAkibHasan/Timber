using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimberV2.Models;

namespace TimberV2.Controllers
{
    public class BlogsController : Controller
    {
        private TimberV2Entities2 db = new TimberV2Entities2();

        // GET: Blogs
        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return PartialView(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create(int id)
        {
            return PartialView();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogVM blog)
        {
            string path = Path.Combine(Server.MapPath("~/Content/blog"), Path.GetFileName(blog.Picture.FileName));

            string shortPath = Path.GetFileName(blog.Picture.FileName);
            blog.Picture.SaveAs(path);
            DateTime now = DateTime.Now;
            if (ModelState.IsValid)
            {
                Blog b = new Blog
                {
                    Title = blog.Title,
                    Topic = blog.Topic,
                    Body = blog.Body,
                    Time = now,
                    BlogArt = shortPath


                };



                db.Blogs.Add(b);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return PartialView(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {

                DateTime time = DateTime.Now;
                blog.Time = time;
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return PartialView(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            db.Database.ExecuteSqlCommand("Delete From Comments Where BlogID=" + id);


            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);

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

        public ActionResult UserBlogView()
        {

            return View(db.Blogs.ToList());
        }
        public ActionResult UserBlogDetails(int id)
        {
            Comment_Blog c = new Comment_Blog();
            c.BlogView = db.Blogs.Find(id);

            c.CommentView = db.Comments.Where(i => i.BlogID == id).ToList();


            Session["blog_id"] = id;
            return View(c);
        }
        [HttpPost]
        public ActionResult UserBlogDetails(FormCollection collection)
        {

            if (Session["user_id"] != null)
            {
                DateTime now = DateTime.Now;
                Comment c = new Comment
                {
                    BlogID = Convert.ToInt32(Session["blog_id"]),
                    UserID = Convert.ToInt32(Session["user_id"]),
                    Time = now,
                    CommentBody = Convert.ToString(collection["comment"])



                };
                db.Comments.Add(c);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("custom_error", "User", new { @error = "You can not comment without logging in!!!" });

            }
           return RedirectToAction("UserBlogDetails", "Blogs",new { @id =(int)Session["blog_id"] });
           

           
        }
    }
}
