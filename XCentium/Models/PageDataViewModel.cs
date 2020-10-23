using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCentium.Models
{
    public class PageDataViewModel
    {
        public List<string> SiteWordList { get; set; }
        public List<HtmlAttribute> ImageAttributeList { get; set; }
        public int StreamBufferSize { get; set; }
        public long TimeElapsed { get; set; }
    }
}