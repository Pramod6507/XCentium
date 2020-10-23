﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCentium.Infrastructure;
using XCentium.Models;

namespace XCentium.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult PageData(Site site)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", site);
            }

            else
            {
                var siteHtmlAgility = new SiteHtmlAgility(site.Url);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                siteHtmlAgility.Load();
                stopWatch.Stop();
            }
            return View(site);
        }
    }
}