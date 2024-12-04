using Alturasphere_learning_Platform.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Alturasphere_learning_Platform.Controllers
{
    public class MainController : Controller
    {
       Altura_WebsiteEntities Al=new Altura_WebsiteEntities();
        Altura_Learning_WebsiteEntities obj=new Altura_Learning_WebsiteEntities();
        Videos Obj=new Videos();
        VideoOperation Op=new VideoOperation();
        // GET: Main
        public ActionResult Navigate()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(contact C)
        {
            if (ModelState.IsValid)  // Check if the form is valid
            {
                Al.contact.Add(C);
                Al.SaveChanges();
                ViewBag.Message = "Your Response Send Successfully";
            }
            return View(C);
        }
        public ActionResult DashBoard()
        {
            return View();
        }
        public ActionResult Groups()
        {
            return View();
        }
        public ActionResult OverView()
        {
            return View();
        }
        public ActionResult InterView()
        {
            return View();
        }
        public ActionResult Inbox()
        {
            return View();
        }
        public ActionResult Lesson()
        {
            return View();
        }
        public ActionResult Task()
        {
            return View();
        }
        public ActionResult Setting()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            return View();
        }

        // GET: Video Upload Form
        public ActionResult Videos()
        {
            return View();
        }

        // POST: Handle Video Upload
        [HttpPost]
        public ActionResult Videos(Videos model)
        {
            if (ModelState.IsValid)
            {
                // Create an instance of VideoOperation
                VideoOperation videoOp = new VideoOperation();

                // Call UploadVideo to insert the video into the database
                int result = videoOp.UploadVideo(model);

                // Check if the video was uploaded successfully
                if (result > 0)
                {
                    // Redirect to another page or show success message
                    TempData["Me"] = "Video Uploaded"; // For example, redirecting to Home page
                }
                else
                {
                    // Display error if video upload failed
                    ModelState.AddModelError("", "Error uploading video.");
                }
            }

            return View(model); // Return the view if model is invalid or there was an error
        }
        public ActionResult ViewVideo(int id)
        {
            VideoOperation videoOp = new VideoOperation();
            byte[] videoData = videoOp.GetVideo(id);

            if (videoData != null)
            {
                return File(videoData, "video/mp4"); // Serve video as a FileResult with MIME type "video/mp4"
            }
            else
            {
                return HttpNotFound("Video not found");
            }
        }
        public ActionResult AdminLogin()
        {
            adminLogin A = new adminLogin();
            return View(A);
        }

        [HttpPost]
        public ActionResult AdminLogin(string C1, adminLogin A)
        {
            if (isvaliduser(A))
            {
                FormsAuthentication.SetAuthCookie(A.UserName, Convert.ToBoolean(C1));
                return RedirectToAction("Videos", "Main");
            }
            else
            {
                ViewBag.Message = "Invalid Username/Password";
                return View(A);
            }
        }

        [NonAction]
        public bool isvaliduser(adminLogin adm)
        {
            return obj.adminLogin.Any(A => A.UserName.Equals(adm.UserName) && A.Password.Equals(adm.Password));
        }
        public ActionResult Login()
        {
            userLogin A = new userLogin();
            return View(A);
        }

        [HttpPost]
        public ActionResult Login(string C1, userLogin A)
        {
            if (isvaliduser(A))
            {
                FormsAuthentication.SetAuthCookie(A.UserName, Convert.ToBoolean(C1));
                return RedirectToAction("EnterId", "Main");
            }
            else
            {
                ViewBag.Message = "Invalid Username/Password";
                return View(A);
            }
        }

        [NonAction]
        public bool isvaliduser(userLogin adm)
        {
            return obj.userLogin.Any(A => A.UserName.Equals(adm.UserName) && A.Password.Equals(adm.Password));
        }
        public ActionResult Class()
        {
            return View();
        }
        public ActionResult EnterId()
        {
            return View();
        }

        // POST: EnterId (Submit form)
        [HttpPost]
        public ActionResult EnterId(int Id)
        {
            // Attempt to get video by ID
            var A = Op.GetVideo(Id); // Replace with actual DB call

            if (A == null)
            {
                // Use TempData to pass the error message across the redirect
                TempData["Message"] = "This Id not found. Please try again!";
                return RedirectToAction("EnterId");
            }
            else
            {
                return RedirectToAction("ViewVideo", new { id = Id });
            }
        }
    }
}

