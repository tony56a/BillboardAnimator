using ColossalFramework.IO;
using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BillboardAnimator.Utils
{
    class FileUtils
    {
        public static readonly string IMAGES_DIR = "images";
        public static readonly string VIDEOS_DIR = "videos";

        private static readonly string MODNAME = "BillboardAnimator";
        private const ulong m_workshopId = 703971825ul;
        private static string m_savedModPath = null;

        public static string GetModPath()
        {
            if (m_savedModPath == null)
            {
                PluginManager pluginManager = PluginManager.instance;

                foreach (PluginManager.PluginInfo pluginInfo in pluginManager.GetPluginsInfo())
                {
                    if (pluginInfo.name == MODNAME || pluginInfo.publishedFileID.AsUInt64 == m_workshopId)
                    {
                        m_savedModPath = pluginInfo.modPath;
                    }
                }
            }

            return m_savedModPath;
        }

        public static string GetAltPath(bool isImages)
        {
            // Originally from: https://github.com/mabako/reddit-for-city-skylines/blob/master/RedditSkylines/Configuration.cs
            // base it on the path Cities: Skylines uses
            string path = string.Format("{0}{1}{2}{3}", DataLocation.localApplicationData + Path.DirectorySeparatorChar, "ModConfig" + Path.DirectorySeparatorChar, MODNAME+Path.DirectorySeparatorChar, (isImages ? IMAGES_DIR : VIDEOS_DIR));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            return path;
        }
    }
}
