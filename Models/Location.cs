using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Models
{
    public class Location : ApplicationUser
    {
        public int DoorNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return StreetName + " " + DoorNumber + ", " + City + ", " + Country;
        }
    }
}
