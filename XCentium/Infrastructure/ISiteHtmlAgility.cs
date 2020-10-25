using System.Collections.Generic;
using HtmlAgilityPack;

namespace XCentium.Infrastructure
{
    public interface ISiteHtmlAgility
    {
        List<HtmlAttribute> ExtractImages();
        List<string> ExtractText();
        int Load(string url);
    }
}