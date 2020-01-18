using Martijn.Extensions.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Instapaper
{
    public class StorageFolderMemory : AsyncMemory
    {
        protected StorageFolder ApplicationDataPath;

        public override async Task<string> Read(string filename)
        {
            StorageFile file = await ApplicationDataPath.GetFileAsync(filename);
            return await FileIO.ReadTextAsync(file);
        }

        public override async Task WriteString(string filePath, string text)
        {
            StorageFile file = await ApplicationDataPath.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, text);
        }
    }

    public class LocalMemory : StorageFolderMemory
    {
        public LocalMemory()
        {
            ApplicationDataPath = ApplicationData.Current.LocalFolder;
        }
    }

    public class RoamingMemory : StorageFolderMemory
    {
        public RoamingMemory()
        {
            ApplicationDataPath = ApplicationData.Current.RoamingFolder;
        }
    }
}
