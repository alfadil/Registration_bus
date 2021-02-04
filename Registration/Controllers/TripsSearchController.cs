using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Registration.Models;
namespace Registration.Controllers
{
    public class TripsSearchController : Controller
    {
        // GET: TripsSearch
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int FromCityId=0, int ToCityId=0, string DateTime="", int NumTrav=0)
        {
            ViewBag.FromCityId = new SelectList(db.Cities, "ID", "Name");
            ViewBag.TOCityId = new SelectList(db.Cities, "ID", "Name");
            ViewBag.FromCity = FromCityId;
            ViewBag.ToCity = ToCityId;
            ViewBag.DateTime = DateTime;
            ViewBag.NumTrav = NumTrav;
            var trips = db.Trips.Where(t => t.num_seats != t.Reserves.Count).Include(t => t.Driver).Include(t => t.Route);

            trips = trips.Where(t => t.Route.FromCityId == FromCityId);
            
            trips = trips.Where(t => t.Route.TOCityId == ToCityId);
            

            if (!DateTime.Equals(""))
            {
                var ruleDate = Convert.ToDateTime(DateTime).Date;
                trips = trips.Where(t => DbFunctions.TruncateTime(t.DateTime) == DbFunctions.TruncateTime(ruleDate));
            }
            if (NumTrav != 0)
            {
                trips = trips.Where(t => (t.num_seats - t.Reserves.Count) >= NumTrav);
            }
            return View(trips.ToList());
        }
        [Authorize(Roles = "Customer")]
        public ActionResult ResTrip(int? id, int? NumTrav)
        {
            if (id == null || NumTrav == null || NumTrav < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            ViewBag.trip = trip;
            ViewBag.NumTrav = NumTrav;
            return RedirectToAction("Create", "Reserves");
        }

    }
}