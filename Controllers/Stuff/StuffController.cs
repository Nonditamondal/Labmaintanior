using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabMaintanance6.Controllers.Stuff
{
    public class StuffController : Controller
    {
        // GET: Stuff
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

            return View();
        }

        // GET: Stuff/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Stuff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stuff/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stuff/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Stuff/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stuff/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Stuff/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
