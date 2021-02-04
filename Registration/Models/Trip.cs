using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Range(1, 100)]
        public int num_seats { get; set; }


        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }


        [Display(Name = "Driver")]
        public string DriverID { set; get; }

        [Display(Name = "Driver")]
        public virtual ApplicationUser Driver { set; get; }

        public float Price { get; set; }

        [Display(Name = "Luggage Price")]
        public float LuggagePrice{ get; set; }

        [InverseProperty(nameof(Reserve.Trip))]
        public virtual ICollection<Reserve> Reserves { get; set; }

        [Display(Name = "Reserved")]
        public int Reserved
        {
            get
            {
                if(Reserves != null)
                {
                    return Reserves.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        [Display(Name = "Full Reserved")]
        public bool FullReserved
        {
            get
            {
                if(Reserves != null && Reserves.Count == num_seats)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }
}