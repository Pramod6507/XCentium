using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCentium.Infrastructure
{
    public class SiteHtmlAgility
    {
        public SiteHtmlAgility()
        {
            /* Below assigns values to ServicePointManager 
             * inorder to bypasses the security protocol issue due to not having a SSL cert on our application.
             * Details can be found at https://stackoverflow.com/questions/2859790/the-request-was-aborted-could-not-create-ssl-tls-secure-channel
             */
        }
        private static List<string> ExtractText(HtmlDocument document)
        {
            List<string> textWordList = new List<string>();
            var root = document.DocumentNode;
            char[] charsToSplit = { ' ', '\n', '\r', '\t', ':', ',', '.', '(', ')', '[', ']', '{', '}', '&' };

            var wordList = root.InnerText.ToLower().Split(charsToSplit);
            foreach (var word in wordList)
            {
                if (!String.IsNullOrWhiteSpace(word.Trim()))
                    textWordList.Add(word.Trim());
                //Console.WriteLine("{0}",word.Trim());
            }
            return textWordList;
        }

        private static List<HtmlAttribute> ExtractImages(HtmlDocument document, string pageUrl)
        {
            var imageNodes = document.DocumentNode.SelectNodes("//img").ToArray();
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
                            attribute.Value = pageUrl + attribute.Value;
                        }
                        Console.WriteLine(attribute.Value);
                    }
                }

            }
            return allImageAttributes;
        }
    }
}