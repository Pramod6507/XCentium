using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XCentium.Models
{
    public class Site
    {
        public int Id { get; set; }

        
        [Display(Name ="Site address")]
        [Required(ErrorMessage ="Please enter a URL")]
        [Url]
        public string Url { get; set; }
    }
}