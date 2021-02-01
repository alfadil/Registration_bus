using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Registration.Models;

namespace Registration.Controllers
{
    public class DriversController : AccountController
    {
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Drivers
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roleID = roleManager.FindByName("Driver");
            var users = from u in db.Users
                        where u.Roles.Any(r => r.RoleId == roleID.Id)
                        select u;

            return View(users.ToList());
        }

        // GET: Drivers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Number, Address = model.Address, Name = model.Name };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Driver");

                    return RedirectToAction("Index", "Drivers");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        // GET: Drivers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            ApplicationUser applicationUser = UserManager.FindById(id);

            db.Users.Remove(applicationUser);
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