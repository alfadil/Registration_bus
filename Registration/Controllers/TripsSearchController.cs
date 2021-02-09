using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        [Authorize(Roles = "Customer")]
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
            return RedirectToAction("FillForm", "TripsSearch", new { trip= trip.TripID, NumTrav= NumTrav });
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult FillForm(int? trip, int? NumTrav)
        {
            if (trip == null || NumTrav == null || NumTrav < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip TripJbj = db.Trips.Find(trip);
            if (TripJbj == null)
            {
                return HttpNotFound();
            }
            ViewBag.trip = trip;
            ViewBag.NumTrav = NumTrav;
            ViewBag.price = TripJbj.Price * NumTrav;
            ViewBag.LuggagePrice = TripJbj.LuggagePrice;

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult FillForm(int? trip, int? NumTrav, string CardNum, string needtaxi, FormCollection fc)
        {
            if (trip == null || NumTrav == null || CardNum == null || NumTrav < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip TripJbj = db.Trips.Find(trip);
            if (TripJbj == null)
            {
                return HttpNotFound();
            }
            Reserve[] reserves = new Reserve[(int)NumTrav];
            for (int i = 0; i < NumTrav; i++)
            {
                var name  = fc.Get("name-" + i);
                if(needtaxi == null)
                {
                    needtaxi = "of";
                }
                Reserve reserve = new Reserve();
                reserve.Name = name;
                reserve.TaxiPaid = needtaxi.Equals("on");
                reserve.CardNumber = CardNum;
                reserve.CustomerID = System.Web.HttpContext.Current.User.Identity.GetUserId();
                reserve.Paid = true;
                reserve.TripID = (int)trip;
                reserve.DateTime = DateTime.Now;
                db.Reserves.Add(reserve);
                db.SaveChanges();
                db.Entry(reserve).GetDatabaseValues();
                reserves[i] = reserve;
            }
            ViewBag.reserves = reserves;
            ViewBag.Paid = true;
            return View(new { trip = 0, NumTrav = 0 });

        }

        public ActionResult InformPayment(Reserve[] reserves)
        {
            return View();
        }
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerTrips()
        {
            var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var reserves = db.Reserves.Where(t => t.CustomerID == userid).Include(t => t.Customer).Include(t => t.Trip);
            return View(reserves.ToList());
        }

        [Authorize(Roles = "Driver")]
        public ActionResult DriverTrips()
        {
            var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var trips = db.Trips.Where(t => t.DriverID == userid).Include(t => t.Driver).Include(t => t.Route);
            return View(trips.ToList());
        }

        [Authorize(Roles = "Driver")]
        public ActionResult DriverTripDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

    }
}