using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Registration.Models
{
    public class City
    {
        public int ID { set; get; }
        public string Name { set; get; }
        [InverseProperty(nameof(Route.FromCity))]
        public virtual ICollection<Route> FromRouts { get; set; }
        [InverseProperty(nameof(Route.TOCity))]
        public virtual ICollection<Route> TORouts { get; set; }
    }
}