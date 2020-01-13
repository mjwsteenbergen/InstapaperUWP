using ApiLibs.Instapaper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Instapaper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var settings = await Settings.LoadSettings();

            settings.Instaper_ID = clientId.Text;
            settings.Instaper_secret = clientSecret.Text;
            settings.Instaper_user_secret = userSecret.Text;
            settings.Instaper_user_token = userToken.Text;

            InstapaperService instaper = settings.GenerateService();

            try
            {
                await instaper.GetBookmarks(null, 1);
                await settings.Save();
                this.Frame.Navigate(typeof(ArticlePage));
            }
            catch (UnauthorizedAccessException)
            {
                await new MessageDialog("Invalid credentials").ShowAsync();
            }
            catch(Exception ex)
            {
                await new MessageDialog("Another exception occured " + ex.Message).ShowAsync();
            }
        }
    }
}
