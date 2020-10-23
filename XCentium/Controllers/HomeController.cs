using System.Diagnostics;
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
                var pageDataViewModel = new PageDataViewModel();
                var siteHtmlAgility = new SiteHtmlAgility(site.Url);
                ViewBag.SiteAddress = site.Url;

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                pageDataViewModel.StreamBufferSize = siteHtmlAgility.Load();
                stopWatch.Stop();


                pageDataViewModel.TimeElapsed = stopWatch.ElapsedMilliseconds;
                pageDataViewModel.ImageAttributeList = siteHtmlAgility.ExtractImages();
                pageDataViewModel.SiteWordList = siteHtmlAgility.ExtractText();
                pageDataViewModel.FrequencyMap = SiteHtmlAgility.ExtractFrequencyMap(pageDataViewModel.SiteWordList);


                return View(pageDataViewModel);
            }            
        }
    }
}