using JobPortal2.Hubs;
using JobPortal2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JobPortal2.Controllers
{
    public class HomeController : Controller
    {
        JobPortalEntities1 db = new JobPortalEntities1();

        public ActionResult Index()
        {
            return View(db.JobPosts.ToList());
        }

        public ActionResult JobList()
        {
            //return RedirectToAction("Index","job");
            return View(db.JobPosts.ToList());
        }

      
        public ActionResult ViewJob(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return HttpNotFound();
            }
            return View(jobPost);
        }

        [Authorize(Roles = "User")]
        public ActionResult ApplicantForm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return HttpNotFound();
            }
            AppliedJob appliedJod = new AppliedJob();
            appliedJod.CompanyName = jobPost.CompanyName;
            appliedJod.JobTitle = jobPost.JobTitle;
            return View(appliedJod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplicantForm([Bind(Include = "AppliedId,CompanyName,JobTitle,UserName,UserEmail,ContactNo")] AppliedJob appliedJob)
        {
            if (ModelState.IsValid)
            {
                db.AppliedJobs.Add(appliedJob);
                db.SaveChanges();

                return RedirectToAction("ViewJob");
            }
            return View();
        }


        //[Authorize(Roles=("User"))]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "ContactId,Name,Email,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();

                return RedirectToAction("Contact");
            }
            return View();
        }

        public ActionResult Details(UserCredentialModel user)
        {
            return View(user);
        }

        public ActionResult Login()
        {
            //ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName");
            string userId = User.Identity.Name;
            bool status = userId.Equals("");

            if (!status)
            {
                int user = Int32.Parse(userId);
                string role = (from usr in db.Users
                               where usr.UserId.Equals(user)
                               select usr.Role.RoleName).FirstOrDefault();
                if (role.Equals("Admin"))
                    return RedirectToAction("Index", "Admin");
                else if (role.Equals("Employee")){
                    return RedirectToAction("Index", "Employee");
                }
                else if (role.Equals("User"))
                    return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserCredentialModel user)
        {
           
            var dataModel = (from userObj in db.Users
                             where userObj.UserId.Equals(user.userId) && userObj.Password.Equals(user.password)
                             select userObj);
            bool status = dataModel.Any();
            Console.WriteLine(status);
            if (ModelState.IsValid)
            {
                if (status)
                {
                    //User usr = dataModel.First();
                    //int users = user.userId;
                    string role = (from usr in db.Users
                                   where usr.UserId.Equals(user.userId)
                                   select usr.Role.RoleName).FirstOrDefault();

                   
                    FormsAuthentication.SetAuthCookie(user.userId.ToString(), false);
                    if (role.Equals("Admin"))
                        return RedirectToAction("Index", "Admin");
                    else if (role.Equals("Employee"))
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    else if (role.Equals("User"))
                        return RedirectToAction("Index", "Admin");
                }
            }
            //ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            string userId = User.Identity.Name;
            bool status = userId.Equals("");

            if (!status)
            {
                int user = Int32.Parse(userId);
                string role = (from usr in db.Users
                               where usr.UserId.Equals(user)
                               select usr.Role.RoleName).FirstOrDefault();
                if (role.Equals("Admin"))
                    return RedirectToAction("Index", "Admin");
                else if (role.Equals("Employee"))
                {
                    return RedirectToAction("Index", "Employee");
                }
                else if (role.Equals("User"))
                    return RedirectToAction("Contact", "Home");
            }
            return RedirectToAction("Login", "Home");
        }


        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName");
            ViewBag.Country = new List<string>{
                "India",
                "Bangladesh",
                "America",
                "Malaysia",
                "Dubai",
                "USA",
                "UK"
            };
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Email,Password,ContactNo,Country,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                user.RoleId = 3;
                db.Users.Add(user);
                db.SaveChanges();
                AppHub.updateUserInfo();
                return RedirectToAction("Login");
            }
            ViewBag.Country = new List<string>{
                "India",
                "Bangladesh",
                "America",
                "Malaysia",
                "Dubai",
                "USA",
                "UK"
            };
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }
    }
}