using ApiLibs.Instapaper;
using Martijn.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Instapaper
{
    public sealed partial class ContentControl : UserControl
    {
        public ContentControl()
        {
            this.InitializeComponent();
            Bookmarks = new ObservableCollection<Bookmark>();
        }

        public InstapaperLibrary Instapaper { get; internal set; }

        public ObservableCollection<Bookmark> Bookmarks { get; set; }

        public Bookmark SelectedBookmark { get; set; }

        public async Task Initiate()
        {
            Window.Current.SizeChanged += (s, ex) =>
            {
                this.ItemsPane.IsPaneOpen = !ApplicationView.GetForCurrentView().IsFullScreenMode;
            };

            await SetBookmarks();
        }

        private async Task SetBookmarks()
        {
            Bookmarks.Clear();

            List<Bookmark> list = await Instapaper.GetBookmarks();

            Instapaper.StoreBookmarks(list);

            var bm = list.Select(i =>
            {
                i.title = HttpUtility.HtmlDecode(i.title);
                return i;
            });

            bm.Foreach(i => Bookmarks.Add(i));
        }

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            var bm = e.AddedItems.First() as Bookmark;
            SelectedBookmark = bm;
            var html = await Instapaper.GetHtml(bm);
            var highlights = await Instapaper.GetHighlights(bm);
            highlights.OrderBy(i => i.position).Foreach(i =>
            {
                html = html.Replace(i.text, $"<mark>{i.text}</mark>");
            });

            RichTextListview.ScrollIntoView(RichTextListview.Items.FirstOrDefault());

            ArticleControl.SetText(html);
        }

        private void FullscreenToggle(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().IsFullScreenMode)
            {
                ExitFullscreen();
            }
            else
            {
                EnterFullscreen();
            }
        }

        private void EnterFullscreen()
        {
            BarButtonStackpanel.Margin = new Thickness(0, 0, 0, 0);
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
        }

        private void ExitFullscreen()
        {
            BarButtonStackpanel.Margin = new Thickness(0, 32, 0, 0);
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
        }

        private void MakeGridHaveAllSpace(object sender, SizeChangedEventArgs e)
        {
            ListViewItem listViewItem = (sender as ListViewItem);
            ArticleWrapGrid.Width = listViewItem.ActualWidth;

        }

        private async void Archive(object sender, RoutedEventArgs e)
        {
            await Instapaper.Archive(SelectedBookmark);
        }

        private async void Star(object sender, RoutedEventArgs e)
        {
            await Instapaper.Star(SelectedBookmark);
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            await Instapaper.Delete(SelectedBookmark);
        }
    }
}
