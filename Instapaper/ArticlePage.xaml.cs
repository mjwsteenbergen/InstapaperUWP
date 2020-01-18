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

            //Launch maximized
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Maximized;

            //Set stuff
            var settigns = await Settings.LoadSettings();
            Instapaper = new InstapaperLibrary(settigns.GenerateService());

            //Set below
            ContentComponent.Instapaper = Instapaper;
            await ContentComponent.Initiate();

            //Automatically go to fullscreen mode
            var scaleFactor = DisplayInformation.GetForCurrentView();
            scaleFactor.OrientationChanged += (s, ex) =>
            {
                if (s.CurrentOrientation == DisplayOrientations.Landscape || s.CurrentOrientation == DisplayOrientations.LandscapeFlipped)
                {
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                }
                else
                {
                    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                }

            };


            Window.Current.SizeChanged += (s, ex) =>
            {
                this.main.DisplayMode = ApplicationView.GetForCurrentView().IsFullScreenMode ? SplitViewDisplayMode.Overlay : SplitViewDisplayMode.CompactOverlay;
            };


            Instapaper.StoreBookmarks();
        }
    }
}
