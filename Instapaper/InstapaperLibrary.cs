using ApiLibs.General;
using ApiLibs.Instapaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Martijn.Extensions;
using System.Net.NetworkInformation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using HtmlAgilityPack;
using Windows.Storage;
using Windows.Web.Http;
using System.Web;
using Martijn.Extensions.AsyncLinq;

namespace Instapaper
{
    public class InstapaperLibrary
    {
        private static InstapaperLibrary _library;

        public static InstapaperLibrary Library { get
            {
                _library = _library ?? new InstapaperLibrary();
                return _library;
            }
        }

        private InstapaperLibrary() { }

        public InstapaperService Instapaper { get; private set; }
        public LocalMemory mem = new LocalMemory();

        public void SetService(InstapaperService instapaper)
        {
            Instapaper = instapaper;
        }

        public async Task StoreBookmarks(DownloadSettings settings)
        {
            List<Bookmark> bms = new List<Bookmark>();
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            if(!isInternetConnected)
            {
                return;
            }



            try
            {
                await ExecuteSavedActions();
                var imagesFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);

                List<Folder> folders = await Instapaper.GetFolders();

                List<KeyValuePair<string, string>> allFolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("My Articles", "unread"),
                    new KeyValuePair<string, string>("Starred", "starred"),
                    new KeyValuePair<string, string>("Archived", "archive"),
                };

                allFolders.AddRange(folders.Select(i => new KeyValuePair<string, string>(i.title, i.folder_id.ToString())));

                var folderDict = new Dictionary<string, string>(allFolders);
                await mem.Write("folders.json", folderDict);

