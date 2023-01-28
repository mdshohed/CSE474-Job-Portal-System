using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal2.Models;

namespace JobPortal2.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private JobPortalEntities1 db = new JobPortalEntities1();

        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            ViewBag.jobPost = db.JobPosts.Count();
            ViewBag.applicant = db.AppliedJobs.Count();
            return View();
        }

        [Authorize(Roles = "Employee")]
        public ActionResult Vacancy()
        {
            return View(db.JobPosts.ToList());
        }

        [Authorize(Roles = "Employee")]
        public ActionResult Applicants()
        {
            return View(db.AppliedJobs.ToList());
        }

        [Authorize(Roles="Employee")]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        // GET: Employee/Details/5
        public ActionResult Details(int? id)
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

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobId,JobTitle,Description,Education,Experience,Specialization,LastDate,Salary,CompanyName,JobType,Email,Address,Country,State,PostedDate,Vacancy,JobNature")] JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                db.JobPosts.Add(jobPost);
                db.SaveChanges();
                return RedirectToAction("Vacancy");
            }

            return View(jobPost);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobId,JobTitle,Description,Education,Experience,Specialization,LastDate,Salary,CompanyName,JobType,Email,Address,Country,State,PostedDate,Vacancy,JobNature")] JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Vacancy", "Employee");
            }
            return View(jobPost);
        }


    }
}
