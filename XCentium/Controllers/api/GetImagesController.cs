using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XCentium.Infrastructure;

namespace XCentium.Controllers.api
{
    public class GetImagesController : ApiController
    {
        private ISiteHtmlAgility _siteHtmlAgility;
        public GetImagesController(ISiteHtmlAgility siteHtmlAgility)
        {
            _siteHtmlAgility = siteHtmlAgility;
        }

        [HttpGet]
        public IHttpActionResult Images(string url)
        {
            var imgs = new List<string>();
            if (!string.IsNullOrWhiteSpace(url))
            {
                var siteLoad = _siteHtmlAgility.Load(url);
                if (siteLoad == -1)
                    return Ok(imgs);


                foreach (var item in _siteHtmlAgility.ExtractImages())
                    imgs.Add(item.Value);

            }
            return Ok(imgs);
        }
    }
}
