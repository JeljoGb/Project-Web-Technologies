using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConesOfAmazonshire.Models
{
    public enum Condition
    {
        New = 1,
        [Display(Name = "Opened once")]
        Opened_once = 2,
        Used = 3,
        [Display(Name = "Well-worn")]
        Well_worn = 4,
        [Display(Name = "Worn out")]
        Worn_out = 5,
        Incomplete = 6,
        [Display(Name = "Empty box")]
        Empty_box = 7
    }
}
