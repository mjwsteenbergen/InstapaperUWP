using ApiLibs.Instapaper;
using Martijn.Extensions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Instapaper
{
    public sealed partial class MoveToFolderDialog : ContentDialog
    {
        private InstapaperLibrary Instapaper { get => InstapaperLibrary.Library; }

        public ObservableCollection<string> Folders { get; set; }

        public string SelectedFolder { get; set; }
        public bool Success { get; internal set; }

        private Dictionary<string, string> folders;

        public Bookmark bookmark;

        public MoveToFolderDialog()
        {
            Folders = new ObservableCollection<string>();
            this.InitializeComponent();
            Success = false;
            IsPrimaryButtonEnabled = false;
            Load();
        }

        public async void Load()
        {
            folders = await Instapaper.GetFolders();
            folders.Foreach(i => Folders.Add(i.Key));
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(SelectedFolder == null)
            {
                return;
            }
            await Instapaper.MoveBookmark(bookmark, folders[SelectedFolder]);
            Success = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            IsPrimaryButtonEnabled = true;
            SelectedFolder = e.AddedItems.First() as string;
        }
    }
}
