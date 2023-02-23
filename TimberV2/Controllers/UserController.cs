using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimberV2.Models;

namespace TimberV2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private TimberV2Entities2 db = new TimberV2Entities2();
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            user.IsAdmin = 0;
            if (ModelState.IsValid)
            {
            
                Random rnd = new Random();
                int otp = rnd.Next(100000, 999999);
                string curotp = otp.ToString();
                TempData["otp"] = curotp;
                EmailSender em = new EmailSender();
                em.To = user.UserEmail;
                em.Subject = "Verification Code";
                em.Body = $"Your Timber Verification Code is {curotp}";
                em.sendEmail();
             
                TempData["user"] = user;

                return RedirectToAction("VerifyOtp", "User");

                





                //db.Users.Add(user);
                //db.SaveChanges();

                //Session["user_email"] = user.UserEmail;
                //return RedirectToAction("Dashboard");

            }
            return View();
        }
        public ActionResult VerifyOtp()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyOTP(Otp otp)
        {
            if (otp.otpCode.ToString() == TempData["otp"].ToString())
            {
                db.Users.Add((User)TempData["user"]);
                db.SaveChanges();
              
                Session["UserName"] = ((User)TempData["user"]).UserEmail.ToString();
                return RedirectToAction("Login", "User");
            }
            else
            {
                ViewBag.msg = "Wrong OTP!!";
                TempData["otp"] = TempData["otp"];
                return View();
            }
        }

 

        public ActionResult DashBoard()
        {
            String email = Convert.ToString( Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();
            return View(user);

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TempUser tempUser) {

           
            if (ModelState.IsValid){
                var user = db.Users.Where(u => u.UserEmail.Equals(tempUser.UserEmail)&&
                u.UserPass.Equals(tempUser.UserPassword)).FirstOrDefault();
                if (user != null && user.IsAdmin==0)
                {  Session["user_email"]=user.UserEmail;
                   Session["user_id"] = user.UserID;
                    Session["user_name"] = user.UserName;
                    return RedirectToAction("Dashboard");

             
                }
                else if(user != null && user.IsAdmin == 1){
                    Session["user_email"] = user.UserEmail;
                    Session["user_isAdmin"] = user.IsAdmin;
                    return RedirectToAction("Index", "Admin");

                }
                else {
                    ViewBag.LogInFailed = "User Not Found or Password Mismatch";
                    return View();
                }
         
             

                 }
            return View();
        }

        public ActionResult logOut()
        {
            Session.Abandon();
           return RedirectToAction("Index", "Home");

        }
        public ActionResult custom_error(string error)
        {
            ViewData["error_msg"]=error;
            return View();
        }

        public ActionResult CartCount()
        {
            int uid =(int)Session["user_id"];
       
            var sql = ("SELECT COUNT(CartID) FROM DraftCarts Where UserID=" + uid);
            var count = db.Database.SqlQuery<int>(sql).First();

            ViewData["cart_number"] = count;
            
            return PartialView();

        }
   
        }
    }
