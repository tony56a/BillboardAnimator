using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace BillboardAnimator
{
    class PropConfig
    {

        private static Dictionary<string, PropPositioningInfo> fallbackDict = new Dictionary<string, PropPositioningInfo>();
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

        [NonSerialized]
        public List<Vector3> positions = new List<Vector3>();



        [NonSerialized]
        public List<float> rotations = new List<float>();

        public List<float> xSize = new List<float>();
        public List<float> ySize = new List<float>();

        [NonSerialized]
        public List<Vector3> scale = new List<Vector3>();
    }
}
