using HtmlAgilityPack;
using Martijn.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Windows.Data.Xml.Dom;
using Windows.Data.Xml.Xsl;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Instapaper
{
    class HtmlToRichTextBlock
    {
        public static Page el;


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
                var para = new Paragraph();

                HtmlNodeCollection childNodes = doc.DocumentNode.Descendants("body").First().ChildNodes;
                childNodes.Where(i => i.Name != "#text").First().ChildNodes.Select(i => Layer2(i)).Select(i => Paragraphy(i)).Foreach(i => richText.Blocks.Add(i));
            }
            catch (Exception e)
            {

            }



        }

        private static Paragraph Layer1(HtmlNode htmlNode)
        {
            return htmlNode.Name switch
            {
                "p" => Paragraphy(htmlNode),
                "div" => Paragraphy(htmlNode),
                _ => Paragraphy(Layer2(htmlNode))
            };
        }

        private static Paragraph Paragraphy(IEnumerable<Inline> enumerable)
        {
            var par = new Paragraph();
            enumerable.Foreach(i => par.Inlines.Add(i));
            return par;
        }

        private static Paragraph Paragraphy(HtmlNode htmlNode)
        {
            return Paragraphy(htmlNode.ChildNodes.SelectMany(i => Layer2(i)));
        }

        private static IEnumerable<Inline> Layer2(HtmlNode htmlNode)
        {
            return htmlNode.Name switch
            {
                "img" => Wrap(SetImage(htmlNode)),
                "h1" => Wrap(new Run
                {
                    Text = htmlNode.InnerText.TrimStart(' ', '\n', '\t').ToSentenceCase(),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontWeight = FontWeights.Light,
                    FontSize = 34,
                }),
                "h2" => Wrap(new Run
                {
                    Text = htmlNode.InnerText.TrimStart(' ', '\n', '\t').ToSentenceCase(),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontWeight = FontWeights.SemiLight,
                    FontSize = 24
                }),
                "h3" => Wrap(new Run
                {
                    Text = htmlNode.InnerText.TrimStart(' ', '\n', '\t').ToSentenceCase(),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontWeight = FontWeights.Normal,
                    FontSize = 20,
                }),
                "pre" => Wrap(new Run
                {
                    FontFamily = new FontFamily("Consolas")
                }),
                "span" => Wrap(new Span(), htmlNode),
                "div" => Wrap(new Span(), htmlNode),
                "i" => Wrap(new Italic(), htmlNode),
                "b" => Wrap(new Bold(), htmlNode),
                "a" => ToHyperlink(htmlNode),
                "ol" => ToOrderedList(htmlNode),
                "ul" => UnOrderedList(htmlNode),
                "#text" => ToRun(htmlNode),
                _ => htmlNode.ChildNodes.SelectMany(i => Layer2(i)),
            };
        }

        private static Image SetImage(HtmlNode htmlNode)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(htmlNode.Attributes.FirstOrDefault(i => i.Name == "src")?.Value));
            var img = new Image
            {
                Source = bitmapImage,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            bitmapImage.ImageOpened += (sender, e) =>
            {
                img.Width = bitmapImage.PixelWidth;
                img.Height = bitmapImage.PixelHeight;
            };
            return img;
        }



        private static List<Span> ToOrderedList(HtmlNode htmlNode)
        {
            return htmlNode.ChildNodes.Where(i => i.Name == "li").Select((i, j) =>
            {
                var span = new Span();

                span.Inlines.Add(new Run
                {
                    Text = $"\t{j + 1}. "
                });
                Layer2(i).Foreach(i => span.Inlines.Add(i));
                span.Inlines.Add(new Run
                {
                    Text = $"\n"
                });

                return span;
            }).ToList();
        }

        private static List<Span> UnOrderedList(HtmlNode htmlNode)
        {
            return htmlNode.ChildNodes.Where(i => i.Name == "li").Select((i, j) =>
            {
                var span = new Span();
                span.Inlines.Add(new Run
                {
                    Text = $"\t - "
                });

                Layer2(i).Foreach(i => span.Inlines.Add(i));
                return span;
            }).ToList();
        }

        private static List<Inline> Wrap(Span span, HtmlNode htmlNode)
        {
            htmlNode.ChildNodes.SelectMany(i => Layer2(i)).Foreach(i => span.Inlines.Add(i));
            return Wrap(span);
        }

        public static List<Inline> Wrap(Inline element)
        {
            if(element is Run run && string.IsNullOrEmpty(run.Text.Trim(' ', '\t', '\n', '\r')))
            {
                return new List<Inline>();
            }

            return new List<Inline>
            {
                element
            };
        }

        public static List<Inline> Wrap(FrameworkElement element)
        {
            return new List<Inline>
                {
                    new InlineUIContainer
                    {
                        Child =  element
                    }
                };
        }

        private static List<Inline> ToHyperlink(HtmlNode htmlNode)
        {
            var href = new Uri(htmlNode.Attributes.FirstOrDefault(i => i.Name == "href")?.Value);

            return htmlNode.ChildNodes.SelectMany(i => Layer2(i)).Select(i =>
                i switch
                {
                    Run run => SetChildren(new Hyperlink
                    {
                        NavigateUri = href
                    }, run),
                    _ => i
                }).ToList();
        }

        private static Span SetChildren(Span parent, params Inline[] span)
        {
            span.Foreach(i => parent.Inlines.Add(i));
            return parent;
        }

        private static List<Inline> ToRun(HtmlNode text) => Wrap(new Run
        {
            Text = text.InnerText.TrimStart(' ', '\n', '\t')
        });
    }

    public static class StringExtensions
    {
        public static string ForceEnd(this string strin, string c)
        {
            var stri = strin.Trim(' ');

            if (!stri.EndsWith(c))
            {
                stri = stri + c;
            }

            return stri;
        }

        public static string ForceStart(this string strin, string c)
        {
            var stri = strin.Trim(' ');

            if (!stri.StartsWith(c))
            {
                stri = c + stri;
            }

            return stri;
        }

        public static string ToSentenceCase(this string input)
        {
            var allcapital = input.All(i => char.IsLetter(i) ? char.IsUpper(i) : true);

            if(allcapital)
            {
                var lowerCase = input.ToLower();
                // matches the first sentence of a string, as well as subsequent sentences
                var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
                // MatchEvaluator delegate defines replacement of setence starts to uppercase
                return r.Replace(lowerCase, s => s.Value.ToUpper());
            } else
            {
                return input;
            }
        }

        public static bool IsReallyEmpty(this string input)
        {
            return string.IsNullOrEmpty(input?.Trim(' ', '\t', '\n', '\r'));
        }

        public static string MakeStartReallyEmpty(this string input)
        {
            return input?.TrimStart(' ', '\t', '\n', '\r');
        }

        public static string TrimReally(this string input)
        {
            return input?.Trim(' ', '\t', '\n', '\r');
        }
    }
}
