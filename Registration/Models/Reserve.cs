using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class Reserve
    {
        public int ReserveID { set; get; }

        [Display(Name = "Trip")]
        public int TripID { set; get; }

        [ForeignKey("TripID")]
        [Display(Name = "Trip")]
        public virtual Trip Trip { set; get; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

        public bool Paid { get; set; }

        [DisplayName("Card Number")]
        public string CardNumber { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Paid For Taxi")]
        public bool TaxiPaid { get; set; }

        [Display(Name = "Customer")]
        public string CustomerID { set; get; }

        [Display(Name = "Customer")]
        public virtual ApplicationUser Customer { set; get; }




    }
}