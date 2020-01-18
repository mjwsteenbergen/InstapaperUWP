using HtmlAgilityPack;
using Martijn.Extensions.Linq;
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
    class HtmlRichTextBlockv3
    {
        private static TypographySettings settings;

        public static void SetHtml(RichTextBlock richText, string html, TypographySettings tsettings = null)
        {
            settings = tsettings ?? TypographySettings.GetDefaultSettings();
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
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string>
                {
                    { "type","HtmlRichTextBlockv3" }
                });
            }



        }

        private static State Parse(State state, HtmlNode htmlNode)
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
                    return state.Push(new Italic(), htmlNode);
                case "b":
                    return state.Push(new Bold 
                    { 
                        FontWeight = FontWeights.SemiBold
                    }, htmlNode);
                case "strong":
                    return state.Push(new Bold
                    {
                        FontWeight = FontWeights.SemiBold
                    }, htmlNode);
                case "em":
                    return state.Push(new Italic(), htmlNode);
                case "a" when htmlNode.Attributes.FirstOrDefault(i => i.Name == "href") != null:
                    return state.Push(new Hyperlink
                    {
                        NavigateUri = new Uri(htmlNode.Attributes.FirstOrDefault(i => i.Name == "href").Value)
                    }, htmlNode);
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
                        FontSize = 11 * settings.SizeModifier
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

        class State
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
                    CloseSpan();
                    if(mygraph != null && mygraph.Inlines.Count > 0)
                    {
                        paragraphs.Add(mygraph);
                    }
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
                Push(new Paragraph());
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
                    if (GetParagraph().Inlines.Count > 0 && (myspan is Bold || myspan is Italic || myspan is Hyperlink))
                    {
                        mygraph.Inlines.Add(new Run
                        {
                            Text = " "
                        });
                    }
                    mygraph.Inlines.Add(myspan);
                    if (myspan is Bold || myspan is Italic || myspan is Hyperlink)
                    {
                        mygraph.Inlines.Add(new Run
                        {
                            Text = " "
                        });
                    }
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
            });
            return state;
        }

        private static State UnOrderedList(HtmlNode htmlNode, State state)
        {
            Paragraph para = new Paragraph();
            htmlNode.ChildNodes.Where(i => i.Name == "li").Select((i, j) =>
            {
                var span = new Span();
                span.Inlines.Add(new Run
                {
                    Text = $"\t - "
                });
                state.Push(span);
                Parse(state, i);
                return state;
            });
            return state;
        }

        private static InlineUIContainer ToImage(HtmlNode htmlNode)
        {
            var href = htmlNode.Attributes.FirstOrDefault(i => i.Name == "src")?.Value;

            if(href == null)
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
}
