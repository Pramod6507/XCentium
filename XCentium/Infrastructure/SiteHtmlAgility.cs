using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace XCentium.Infrastructure
{
    public class SiteHtmlAgility : ISiteHtmlAgility
    {
        private HtmlDocument _document { get; set; }
        private Uri _pageUrl { get; set; }
        private HtmlWeb _web { get; set; }

        public int Load(string url)
         {

            try
            {
                _pageUrl = new Uri(url);
                _web = new HtmlWeb();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                _document = _web.Load(_pageUrl);
                return _web.StreamBufferSize;
            }
            catch (Exception)
            {
                return -1;
            }

        }
        public List<string> ExtractText()
        {

            List<string> textWordList = new List<string>();
            try
            {
                var root = _document.DocumentNode;
                char[] charsToSplit = { ' ', ';', '\n', '\r', '\t', ':', ',', '.', '(', ')', '[', ']', '{', '}', '&' };

                var wordList = root.InnerText.ToLower().Split(charsToSplit);
                foreach (var word in wordList)
                {
                    if (!String.IsNullOrWhiteSpace(word.Trim()))
                        textWordList.Add(word.Trim());
                }
                return textWordList;

            }
            catch (Exception)
            {
                textWordList.Add("No words were found on this page!");
                return textWordList;
            }
        }

        public List<HtmlAttribute> ExtractImages()
        {
            var allImageAttributes = new List<HtmlAttribute>();
            try
            {
                var imageNodes = _document.DocumentNode.SelectNodes("//img").ToList();
                imageNodes.AddRange(_document.DocumentNode.SelectNodes("//div[@style]").ToList());
                if (!(imageNodes.Count() == 0))
                {
                    foreach (var imageNode in imageNodes)
                    {
                        var imageAttributes = imageNode.Attributes.ToList();
                        foreach (var attribute in imageAttributes)
                        {
                            if (attribute.Name == "src" | (attribute.Name == "style" && attribute.Value.Contains("background-image:")))
                            {

                                //Validation to check if the image src is in relative path of the site.
                                attribute.Value = attribute.Value.Contains("background-image:") ? attribute.Value.Split(new char[] { '(', ')' })[1] : attribute.Value;
                                attribute.Value = attribute.Value.Contains("background=") ? attribute.Value.Split('=')[1] : attribute.Value;
                                attribute.Value = !attribute.Value.Trim().StartsWith("https://") && !String.IsNullOrWhiteSpace(attribute.Value) ? _pageUrl.Scheme + "://" + _pageUrl.Host + attribute.Value : attribute.Value;



                                if (!String.IsNullOrWhiteSpace(attribute.Value))
                                    allImageAttributes.Add(attribute);
                            }

                        }

                    }

                }
            }
            catch (ArgumentNullException)
            {
                return allImageAttributes;
            }
            return allImageAttributes;
        }


        public static Dictionary<string, int> ExtractFrequencyMap(List<string> wordList)
        {
            Dictionary<string, int> frequencyMap = new Dictionary<string, int>();
            try
            {
                frequencyMap = wordList.GroupBy(x => x)
                    .Where(g => g.Count() > 1).OrderByDescending(x => x.Count())
                    .ToDictionary(x => x.Key, x => x.Count());

                return frequencyMap;
            }
            catch (Exception)
            {
                return frequencyMap;
            }

        }
    }
}