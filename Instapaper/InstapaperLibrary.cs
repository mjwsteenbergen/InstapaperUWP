using ApiLibs.General;
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
            try
            {
                var bms = await Instapaper.GetBookmarks(null, 50);

                await mem.Write("bookmarks", bms);

                foreach (var i in bms)
                {
                    try
                    {
                        await mem.ReadOrCalculate($"html-{i.bookmark_id}", () => Instapaper.GetHTML(i));
                    } catch { }
                    await mem.ReadOrCalculate($"highlights-{i.bookmark_id}", () => Instapaper.GetHighlights(i));
                }
            } 
            catch(NoInternetException) {}
            
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
