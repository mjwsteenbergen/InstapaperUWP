using ApiLibs.Instapaper;
using Martijn.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Instapaper
{
    public sealed partial class Article : UserControl, INotifyPropertyChanged
    {
        public delegate void HighlightHandler(object sender, string highlightedText);

        // Declare the event.
        public event HighlightHandler TextHighlighted;

        public event PropertyChangedEventHandler PropertyChanged;

        private string AllText;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Bookmark selectedBookmark { get; set; }

        public Article()
        {
            this.InitializeComponent();
        }

        public void SetDetailedView(string html, Bookmark bm, List<Highlight> highlights)
        {
            selectedBookmark = bm;
            NotifyPropertyChanged(nameof(selectedBookmark));

            var res = HtmlRichTextBlockv3.SetHtml(RichText, html, null, (e) =>
            {
                UrlPopupControl.Instance.ShowWithUrl(new Uri(e));
            });

            AllText = RichText.TextUpTo(RichText.ContentEnd);

            //RichText.SelectAll();
            //AllText = RichText.SelectedText.Where(i => char.IsWhiteSpace(i) || char.IsLetterOrDigit(i)).Select(i => i.ToString()).Combine((i,j) => i + j);
            RichText.Select(RichText.ContentStart, RichText.ContentStart);
            

            RichText.TextHighlighters.Clear();

            highlights.ForEach(i =>
            {
                RichText.TextHighlighters.Add(new TextHighlighter()
                {
                    Background = new SolidColorBrush(Colors.LightYellow),
                    Ranges = { new TextRange() { StartIndex = AllText.IndexOf(i.text), Length = i.text.Length } }
                });
            });

        }

        private void Highlight_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //TextHighlighted.Invoke(this, RichText.SelectedText);

            string textUpToStart = this.RichText.TextUpTo(this.RichText.SelectionStart);
            string textUpToEnd = this.RichText.TextUpTo(this.RichText.SelectionEnd);
            RichText.TextHighlighters.Add(new TextHighlighter()
            {
                Background = new SolidColorBrush(Colors.LightYellow),
                Ranges = { new TextRange() { StartIndex = AllText.IndexOf(RichText.SelectedText), Length = RichText.SelectedText.Length } }
            });
        }

        internal void ClearText()
        {
            RichText.Blocks.Clear();
            selectedBookmark = new Bookmark
            {
                title = "",
                url = "https://nntn.nl/"
            };
            NotifyPropertyChanged(nameof(selectedBookmark));
        }
    }

    static class DocumentHelper
    {
        static public string TextUpTo(this InlineCollection inlines, TextPointer pointer)
        {
            StringBuilder textUpTo = new StringBuilder();
            foreach (Inline inline in inlines)
            {
                if (inline.ElementStart.Offset > pointer.Offset)
                {
                    break;
                }
                if (inline is Run run)
                {
                    // Need some more work here to take account of run.FlowDirection and pointer.LogicalDirection.
                    textUpTo.Append(run.Text.Substring(0, Math.Max(0, Math.Min(run.Text.Length, pointer.Offset - run.ContentStart.Offset))));
                }
                else if (inline is Span span)
                {
                    string spanTextUpTo = span.Inlines.TextUpTo(pointer);
                    textUpTo.Append(spanTextUpTo);
                }
                else if (inline is LineBreak lineBreak)
                {
                    textUpTo.Append((pointer.Offset >= lineBreak.ContentEnd.Offset) ? Environment.NewLine : "");
                }
                else if (inline is InlineUIContainer uiContainer)
                {
                    textUpTo.Append(""); // empty string replacing the UI content. 
                }
                else
                {
                    throw new InvalidOperationException($"Unrecognized inline type {inline.GetType().Name}");
                }
            }
            return textUpTo.ToString();
        }

        static public string TextUpTo(this RichTextBlock rtb, TextPointer pointer)
        {
            StringBuilder textUpTo = new StringBuilder();
            foreach (Block block in rtb.Blocks)
            {
                if (block is Paragraph paragraph)
                {
                    textUpTo.Append(paragraph.Inlines.TextUpTo(pointer));
                }
                else
                {
                    throw new InvalidOperationException($"Unrecognized block type {block.GetType().Name}");
                }
            }
            return textUpTo.ToString();
        }
    }
}
