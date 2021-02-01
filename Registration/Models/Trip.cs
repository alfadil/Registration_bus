using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class Trip
    {
        public int TripID { get; set; }


        [Display(Name = "Route")]
        public int RouteID { set; get; }

        [ForeignKey("RouteID")]
        [Display(Name = "Route")]
        public virtual Route Route { set; get; }

        [Display(Name = "Number Of Seats")]
        public int num_seats { get; set; }

        public DateTime DateTime { get; set; }


        [Display(Name = "Driver")]
        public string DriverID { set; get; }

        [Display(Name = "Driver")]
        public virtual ApplicationUser Driver { set; get; }
    }
}