                foreach (var folder in allFolders)
                {
                    int downloadLimit = settings.DownloadFolder;
                    if (folder.Value == "unread")
                    {
                        downloadLimit = settings.DownloadMain;
                    }
                    else if (folder.Value == "starred")
                    {
                        downloadLimit = settings.DownloadLiked;
                    }
                    else if (folder.Value == "archive")
                    {
                        downloadLimit = settings.DownloadArchived;
                    }

                    var bookmarkInfo = await Instapaper.GetAllBookmarkInfo(folder.Value, downloadLimit);
                    await mem.Write($"bookmarks-{folder.Value}.json", bookmarkInfo);

                    foreach (var i in bookmarkInfo.bookmarks)
                    {
                        try
                        {
                            var res = await mem.ReadOrCalculate($"html-{i.bookmark_id}", () => Instapaper.GetHTML(i));
                            await ExtractAndDownloadImages(res, imagesFolder, i.bookmark_id);
                        }
                        catch(Exception e) {
                            Crashes.TrackError(e);
                        }
                        await mem.Write($"highlights-{i.bookmark_id}", bookmarkInfo.highlights.Where(j => j.bookmark_id == i.bookmark_id).ToList());
                    }
                }
            }
            catch(NoInternetException) {}

        }

        public async Task ExtractAndDownloadImages(string html, StorageFolder imagesFolder, int bookmark_id)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html); 
            await htmlDoc.DocumentNode.Descendants("img").Select(i => i.Attributes.FirstOrDefault(j => j.Name == "src")).Where(i => i != null).Select(async i =>
            {
                HttpClient client = new HttpClient();
                var res = await client.GetAsync(new Uri(i.Value));
                //res.Content.ReadAsInputStreamAsync
                var file = await imagesFolder.CreateFileAsync(bookmark_id + new Uri(i.Value).LocalPath.Replace('/', '-'));
                await FileIO.WriteBufferAsync(file, await res.Content.ReadAsBufferAsync());
                return file;
            }).WhenAll();
        }

        internal Task MoveBookmark(Bookmark bookmark, string v)
        {
            return TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Folder = v
            });
        }

        internal Task Save(Uri source)
        {
            return TryToExecute(new InstapaperAction
            {
                Action = ActionType.Save,
                Url = source.AbsoluteUri
            });
        }

        public Task<Dictionary<string, string>> GetFolders()
        {
            return mem.Read("folders.json", new Dictionary<string, string>());
        }

        public async Task<List<Bookmark>> GetBookmarks(string folder)
        {

            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            if (isInternetConnected)
            {
                try
                {
                    return await Instapaper.GetBookmarks(folder, 60);
                }
                catch (NoInternetException)
                {
                    return (await mem.Read<BookmarksObject>($"bookmarks-{folder}.json")).bookmarks;
                }
            }
            else
            {
                return (await mem.Read<BookmarksObject>($"bookmarks-{folder}.json")).bookmarks;
            }
        }

        public async Task<string> GetHtml(Bookmark mark)
        {
            try
            {
                return await mem.Read($"html-{mark.bookmark_id}");
            } 
            catch
            {
                return $"<h1>Not yet downloaded</h1><p>Still downloading this article</p>";
            }
        }

        public async Task<List<Highlight>> GetHighlights(Bookmark mark)
        {
            try
            {
                return await mem.Read<List<Highlight>>($"highlights-{mark.bookmark_id}");
            }
            catch
            {
                return new List<Highlight>();
            }
        }

        public Task Archive(Bookmark bookmark)
        {
            return TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Archive
            });
        }

        public Task Star(Bookmark bookmark)
        {
            return TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Star
            });
        }

        public Task Delete(Bookmark bookmark)
        {
            return TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Delete
            });
        }


        public Task Highlight(Bookmark selectedBookmark, string text)
        {
            return TryToExecute(new InstapaperAction
            {
                Action = ActionType.Highlight,
                Bookmark = selectedBookmark.bookmark_id,
                Text = text
            });
        }

        public async Task TryOrElse(Task t, Func<Task> t2)
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            if (isInternetConnected)
            {
                try
                {
                    await t;
                }
                catch (NoInternetException)
                {
                    await t2();
                }
            }
            else
            {
                await t2();
            }
        }


        internal Task TryToExecute(InstapaperAction action) => TryOrElse(Execute(action), () => Save(action));

        private async Task Save(InstapaperAction action)
        {
            LocalMemory mem = new LocalMemory();
            var actions = await mem.Read("actions.json", new List<InstapaperAction>());
            actions.Add(action);
            await mem.Write("actions.json", actions);
        }

        private async Task ExecuteSavedActions()
        {
            LocalMemory mem = new LocalMemory();
            var actions = await mem.Read("actions.json", new List<InstapaperAction>());
            await Task.WhenAll(actions.Select(i => Execute(i)));
            await mem.Write("actions.json", new List<InstapaperAction>());
        }

        internal async Task Execute(InstapaperAction action)
        {
            switch (action.Action)
            {
                case ActionType.Archive:
                    await Instapaper.ArchiveBookmark(action.Bookmark);
                    break;
                case ActionType.UnArchive:
                    await Instapaper.UnarchiveBookmark(action.Bookmark);
                    break;
                case ActionType.Star:
                    await Instapaper.StarBookmark(action.Bookmark);
                    break;
                case ActionType.UnStar:
                    await Instapaper.UnstarBookmark(action.Bookmark);
                    break;
                case ActionType.Delete:
                    await Instapaper.DeleteBookmark(action.Bookmark);
                    break;
                case ActionType.Highlight:
                    await Instapaper.AddHighlight(action.Bookmark, action.Text);
                    break;
                case ActionType.Save:
                    await Instapaper.AddBookmark(action.Url);
                    break;
                case ActionType.Move:
                    await Instapaper.MoveBookmark(action.Bookmark, action.Folder);
                    break;
                default:
                    break;
            }
        }
    }

    public class InstapaperAction {
        public int Bookmark { get; set; }
        public ActionType Action { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Folder { get; set; }
    }

    public enum ActionType
    {
        Archive, UnArchive,
        Star,
        UnStar,
        Delete,
        Highlight,
        Save,
        Move
    }
}
