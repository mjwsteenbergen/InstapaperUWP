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

namespace Instapaper
{
    public class InstapaperLibrary
    {
        public InstapaperLibrary(InstapaperService instapaper)
        {
            Instapaper = instapaper;
        }

        public InstapaperService Instapaper { get; }
        public LocalMemory mem = new LocalMemory();

        public async Task<List<Bookmark>> StoreBookmarks(DownloadSettings settings)
        {
            List<Bookmark> bms = new List<Bookmark>();
            try
            {
                await ExecuteSavedActions();


                List<Folder> folders = await Instapaper.GetFolders();
                await mem.Write("folders.json", new Dictionary<string, int>(folders.Select(i => new KeyValuePair<string, int>(i.title, i.folder_id))));

                foreach (var folder in new List<string> { "unread", "starred", "archive" })
                {
                    int downloadLimit = 20;
                    if (folder == "unread")
                    {
                        downloadLimit = settings.DownloadMain;
                    } else if (folder == "starred")
                    {
                        downloadLimit = settings.DownloadLiked;
                    } else if (folder == "archive")
                    {
                        downloadLimit = settings.DownloadArchived;
                    }

                    var bookmarks = await Instapaper.GetBookmarks(folder, downloadLimit);
                    await mem.Write($"bookmarks-{folder}.json", bookmarks);

                    foreach (var i in bookmarks)
                    {
                        try
                        {
                            await mem.ReadOrCalculate($"html-{i.bookmark_id}", () => Instapaper.GetHTML(i));
                        }
                        catch { }
                        await mem.ReadOrCalculate($"highlights-{i.bookmark_id}", () => Instapaper.GetHighlights(i));
                    }
                }

                foreach (var folder in folders)
                {
                    var bookmarks = await Instapaper.GetBookmarks(settings.DownloadFolder, folder);
                    await mem.Write($"bookmarks-{folder.folder_id}.json", bookmarks);

                    foreach (var i in bookmarks)
                    {
                        try
                        {
                            await mem.ReadOrCalculate($"html-{i.bookmark_id}", () => Instapaper.GetHTML(i));
                        }
                        catch { }
                        await mem.ReadOrCalculate($"highlights-{i.bookmark_id}", () => Instapaper.GetHighlights(i));
                    }
                }

                
            }
            catch(NoInternetException) {}

            //    bms = bms ?? await Instapaper.GetBookmarks(null, 50);

            //  await mem.Write("bookmarks", bms);



            return bms;
        }

        public Task<Dictionary<string, int>> GetFolders()
        {
            return mem.Read<Dictionary<string, int>>("folders.json");
        }

        public async Task<List<Bookmark>> GetBookmarks(string folder)
        {
            try
            {
                return await Instapaper.GetBookmarks(folder, 60);
            }
            catch
            {
                return await mem.Read<List<Bookmark>>($"bookmarks-{folder}.json");
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
                return $"<h1>Not yet downloaded</h1><p>Please wait while we are downloading this article</p>";
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

        public async Task Archive(Bookmark bookmark)
        {
            await Execute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Archive
            });
        }

        public async Task Star(Bookmark bookmark)
        {
            await Execute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Star
            });
        }

        public async Task Delete(Bookmark bookmark)
        {
            await Execute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Delete
            });
        }


        internal async Task TryToExecute(InstapaperAction action)
        {
            try
            {
                await Execute(action);
            }
            catch(NoInternetException)
            {
                await Save(action);
            }
        }

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
            await mem.Write("actions.json", actions);
        }

        internal async Task Execute(InstapaperAction action)
        {
            switch (action.Action)
            {
                case ActionType.Archive:
                    Instapaper.ArchiveBookmark(action.Bookmark);
                    break;
                case ActionType.UnArchive:
                    await Instapaper.UnarchiveBookmark(action.Bookmark);
                    break;
                case ActionType.Star:
                    Instapaper.StarBookmark(action.Bookmark);
                    break;
                case ActionType.UnStar:
                    Instapaper.UnstarBookmark(action.Bookmark);
                    break;
                case ActionType.Delete:
                    await Instapaper.DeleteBookmark(action.Bookmark);
                    break;
                case ActionType.Highlight:
                    //await Instapaper.GetHighlights
                    break;
                default:
                    break;
            }
        }
    }

    public class InstapaperAction {
        public int Bookmark { get; set; }
        public ActionType Action { get; set; }
    }

    public enum ActionType
    {
        Archive, UnArchive,
        Star,
        UnStar,
        Delete,
        Highlight
    }
}
