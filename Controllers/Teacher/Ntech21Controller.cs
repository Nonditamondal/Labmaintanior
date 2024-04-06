using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using LabMaintanance6.Models;
using PagedList;
using PagedList.Mvc;

namespace LabMaintanance6.Controllers.Teacher
{
    public class Ntech21Controller : Controller
    {
        private LabMaintanance4Entities db = new LabMaintanance4Entities();

        // GET: Ntech21
        // GET: tech2
        public ActionResult Index(int? i)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 1)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            var tech21 = db.tech2.Include(t => t.Complain)
                                .Where(t => t.status == true)
                                .ToList()
                                .OrderByDescending(c => c.action_id) // Order by a specific property, such as Id
                .ToList()
                .ToPagedList(i ?? 1, 3);


            return View(tech21);
        }

        
        // GET: Ntech21/Create
        public ActionResult Create()
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 1)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Ntech21/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "action_id,complain_id,technicianName,action_description,action_date,status")] tech2 tech2, int? id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 1)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                tech2.complain_id = (int)id;
                tech2.status = true;
                tech2.user_id = userId.Value;
                db.tech2.Add(tech2);
                db.SaveChanges();
                // Get the list of users with role_id = 2
                var users = db.AllUsers.Where(u => u.role_id == 1 && u.status == true).ToList();

                // Send email to each user
                try
                {
                    foreach (var user in users)
                    {
                        SendEmail(user.email, "Labmaintanior", "Sir, A new Action has been created. Please review it.");
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.complain_id = new SelectList(db.Complains, "complain_id", "Name_Of_the_Item", tech2.complain_id);
            return View(tech2);
        }

        private void SendEmail(string recipient, string subject, string body)
        {
            // Configure the SMTP client
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("soumaykanti2859@gmail.com", "ssatuysevdkgrthi");

            // Create the email message
            var message = new MailMessage();
            message.From = new MailAddress("soumaykanti2859@gmail.com");
            message.To.Add(new MailAddress(recipient));
            message.Subject = subject;
            message.Body = body;

            // Send the email
            smtpClient.Send(message);
        }
        public ActionResult Delete(int? id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 1)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tech2 tech2 = db.tech2.Find(id);
            if (tech2 == null)
            {
                return HttpNotFound();
            }
            return View(tech2);
        }

        // POST: tech2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 1)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            tech2 tech2 = db.tech2.Find(id);
            tech2.status = false;
            db.Entry(tech2).State = EntityState.Modified;
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
    }
}
