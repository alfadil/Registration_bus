using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class Route
    {
        public int ID { set; get; }
        [Display(Name = "Route")]
        public string Name
        {
            get
            {
                return FromCity.Name + "-" + TOCity.Name;
            }
        }
        [Display(Name = "From")]
        public int FromCityId { set; get; }
        [Display(Name = "To")]
        public int TOCityId { set; get; }

        [ForeignKey("FromCityId")]
        [Display(Name = "From")]
        public virtual City FromCity { set; get; }

        [ForeignKey("TOCityId")]
        [Display(Name = "To")]
        public virtual City TOCity { set; get; }
    }
}