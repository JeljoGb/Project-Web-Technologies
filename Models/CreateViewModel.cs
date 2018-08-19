using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Models
{
    public class CreateViewModel
    {
        public BoardGame BoardGame { get; set; }
        public string UserId { get; set; }
        public Location UserLocation { get; set; }
    }
}
