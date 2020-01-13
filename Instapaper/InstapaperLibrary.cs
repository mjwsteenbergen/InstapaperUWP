using ApiLibs.Instapaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async void StoreBookmarks()
        {
            var bms = await Instapaper.GetBookmarks(null, 50);

            await mem.Write("bookmarks", bms);

            foreach (var i in bms)
            {
                try
                {
                    await mem.Read($"html-{i.bookmark_id}");
                }
                catch
                {
                    try
                    {
                        await mem.Write($"html-{i.bookmark_id}", await Instapaper.GetHTML(i));
                    } catch { }
                }

                try
                {
                    await mem.Read($"highlights-{i.bookmark_id}");
                }
                catch
                {
                    try
                    {
                        await mem.Write($"highlights-{i.bookmark_id}", await Instapaper.GetHighlights(i));
                    }
                    catch { }
                }
            }
        }

        public async Task<List<Bookmark>> GetBookmarks()
        {
            try
            {
                return await Instapaper.GetBookmarks(null, 50);
            }
            catch
            {
                return await mem.Read<List<Bookmark>>("bookmarks");
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
    }

    public class InstapaperAction {
        
    }
}
