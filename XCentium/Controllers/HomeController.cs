using System.Diagnostics;
using System.Web.Mvc;
using XCentium.Infrastructure;
using XCentium.Models;

namespace XCentium.Controllers
{
    public class HomeController : Controller
    {
        private ISiteHtmlAgility _siteHtmlAgility;
        public HomeController(ISiteHtmlAgility siteHtmlAgility)
        {
            _siteHtmlAgility = siteHtmlAgility;
        }
        public ActionResult Index(string error)
        {
            ViewBag.Error = error;
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

        public ActionResult PageData()
        {
            return RedirectToAction("Index");
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
                //var siteHtmlAgility = new SiteHtmlAgility();
                ViewBag.SiteAddress = site.Url;

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                pageDataViewModel.StreamBufferSize = _siteHtmlAgility.Load(site.Url);
                stopWatch.Stop();
                if (pageDataViewModel.StreamBufferSize == -1)
                {
                    return RedirectToAction("Index", new { error = "The site entered is not accessible form this applicaton due to network restrictions. Please enter a different URL." });
                }


                pageDataViewModel.TimeElapsed = stopWatch.ElapsedMilliseconds;
                pageDataViewModel.ImageAttributeList = _siteHtmlAgility.ExtractImages();
                pageDataViewModel.SiteWordList = _siteHtmlAgility.ExtractText();
                pageDataViewModel.FrequencyMap = _siteHtmlAgility.ExtractFrequencyMap(pageDataViewModel.SiteWordList);


                return View(pageDataViewModel);
            }            
        }
    }
}