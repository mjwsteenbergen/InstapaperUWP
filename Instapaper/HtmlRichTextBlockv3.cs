using HtmlAgilityPack;
using Martijn.Extensions.Linq;
using Martijn.Extensions.Text;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Instapaper
{
    public class HtmlRichTextBlockv3
    {
        public static TypographySettings settings;
        private static Action<string> OnUrl = (e) => { };

        public static List<Paragraph> SetHtml(RichTextBlock richText, string html, TypographySettings tsettings = null, Action<string> onUrl = null)
        {
            settings = tsettings ?? TypographySettings.GetDefaultSettings();
            OnUrl = onUrl ?? OnUrl;
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
                var res = Parse(new State(), childNodes.Where(i => i.Name != "#text").First());
                res.paragraphs.Add(res.mygraph);
                res.paragraphs.Foreach(i => richText.Blocks.Add(i));
                return res.paragraphs;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string>
                {
                    { "type","HtmlRichTextBlockv3" }
                });
            }
            return null;



        }

        public static State Parse(State state, HtmlNode htmlNode)
        {
            switch(htmlNode.Name)
            {
                case "iframe":
                    var wv = new WebView(WebViewExecutionMode.SeparateThread);
                    wv.Width = 1000;
                    wv.Height = 562.5;
                    wv.Navigate(new Uri(htmlNode.Attributes.FirstOrDefault(i => i.Name == "src").Value));
                    wv.NavigationCompleted += async (s, e) =>
                    {
                        var heightString = await s.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
                        int height;
                        if (int.TryParse(heightString, out height))
                        {
                            s.Height = height;
                        };
                    };

                    return state.PushInSingleParagraph(wv);
                case "h1":
                    return state.PushInSingleParagraph(new Run
                    {
                        Text = htmlNode.InnerText.TrimReally().ToSentenceCase(),
                        FontFamily = new FontFamily("Segoe UI"),
                        FontWeight = FontWeights.Light,
                        FontSize = 34 * settings.SizeModifier,
                    });
                case "h2": return state.PushInSingleParagraph(new Run
                    {
                        Text = htmlNode.InnerText.TrimReally().ToSentenceCase(),
                        FontFamily = new FontFamily("Segoe UI"),
                        FontWeight = FontWeights.SemiLight,
                        FontSize = 24 * settings.SizeModifier
                });
                case "h3":
                    return state.PushInSingleParagraph(new Run
                    {
                        Text = htmlNode.InnerText.TrimReally().ToSentenceCase(),
                        FontFamily = new FontFamily("Segoe UI"),
                        FontWeight = FontWeights.Normal,
                        FontSize = 20 * settings.SizeModifier,
                    });
                case "h4":
                    return state.PushInSingleParagraph(new Run
                    {
                        Text = htmlNode.InnerText.TrimReally().ToSentenceCase(),
                        FontFamily = new FontFamily("Segoe UI"),
                        FontWeight = FontWeights.SemiBold,
                        FontSize = 13 * settings.SizeModifier,
                    });
                case "p":
                    state.CloseParagraph();
                    return state.Push(new Span(), htmlNode);
                case "ol":
                    return ToOrderedList(htmlNode, state);
                case "ul":
                    return UnOrderedList(htmlNode, state);
                case "img":
                    return state.Push(ToImage(htmlNode), htmlNode.ParentNode.ChildNodes.Count == 1 ? new Paragraph() : null);
                case "span":
                    return state.Push(new Span(), htmlNode);
                case "i":
                    return state.PushSurroundedWithSpaces(new Italic(), htmlNode);
                case "b":
                    return state.PushSurroundedWithSpaces(new Bold 
                    { 
                        FontWeight = FontWeights.SemiBold
                    }, htmlNode);
                case "strong":
                    return state.PushSurroundedWithSpaces(new Bold
                    {
                        FontWeight = FontWeights.SemiBold
                    }, htmlNode);
                case "em":
                    return state.PushSurroundedWithSpaces(new Italic(), htmlNode);
                case "a" when htmlNode.Attributes.FirstOrDefault(i => i.Name == "href") != null:
                    Hyperlink hl = new Hyperlink();
                    hl.Click += (s, e) => { OnUrl(htmlNode.Attributes.FirstOrDefault(i => i.Name == "href").Value); };
                    return state.PushSurroundedWithSpaces(hl, htmlNode);
                case "a":
                    return state.Push(new Span(), htmlNode);
                case "mark":
                    var hyperlink = new Hyperlink
                    {
                        TextDecorations = TextDecorations.None,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(new UISettings().GetColorValue(UIColorType.AccentDark3))
                    };
                    return state.Push(hyperlink, htmlNode);
                case "#text" when !htmlNode.InnerText.IsReallyEmpty():
                    
                    return state.Push( new Run
                    {
                        Text = htmlNode.InnerText.TrimReally(),
                        FontSize = (int) (11 * settings.SizeModifier)
                    });
                case "#text":
                    return state;
                default:
                    return defaultParse(state, htmlNode);
                    
            }
        }

        private static State defaultParse(State state, HtmlNode node)
        {
            foreach (var item in node.ChildNodes)
            {
                state = Parse(state, item);
            }
            state.CloseParagraph();
            return state;
        }

        public class State
        {
            public State()
            {
                this.paragraphs = new List<Paragraph>();
            }

            public List<Paragraph> paragraphs { get; set; }
            public Paragraph mygraph { get; set; }
            public Span myspan { get; set; }

            internal State Push(Run run)
            {
                myspan = myspan ?? new Span();

                if (run != null)
                {
                    myspan.Inlines.Add(run);
                }
                return this;
            }

            internal State Push(Span inline, HtmlNode htmlNode)
            {
                CloseSpan();
                myspan = inline;

                foreach(var node in htmlNode.ChildNodes)
                {
                    Parse(this, node);
                }

                CloseSpan();
                return this;
            }

            internal State PushSurroundedWithSpaces(Span inline, HtmlNode htmlNode)
            {
                CloseSpan();
                myspan = inline;

                if(paragraphs.Count > 0)
                {
                    AddSpace();
                }

                foreach (var node in htmlNode.ChildNodes)
                {
                    Parse(this, node);
                }


                CloseSpan();

                AddSpace();
                CloseSpan();
                return this;
            }

            private void AddSpace()
            {
                Span inline = new Span();

                inline.Inlines.Add(new Run
                {
                    Text = " "
                });

                Push(inline);
            }

            internal State PushInSingleParagraph(Inline inline)
            {
                Push(inline, new Paragraph());
                mygraph = GetParagraph(new Paragraph());
                return this;
            }

            internal State PushInSingleParagraph(WebView wv)
            {
                return PushInSingleParagraph(WrapInContainer(wv));
            }

            internal State Push(Inline inline, Paragraph p = null)
            {
                mygraph = GetParagraph(p);

                if(inline != null)
                {
                    mygraph.Inlines.Add(inline);
                }
                return this;
            }

            private Paragraph GetParagraph() => GetParagraph(null);

            private Paragraph GetParagraph(Paragraph p)
            {
                if (p != null)
                {
                    p.Margin = new Thickness(0, 10 * settings.SizeModifier, 0, 10 * settings.SizeModifier);
                    CloseParagraph();
                    mygraph = p;
                    return p;
                }
                else if (mygraph == null)
                {
                    mygraph = new Paragraph
                    {
                        Margin = new Thickness(0, 10 * settings.SizeModifier, 0, 10 * settings.SizeModifier)
                    };
                    return mygraph;
                }
                else
                {
                    return mygraph;
                }
            }

            internal void CloseParagraph()
            {
                CloseSpan();
                if (mygraph != null && mygraph.Inlines.Count > 0)
                {
                    paragraphs.Add(mygraph);
                    mygraph = null;
                }
            }

            internal State Push(Paragraph p)
            {
                CloseSpan();
                GetParagraph(p);
                GetParagraph(new Paragraph());
                return this;
            }

            internal void CloseSpan()
            {
                if (myspan != null && myspan.Inlines.Count > 0)
                {
                    //if (GetParagraph().Inlines.Count > 0 && (myspan is Bold || myspan is Italic || myspan is Hyperlink))
                    //{
                    //    mygraph.Inlines.Add(new Run
                    //    {
                    //        Text = " "
                    //    });
                    //}
                    GetParagraph().Inlines.Add(myspan);
                    //if (myspan is Bold || myspan is Italic || myspan is Hyperlink)
                    //{
                    //    mygraph.Inlines.Add(new Run
                    //    {
                    //        Text = " "
                    //    });
                    //}
                    myspan = null;
                }
            }

            
        }

        private static State ToOrderedList(HtmlNode htmlNode, State state)
        {
            Paragraph para = new Paragraph();
            htmlNode.ChildNodes.Where(i => i.Name == "li").Select((i, j) =>
            {
                var span = new Span();
                span.Inlines.Add(new Run
                {
                    Text = $"\t{j+1}. "
                });
                state.Push(span);
                return Parse(state, i);
            }).ToList();
            return state;
        }

        private static State UnOrderedList(HtmlNode htmlNode, State state)
        {
            Paragraph para = new Paragraph();

            foreach (var i in htmlNode.ChildNodes)
            {
                if(i.Name != "li")
                {
                    continue;
                }

                var span = new Span();
                span.Inlines.Add(new Run
                {
                    Text = $"\t - "
                });
                state.Push(span);
                Parse(state, i);
            }
            return state;
        }

        private static InlineUIContainer ToImage(HtmlNode htmlNode)
        {
            var href = htmlNode.Attributes.FirstOrDefault(i => i.Name == "src")?.Value;

            if(string.IsNullOrEmpty(href))
            {
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage(new Uri(href));
            var img = new Image
            {
                Source = bitmapImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            bitmapImage.ImageOpened += (sender, e) =>
            {
                img.Width = Math.Min(1000, bitmapImage.PixelWidth);
                img.Height = (img.Width * bitmapImage.PixelHeight) / bitmapImage.PixelWidth;
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

            if (allcapital)
            {
                var lowerCase = input.ToLower();
                // matches the first sentence of a string, as well as subsequent sentences
                var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
                // MatchEvaluator delegate defines replacement of setence starts to uppercase
                return r.Replace(lowerCase, s => s.Value.ToUpper());
            }
            else
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

    public class ParagraphTree
    {
        public static string Parse(List<Paragraph> paragraphs)
        {
            if(paragraphs == null)
            {
                return "[]";
            }

            return "\r\n" + paragraphs.Select(p => "p {\r\n" + Parse(p.Inlines, 1) + "\r\n}").Combine((i, j) => $"{i},\r\n{j}");
        }

        private static string Parse(InlineCollection inlines, int v)
        {
            return inlines.Select(i => Parse(i, v)).Combine((i,j) => $"{i},\r\n{j}");
        }


        private static string Parse(Inline inline, int indent)
        {
            var res = "";
            if(inline is Span span)
            {
                res += "    ".Repeat(indent) + inline.GetType().Name + " {\r\n";
                res += Parse(span.Inlines, indent + 1) + "\r\n";
                res += "    ".Repeat(indent) + "}";
            } 
            else if (inline is Run run)
            {
                res += "    ".Repeat(indent) + inline.GetType().Name + " {\r\n";
                res += "    ".Repeat(indent+1) + $"'{run.Text}'\r\n";
                res += "    ".Repeat(indent) + "}";

            }
            else
            {
                res += "    ".Repeat(indent) + inline.GetType().Name + " {}";
            }

            return res;

            
        }
    }
}
