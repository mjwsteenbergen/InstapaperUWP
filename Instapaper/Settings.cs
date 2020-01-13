﻿using ApiLibs.Instapaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instapaper
{
    class Settings
    {
        public string Instaper_ID { get; set;  }
        public string Instaper_secret { get; set; }
        public string Instaper_user_token { get; set; }
        public string Instaper_user_secret { get; set; }

        public static Task<Settings> LoadSettings()
        {
            RoamingMemory mem = new RoamingMemory();
            return mem.Read("settings.json", new Settings());
        }

        public async Task Save()
        {
            RoamingMemory mem = new RoamingMemory();
            await mem.Write("settings.json", this);
        }

        internal InstapaperService GenerateService()
        {
            return new InstapaperService(Instaper_ID, Instaper_secret, Instaper_user_token, Instaper_user_secret);
        }
    }
}
