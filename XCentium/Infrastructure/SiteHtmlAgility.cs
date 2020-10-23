using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

namespace XCentium.Infrastructure
{
    public class SiteHtmlAgility
    {
        private HtmlDocument _document { get; set; }
        private Uri _pageUrl { get; set; }
        private HtmlWeb _web { get; set; }
        public SiteHtmlAgility(string url)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _pageUrl = new Uri(url);
            _web = new HtmlWeb();

        }
        private HtmlDocument Load()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            _document = _web.Load(_pageUrl);
            stopWatch.Stop();
            return _document;
        }
        private List<string> ExtractText()
        {
            List<string> textWordList = new List<string>();
            var root = _document.DocumentNode;
            char[] charsToSplit = { ' ', '\n', '\r', '\t', ':', ',', '.', '(', ')', '[', ']', '{', '}', '&' };

            var wordList = root.InnerText.ToLower().Split(charsToSplit);
            foreach (var word in wordList)
            {
                if (!String.IsNullOrWhiteSpace(word.Trim()))
                    textWordList.Add(word.Trim());
            }
            return textWordList;
        }

        private List<HtmlAttribute> ExtractImages()
        {
            var imageNodes = _document.DocumentNode.SelectNodes("//img").ToArray();
            var allImageAttributes = new List<HtmlAttribute>();
            foreach (var imageNode in imageNodes)
            {
                var imageAttributes = imageNode.Attributes.ToList();
                foreach (var attribute in imageAttributes)
                {

                    if (attribute.Name == "src")
                    {
                        if (!attribute.Value.Trim().StartsWith("https://"))
                        {
                            attribute.Value = _pageUrl.Scheme + "://" + _pageUrl.Host + attribute.Value;
                        }
                        Console.WriteLine(attribute.Value);
                    }
                }

            }
            return allImageAttributes;
        }
    }
}