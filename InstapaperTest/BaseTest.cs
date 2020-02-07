using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Instapaper;
using Martijn.Extensions.Linq;
using Windows.UI.Xaml.Documents;
using static Instapaper.HtmlRichTextBlockv3;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace InstapaperTest
{
    
    public class BaseTest
    {
        public List<Paragraph> ToParagraph(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml($@"
<!DOCTYPE html>
<html>
<head>
</head>
<body>
    {html}
</body>
</html>
");
            List<HtmlNode> childNodes = doc.DocumentNode.Descendants("body").ToList();
            HtmlRichTextBlockv3.settings = TypographySettings.GetDefaultSettings();
            var res = HtmlRichTextBlockv3.Parse(new State(), childNodes.Where(i => i.Name != "#text").First());
            //res.paragraphs.Add(res.mygraph);
            return res.paragraphs;
        }

        public void Compare(string tree, string html)
        {
            //Assert.AreEqual(tree, ParagraphTree.Parse(ToParagraph(html)));
            //CollectionAssert.AreEqual(tree.ToCharArray(), ParagraphTree.Parse(ToParagraph(html)).ToCharArray());
            AssertSame(tree, ParagraphTree.Parse(ToParagraph(html)));
            //StringAssert.StartsWith(tree, ParagraphTree.Parse(ToParagraph(html)));
        }

        public void AssertSame(string expected, string actual)
        {
            var exl = expected.Split("\r\n").ToList();
            var acl = actual.Split("\r\n").ToList();

            if(exl.Count != acl.Count)
            {
                Assert.Fail($"Expected:\n{expected}\n\nActual:\n{actual}\n\nNot the same amount of lines. Expected had {exl.Count}. Actually was {acl.Count}");
            }

            int linen = 0;
            exl.Zip(acl, (i, j) =>
            {
                linen++;
                int charn = 0;
                i.Zip(j, (k, l) =>
                {
                    charn++;
                    if(k != l)
                    {
                        Assert.Fail($"Expected:\n{expected}\n\nActual:\n{actual}\n\n line:{linen} char:{charn} Was expecting {k}, but got {l}");
                    }
                    return true;
                }).ToList();
                return true;
            }).ToList();
        }
    }

    [TestClass]
    public class SimpleTest : BaseTest
    {
        [UITestMethod]
        public void Simple()
        {
            Compare(@"
p {
    Span {
        Run {
            'hello'
        }
    }
}", "<span>hello</span>");
        }
    }
}
