using HtmlAgilityPack;
using Martijn.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Instapaper
{
    class RichTextBlockv2
    {
        public static void SetHtml(RichTextBlock richText, string html)
        {
            richText.Blocks.Clear();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml($@"
<!DOCTYPE html>
<html>
<head>
</head>
<body>
    {HttpUtility.HtmlDecode(html)}
</body>
</html>
");
                HtmlNodeCollection childNodes = doc.DocumentNode.Descendants("body").First().ChildNodes;
                Parse(childNodes.Where(i => i.Name != "#text").First()).Foreach(i => richText.Blocks.Add(i));
            }
            catch (Exception e)
            {

            }



        }

        private static IEnumerable<Paragraph> Parse(HtmlNode htmlNode)
        {
            if (SpecialParse(htmlNode, out Inline i))
            {
                return Paragraphy(Listify(i));
            } else if (htmlNode.ChildNodes.Any(i => i.Name == "#text" && !i.InnerText.IsReallyEmpty())) {
                return Paragraphy(htmlNode.ChildNodes.SelectMany(i => ParseAsText(i)));
            } 
            else
            {
                return htmlNode.Name switch
                {
                    "ol" => Listify(ToOrderedList(htmlNode)),
                    "ul" => Listify(UnOrderedList(htmlNode)),
                    _ => htmlNode.ChildNodes.SelectMany(node => Parse(node))
                };
            }
        }

        private static IEnumerable<Paragraph> Paragraphy(IEnumerable<Inline> list)
        {
            Paragraph p = new Paragraph();
            list.Foreach(i => p.Inlines.Add(i));
            return Listify(p);
        }

        private static IEnumerable<Paragraph> Listify(Paragraph p) => p == null ? new List<Paragraph>() : new List<Paragraph> { p };
        private static IEnumerable<Inline> Listify(Inline i) => i == null ? new List<Inline>() : new List<Inline> { i };

        private static bool SpecialParse(HtmlNode htmlNode, out Inline i)
        {
            i = htmlNode.Name switch
            {
                "h1" => new Run
                {
                    Text = htmlNode.InnerText.TrimStart(' ', '\n', '\t').ToSentenceCase(),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontWeight = FontWeights.Light,
                    FontSize = 34,
                },
                "h2" => new Run
                {
                    Text = htmlNode.InnerText.TrimStart(' ', '\n', '\t').ToSentenceCase(),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontWeight = FontWeights.SemiLight,
                    FontSize = 24
                },
                "h3" => new Run
                {
                    Text = htmlNode.InnerText.TrimStart(' ', '\n', '\t').ToSentenceCase(),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontWeight = FontWeights.Normal,
                    FontSize = 20,
                },
                "img" => ToImage(htmlNode),
                _ => null
            };

            return i != null;
        }

        private static Paragraph ToOrderedList(HtmlNode htmlNode)
        {
            Paragraph para = new Paragraph();
            htmlNode.ChildNodes.Where(i => i.Name == "li").Select((i, j) =>
            {
                var span = new Span();

                span.Inlines.Add(new Run
                {
                    Text = $"\t{j + 1}. "
                });
                ParseAsText(i).Foreach(i => span.Inlines.Add(i));
                span.Inlines.Add(new Run
                {
                    Text = $"\n"
                });

                return span;
            }).Foreach(i => para.Inlines.Add(i));
            return para;
        }

        private static Paragraph UnOrderedList(HtmlNode htmlNode)
        {
            Paragraph para = new Paragraph();
            htmlNode.ChildNodes.Where(i => i.Name == "li").Select((i, j) =>
            {
                var span = new Span();
                span.Inlines.Add(new Run
                {
                    Text = $"\t - "
                });

                ParseAsText(i).Foreach(i => span.Inlines.Add(i));
                return span;
            }).Foreach(i => para.Inlines.Add(i));
            return para;
        }

        private static InlineUIContainer ToImage(HtmlNode htmlNode)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(htmlNode.Attributes.FirstOrDefault(i => i.Name == "src")?.Value));
            var img = new Image
            {
                Source = bitmapImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            bitmapImage.ImageOpened += (sender, e) =>
            {
                img.Width = bitmapImage.PixelWidth;
                img.Height = bitmapImage.PixelHeight;
            };
            return WrapInContainer(img);
        }

        private static InlineUIContainer WrapInContainer(FrameworkElement element)
        {
            return new InlineUIContainer
            {
                Child = element
            };
        }

        private static IEnumerable<Inline> ParseAsText(HtmlNode node)
        {
            if(SpecialParse(node, out Inline i))
            {
                return Listify(i);
            }

            return Listify(node.Name switch
            {
                "#text" when !node.InnerText.IsReallyEmpty() => new Run
                {
                    Text = node.InnerText.TrimStart(' ', '\n', '\t')
                },
                _ => null
            });
        }
    }
}
