using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class UrlPopupControl : UserControl
    {
        public static UrlPopupControl Instance { get; internal set; }

        public UrlPopupControl()
        {
            this.InitializeComponent();
            Instance = this;
        }

        public void Show()
        {
            PopupContainer.Visibility = Visibility.Visible;
        }

        public void ShowWithUrl(Uri uri)
        {
            Show();
            wv.Navigate(uri);
            wvUrl.Text = uri.AbsoluteUri;
        }

        private void CloseHandler(object sender, TappedRoutedEventArgs e)
        {
            Close();
        }

        private void Close()
        {
            wv.Navigate(new Uri("about:blank"));
            PopupContainer.Visibility = Visibility.Collapsed;
        }

        private void StopDissmiss(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private async void OpenInBrowser(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(wv.Source);
        }

        private async void Save(object sender, TappedRoutedEventArgs e)
        {
            await InstapaperLibrary.Library.Save(wv.Source);
        }
    }
}
