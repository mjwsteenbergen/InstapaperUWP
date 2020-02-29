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
using Windows.UI.Popups;
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
            this.Loaded += (s, e) =>
            {
                SetSidebarVisibility(Window.Current.Bounds.Width);

                Window.Current.SizeChanged += (sender, ex) =>
                {
                    SetSidebarVisibility(ex.Size.Width);

                    //this.main.DisplayMode = ApplicationView.GetForCurrentView().IsFullScreenMode ? SplitViewDisplayMode.Overlay : SplitViewDisplayMode.CompactOverlay;
                };
            };
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
        }

        public void SetSidebarVisibility(double width)
        {
            var scaleFactor2 = DisplayInformation.GetForCurrentView();
            this.mdView.CompactModeThresholdWidth = Math.Max(scaleFactor2.ScreenWidthInRawPixels / 2 + 10, 1300);

            if (width < Math.Max(scaleFactor2.ScreenWidthInRawPixels / 2 + 10, 1300) && SelectedBookmark != null)
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



        private async void MoveToFolder(object sender, RoutedEventArgs args)
        {
            if (SelectedBookmark == null) { return; }
            await MoveToFolder(SelectedBookmark);
        }

        private async Task MoveToFolder(Bookmark bm)
        {
            var diag = new MoveToFolderDialog();
            diag.bookmark = bm;
            await diag.ShowAsync();
            if (diag.Success)
            {
                Bookmarks.Remove(bm);
                UnsetArticleView();
            }
        }

        private async void Delete(Bookmark mark = null)
        {
            var mBookmark = mark ?? SelectedBookmark;
            if(mBookmark == null) { return; }

            MessageDialog messageDialog = new MessageDialog("Are you sure you want to delete this item?", "Delete bookmark?");
            messageDialog.Commands.Add(new UICommand("Delete", new UICommandInvokedHandler(async (s) =>
            {
                await Instapaper.Delete(mBookmark);
                Bookmarks.Remove(mBookmark);
                UnsetArticleView();
            })));
            messageDialog.Commands.Add(new UICommand("No"));
            await messageDialog.ShowAsync();
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
            this.ArticleControl.TextHighlighted += async (s, text) =>
            {
                await Instapaper.Highlight(SelectedBookmark, text);
            };
        }

        public static string ItemSubheaderUrl(string url) => new Uri(url).Host;
        public static string ItemSubheaderProgress(float url) => url.ToString("P");
        public static string ItemSubheaderTime(int time) => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(time).ToLocalTime().ToString();

        private void BarButtonStackpanel_Loaded(object sender, RoutedEventArgs e)
        {
            BarButtonStackpanel = sender as StackPanel;
        }

        private void Delete(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            var s = args.SwipeControl.DataContext;
            Delete(s as Bookmark);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private async void MoveToFolderSwipe(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            await MoveToFolder(args.SwipeControl.DataContext as Bookmark);
        }
    }
}
