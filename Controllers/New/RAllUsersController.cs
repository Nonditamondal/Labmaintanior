using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LabMaintanance6.Models;

namespace LabMaintanance6.Controllers.New
{
    public class RAllUsersController : Controller
    {
        private LabMaintanance4Entities db = new LabMaintanance4Entities();

        // GET: RAllUsers
        public ActionResult Index()
        {
            string userEmail = Session["UserEmail"] as string;
            var allUsers = db.AllUsers.Where(u => u.email == userEmail);
            if (allUsers != null)
            {
                return View(allUsers.ToList());
            }
            else
            {
                return View("Error", "Home");
            }
        }

        public async Task<ActionResult> SendEmail(string email, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("soumaykanti2859@gmail.com", "ssatuysevdkgrthi"),
                    EnableSsl = true
                };

                using (var message = new MailMessage("soumaykanti2859@gmail.com", email)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    await smtpClient.SendMailAsync(message);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle exception and display appropriate error message
                ModelState.AddModelError($"{ex}", "Failed to send email. Please try again later.");
                return RedirectToAction("Index");
            }
        }



        public async Task<ActionResult> VerifyEmail(int? id)
        {
            string email = Session["UserEmail"] as string;
            var user = db.AllUsers.SingleOrDefault(u => u.email == email);
            if (user != null)
            {
                user.status = false;
                user.email_verified = false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
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

            // Generate and send a new verification OTP to the user's email
            int verificationOTP = GenerateOTP(); // Replace this with your logic to generate the OTP
            string emailBody = $"Your verification OTP is: {verificationOTP}";
            await SendEmail(allUser.email, "Email Verification OTP", emailBody);

            // Save the OTP and the registered email in the session for verification
            Session["EmailVerificationOTP"] = verificationOTP;
            Session["RegisteredEmail"] = allUser.email;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyEmail(string otp)
        {
            var enteredOTP = int.Parse(otp);
            var savedOTP = (int)Session["EmailVerificationOTP"];
            if (enteredOTP == savedOTP)
            {
                var email = Session["RegisteredEmail"].ToString();

                var user = db.AllUsers.SingleOrDefault(u => u.email == email);
                if (user != null)
                {
                    user.status = true;
                    user.email_verified = true;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }

                TempData["SuccessMessage"] = "Email verified successfully!";
                return RedirectToAction("Login","Home", new { id = user.user_id });
            }

            ModelState.AddModelError("", "Invalid OTP. Please try again.");
            return View();
        }

        // Helper method to generate a random 6-digit OTP (you can implement your own logic here)
        private int GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }



        public ActionResult StoreEmailInSession()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StoreEmailInSession(string email)
        {
            // Check if the email is not empty (you can add further validation if needed)
            if (!string.IsNullOrEmpty(email))
            {
                // Store the email in the session
                Session["UserEmail"] = email;
                return RedirectToAction("Index", "RAllUsers");
            }

            // Redirect back to the Getsession action
            return RedirectToAction("StoreEmailInSession");
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
