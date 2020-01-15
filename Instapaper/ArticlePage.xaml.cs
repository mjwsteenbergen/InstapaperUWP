using ApiLibs.Instapaper;
using Martijn.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Memory = Martijn.Extensions.Memory.Memory;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Instapaper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticlePage : Page, INotifyPropertyChanged
    {
        private string _html;
        public string Html
        {
            get { return _html; }
            set
            {
                if (_html == value) return;

                _html = value;
                NotifyPropertyChanged(nameof(Html));
            }
        }

        public InstapaperLibrary Instapaper { get; set; }

        public ObservableCollection<Bookmark> Bookmarks { get; set; }


        public ArticlePage()
        {
            this.InitializeComponent();
            Bookmarks = new ObservableCollection<Bookmark>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private async void Load(object sender, RoutedEventArgs e)
        {
            // Remove top bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Window.Current.SizeChanged += (s, ex) =>
            {
                BookmarkList.Visibility = ApplicationView.GetForCurrentView().IsFullScreenMode ? Visibility.Collapsed : Visibility.Visible;
            };

            var settigns = await Settings.LoadSettings();
            Instapaper = new InstapaperLibrary(settigns.GenerateService());

            Instapaper.StoreBookmarks();

            var scaleFactor = DisplayInformation.GetForCurrentView();
            scaleFactor.OrientationChanged += (s, ex) =>
            {
                if (s.NativeOrientation == DisplayOrientations.Portrait || s.NativeOrientation == DisplayOrientations.Portrait)
                {
                    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                } else
                {
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                }

            };

            var bm = (await Instapaper.GetBookmarks()).Select(i => {
                i.title = HttpUtility.HtmlDecode(i.title);
                return i;
            });

            bm.Foreach(i => Bookmarks.Add(i));
        }

        private async void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count == 0)
            {
                return;
            }
            
            var bm = e.AddedItems.First() as Bookmark;
            Html = await Instapaper.GetHtml(bm);
            var highlights = await Instapaper.GetHighlights(bm);
            highlights.OrderBy(i => i.position).Foreach(i =>
            {
                Html = Html.Replace(i.text, $"<mark>{i.text}</mark>");
            });


            HtmlRichTextBlockv3.SetHtml(RichText, Html);
        }

        private void FullscreenToggle(object sender, RoutedEventArgs e)
        {
            if(ApplicationView.GetForCurrentView().IsFullScreenMode)
            {
                ExitFullscreen();
            } else
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
            RichTextBlockGrid.Width = listViewItem.ActualWidth;
            
        }
    }
}
