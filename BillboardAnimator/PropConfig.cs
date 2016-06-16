using BillboardAnimator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace BillboardAnimator
{
    class PropConfig
    {
        private static readonly PropPositioningInfo bigBillboardObj = new PropPositioningInfo
        {
            numSigns = 3,
            xPos = new List<float> { -0.05f, 4f, -4.1f },
            yPos = new List<float> { 26f, 26f, 26f },
            zPos = new List<float> { -4.6f, 2.4f, 2.3f },
            rotations = new List<float> { 60f, -60f, 180f },
            xSize = new List<float> { 1.93f, 1.93f, 1.93f },
            ySize = new List<float> { 1.93f, 1.93f, 1.93f }
        };

        private static readonly PropPositioningInfo ledBillboardObj = new PropPositioningInfo
        {
            numSigns = 1,
            xPos = new List<float> { -2.26f},
            yPos = new List<float> { 14f },
            zPos = new List<float> { 1.3f },
            rotations = new List<float> { 180f },
            xSize = new List<float> { 1.98f },
            ySize = new List<float> { 1.97f }
        };

        private static readonly Dictionary<string, PropPositioningInfo> fallbackDict = new Dictionary<string, PropPositioningInfo>
        {
            { "LED Billboard", ledBillboardObj },
            { "Big Billboard A", bigBillboardObj },
            { "Big Billboard B", bigBillboardObj },
            { "Big Billboard C", bigBillboardObj },
            { "Big Billboard D", bigBillboardObj },
            { "Big Billboard E", bigBillboardObj },

        };

        private static PropConfig instance = null;
        public static PropConfig Instance()
        {
            if (instance == null)
            {
                instance = new PropConfig();
            }

            return instance;
        }

        public Dictionary<string, PropPositioningInfo> propPositioningDict;

        public PropPositioningInfo GetRouteShieldInfo(string key)
        {
            return propPositioningDict.ContainsKey(key) ? propPositioningDict[key] : null;
        }

        /// <summary>
        /// Load all options from the disk.
        /// </summary>
        public static void LoadPropInfo()
        {
            if (File.Exists("BAPropPositioningInfo.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PropPositioningInfoObject[]), new XmlRootAttribute() { ElementName = "PropPositioningInfo" });
                StreamReader reader = new StreamReader("BAPropPositioningInfo.xml");
                Dictionary<string, PropPositioningInfo> routeShieldDict = ((PropPositioningInfoObject[])serializer.Deserialize(reader)).ToDictionary(i => i.key, i => i.value);
                reader.Close();

                if (routeShieldDict != null)
                {
                    Instance().propPositioningDict = routeShieldDict;

                    LoggerUtils.Log("Loaded prop position info file.");
                }
                else
                {
                    Instance().propPositioningDict = fallbackDict;
                    LoggerUtils.LogError("Created prop position file is invalid!");
                }
            }
            else
            {
                Instance().propPositioningDict = fallbackDict;
                LoggerUtils.LogWarning("Could not load the prop position file!");
            }
        }

        /// <summary>
        /// Save all options from the disk.
        /// </summary>
        public static void SavePropInfo()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PropPositioningInfoObject[]), new XmlRootAttribute() { ElementName = "PropPositioningInfo" });
            StreamWriter writer = new StreamWriter("RouteShieldOptions.xml");

            serializer.Serialize(writer, instance.propPositioningDict.Select(kv => new PropPositioningInfoObject() { key = kv.Key, value = kv.Value }).ToArray());
            writer.Close();

            LoggerUtils.Log("Saved route shield info file.");

        }
    }

    [Serializable]
    public class PropPositioningInfoObject
    {
        //TODO: replace this( and serialization entirely ) with JSON, since dictionaries are supported by default.
        [XmlElement("PropInfoKey")]
        public string key;
        [XmlElement("PropPositingInfo")]
        public PropPositioningInfo value;
    }

    [Serializable]
    public class PropPositioningInfo
    {
        public byte numSigns = 0;

        public List<float> xPos = new List<float>();
        public List<float> yPos = new List<float>();
        public List<float> zPos = new List<float>();
        
        public List<float> rotations = new List<float>();

        public List<float> xSize = new List<float>();
        public List<float> ySize = new List<float>();
        
    }
}
