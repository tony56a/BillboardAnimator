using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace BillboardAnimator.Managers
{
    class ScreenManager : Singleton<ScreenManager>
    {
        /// <summary>
        /// Dictionary of prop Ids, to the container for the dynamic screen object
        /// </summary> 
        public Dictionary<ushort, ScreenObj> propDict = new Dictionary<ushort, ScreenObj>();

        public Dictionary<ushort, ScreenObj> buildingDict = new Dictionary<ushort, ScreenObj>();

        public void SetPropScreen(bool isProp,ushort id, Vector3 position, float angle, string propName, List<bool> isEnabled, List<bool> isStatic, List<string> staticStringName=null)
        {

            ScreenObj obj = new ScreenObj(id, position, angle, propName, isEnabled, isStatic, staticStringName);
            if(isProp)
            {
                if (propDict.ContainsKey(id))
                {
                    Destroy(propDict[id].signObject);
                }
                propDict[id] = obj;

            }
            else
            {
                if (buildingDict.ContainsKey(id))
                {
                    Destroy(buildingDict[id].signObject);
                }
                buildingDict[id] = obj;
            }
            RenderingManager.instance.update();
        }

        public void delScreenObj(ushort key, bool isBuilding)
        {
            ScreenObj obj = isBuilding ? buildingDict[key] : propDict[key];
            Destroy(obj.signObject);
            if( isBuilding)
            {
                buildingDict.Remove(key);
            }
            else
            {
                propDict.Remove(key);
            }
        }

        public ScreenObj[] saveScreen (bool isProp)
        {
            return isProp ? new List<ScreenObj>(propDict.Values).ToArray() : new List<ScreenObj>(buildingDict.Values).ToArray();
        }

        public void loadScreen( bool isProp, ScreenObj[] screenObjs )
        {
            if (screenObjs != null)
            {
                foreach (ScreenObj screen in screenObjs)
                {
                    SetPropScreen(isProp, screen.entityId, new Vector3(screen.x, screen.y, screen.z), screen.angle, screen.propName, screen.isActive, screen.isStatic, screen.staticImageString);
                }
            }
        }
    }

    [Serializable]
    class ScreenObj
    {
        public string propName;
        public float angle;
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public List<bool> isActive;
        public List<bool> isStatic;
        public ushort entityId = 0;
        
        [XmlElement(IsNullable = true)]
        public List<string> staticImageString;

        [NonSerialized]
        public Vector3 position;
        
        [XmlElement(IsNullable = true)]
        public string extras;

        [NonSerialized]
        public GameObject signObject;

        public ScreenObj( ushort entityId, Vector3 position, float angle, string propName, List<bool> isActive, List<bool> isStatic, List<string> staticImageString=null, string extras = null)
        {
            this.entityId = entityId;
            this.x = position.x;
            this.y = position.y;
            this.z = position.z;
            this.position = position;

            this.propName = propName;
            this.angle = angle;

            this.isActive = isActive;
            this.isStatic = isStatic;
            this.staticImageString = staticImageString;

            this.extras = extras;

        }
    }
}
