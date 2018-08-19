using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Models
{
    public class BoardGameQueryViewModel
    {
        public List<BoardGame> boardGames;

        [Display(Name = "Genre")]
        public string BoardGameGenre { get; set; }
        public SelectList genres;

        [Display(Name = "Search titles:")]
        public string SearchTitles { get; set; }

        [Display(Name = "Locations:")]
        public string SearchLocations { get; set; }

        [Display(Name = "Users:")]
        public string SearchUsers { get; set; }

        [Display(Name = "Condition")]
        public Condition BoardGameCondition { get; set; }

        [Display(Name = "From - To")]
        public List<int?> BoardGamePriceRange;
    }
}
