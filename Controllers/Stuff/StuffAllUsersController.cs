using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LabMaintanance6.Models;

namespace LabMaintanance6.Controllers.Stuff
{
    public class StuffAllUsersController : Controller
    {
        private LabMaintanance4Entities db = new LabMaintanance4Entities();

        // GET: StuffAllUsers
        public ActionResult Index()
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 2)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }

            var allUsers = db.AllUsers
                     .Include(a => a.Role)
                      .Where(a => a.status && a.user_id == userId) // Filter users with status = true and matching user ID
                              .ToList();


            return View(allUsers.ToList());
        }




        // GET: StuffAllUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;

            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;



            // Check if the requested ID is the same as the user's ID
            if (id != userId)
            {
                // Redirect to the Index page in the Home controller if IDs do not match
                return RedirectToAction("Index", "Home");
            }

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 2)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AllUser allUser = db.AllUsers.Find(id);
            if (allUser == null)
            {
            }

            return View();
        }

        // POST: StuffAllUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AllUser updatedPassword)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 2)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {

                var existingpassword = db.AllUsers.Find(id);
                if (existingpassword == null)
                {
                    return HttpNotFound();
                }
                existingpassword.password = updatedPassword.password;
                existingpassword.hashPassword = ComputeHash(updatedPassword.password);
                db.Entry(existingpassword).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(updatedPassword);
        }
        private string ComputeHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        // GET: StuffAllUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 2)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AllUser allUser = db.AllUsers.Find(id);
            if (allUser == null)
            {
                return HttpNotFound();
            }

            return View(allUser);
        }

        // POST: StuffAllUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            // Retrieve role ID from session
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            if (userId == null || roleId != 2)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }

            AllUser allUser = db.AllUsers.Find(id);
            allUser.status = false; // Set the status bit to off (false)
            db.Entry(allUser).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Logout", "Home");
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
