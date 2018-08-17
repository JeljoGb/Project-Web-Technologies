using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ConesOfAmazonshire.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<BoardGame> BoardGameList { get; set; }
        public string Location { get; set; }

        //public List<BoardGame> WishList { get; set; }
    }
}