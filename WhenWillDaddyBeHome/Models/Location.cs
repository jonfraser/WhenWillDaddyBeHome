using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhenWillDaddyBeHome.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double TimestampUTC { get; set; }
    }
}