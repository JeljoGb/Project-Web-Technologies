using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Models
{
    public class BoardGame
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ApplicationUser User { get; set; }
        public int Price { get; set; }
        public string Condition { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Image { get; set; }
        public string PurchaseDate { get; set; }


        
    }
}