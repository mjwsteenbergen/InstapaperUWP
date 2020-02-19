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
using Windows.Graphics.Display;
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
        internal Settings Settings { get; set; }

        private Article ArticleControl;
        private StackPanel BarButtonStackpanel;

        public void Initiate()
        {

            SetBookmarks();
            this.ArticleControl.TextHighlighted += async (s, text) =>
            {
                await Instapaper.Highlight(SelectedBookmark, text);
            };

            SetSidebarVisibility(Window.Current.Bounds.Width);

            Window.Current.SizeChanged += (s, ex) =>
            {
                SetSidebarVisibility(ex.Size.Width);

                //this.main.DisplayMode = ApplicationView.GetForCurrentView().IsFullScreenMode ? SplitViewDisplayMode.Overlay : SplitViewDisplayMode.CompactOverlay;
            };
        }

        public void SetSidebarVisibility(double width)
        {
            var scaleFactor2 = DisplayInformation.GetForCurrentView();
            this.mdView.CompactModeThresholdWidth = scaleFactor2.ScreenWidthInRawPixels / 2 + 10;

            if (width < scaleFactor2.ScreenWidthInRawPixels / 2 + 10 && SelectedBookmark != null)
            {
                ArticlePage.Instance.HideSidebar();
            } else
            {
                ArticlePage.Instance.ShowSidebar();
            }
        }

        private async void SetBookmarks()
        {
            await UpdateBookMarksInView();
            await Instapaper.StoreBookmarks(Settings.DownloadSettings);
        }

        public async Task UpdateBookMarksInView()
        {
            Bookmarks.Clear();

            var list = await Instapaper.GetBookmarks(Settings.FolderId);

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

            ArticleControl.SetDetailedView(html, bm, highlights);
            SetSidebarVisibility(Window.Current.Bounds.Width);
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
            //ArticleWrapGrid.Width = listViewItem.ActualWidth;

        }

        private async void Archive(object sender, RoutedEventArgs e)
        {
            if(SelectedBookmark == null) { return; }
            await Instapaper.Archive(SelectedBookmark);
            Bookmarks.Remove(SelectedBookmark);
            UnsetArticleView();
        }

        private async void Star(object sender, RoutedEventArgs e)
        {
            if(SelectedBookmark == null) { return; }
            await Instapaper.Star(SelectedBookmark);
            Bookmarks.Remove(SelectedBookmark);
            UnsetArticleView();
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if(SelectedBookmark == null) { return; }
            await Instapaper.Delete(SelectedBookmark);
            Bookmarks.Remove(SelectedBookmark);
            UnsetArticleView();
        }

        public void UnsetArticleView()
        {
            ArticleControl.ClearText();
            SelectedBookmark = null;
            SetSidebarVisibility(Window.Current.Bounds.Width);
        }

        private void Article_Loaded(object sender, RoutedEventArgs e)
        {
            ArticleControl = sender as Article;
        }

        public static string ItemSubheaderUrl(string url) => new Uri(url).Host;
        public static string ItemSubheaderProgress(float url) => url.ToString("P");
        public static string ItemSubheaderTime(int time) => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(time).ToLocalTime().ToString();

        private void BarButtonStackpanel_Loaded(object sender, RoutedEventArgs e)
        {
            BarButtonStackpanel = sender as StackPanel;
        }
    }
}
