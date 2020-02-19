﻿using ApiLibs.General;
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

        public async Task<List<Bookmark>> StoreBookmarks(DownloadSettings settings)
        {
            List<Bookmark> bms = new List<Bookmark>();
            try
            {
                await ExecuteSavedActions();


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

                    var bookmarks = await Instapaper.GetBookmarks(folder.Value, downloadLimit);
                    await mem.Write($"bookmarks-{folder.Value}.json", bookmarks);

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

        internal async Task MoveBookmark(Bookmark bookmark, string v)
        {
            await TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Folder = int.Parse(v)
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
            return mem.Read<Dictionary<string, string>>("folders.json");
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
            await TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Archive
            });
        }

        public async Task Star(Bookmark bookmark)
        {
            await TryToExecute(new InstapaperAction
            {
                Bookmark = bookmark.bookmark_id,
                Action = ActionType.Star
            });
        }

        public async Task Delete(Bookmark bookmark)
        {
            await TryToExecute(new InstapaperAction
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
            await mem.Write("actions.json", new List<InstapaperAction>());
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
        public string Text { get; internal set; }
        public string Url { get; internal set; }
        public int Folder { get; internal set; }
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
