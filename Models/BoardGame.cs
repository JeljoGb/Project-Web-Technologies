using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Models
{
    public class BoardGame
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }

        [StringLength(60, MinimumLength = 1)]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please format your title nicely.")]
        [Required]
        public string Title { get; set; }

        [Range(0.01, 1000.00)]
        [DataType(DataType.Currency)]
        [RegularExpression(@"\d+([,\.]\d{1,2})?", ErrorMessage = "Invalid price")]
        public int Price { get; set; }

        public Condition Condition { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public string Image { get; set; }

        [Display(Name = "Date of Purchase")]
        [DataType(DataType.Date)]
        public string PurchaseDate { get; set; }


        
    }
}