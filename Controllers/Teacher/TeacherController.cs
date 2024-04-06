using LabMaintanance6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabMaintanance6.Controllers.Teacher
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        private LabMaintanance4Entities db = new LabMaintanance4Entities();
        public ActionResult Index()
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
            var user = db.AllUsers.SingleOrDefault(u => u.user_id == userId);
            var model = new LabMaintanance6.Models.AllUser
            {
                user_id = user.user_id,
                // Assign other properties as needed
            };


            // Authorization succeeded, continue with the action logic
            return View(model);
        }

        // Rest of your action methods...

        // GET: Teacher/Details/5
        public ActionResult Details(int id)
        {
            // Retrieve user ID from session
            int? userId = Session["UserId"] as int?;
            int? roleId = Session["RoleId"] as int?;

            // Perform authorization logic using the session's UserId and RoleId
            // For example, check if the user is authenticated and has the required role
            if (userId == null || roleId != 1)
            {
                // Authorization failed, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }

            // Authorization succeeded, continue with the action logic
            return View();
        }

        // Rest of your action methods...

        // Add other action methods with authorization as required

    }
}
