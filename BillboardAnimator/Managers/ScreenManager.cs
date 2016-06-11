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

        public void SetPropScreen(bool isBuilding,ushort id, Vector3 position, float angle, string propName)
        {

            ScreenObj obj = new ScreenObj(position, angle, propName);
            if( isBuilding)
            {
                if(!buildingDict.ContainsKey(id))
                {
                    buildingDict[id] = obj;

                }
            }
            else
            {
                if (!propDict.ContainsKey(id))
                {
                    propDict[id] = obj;
                }
            }
            RenderingManager.instance.update();
        }

        internal void delScreenObj(ushort key, bool isBuilding)
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
    }

    [Serializable]
    class ScreenObj
    {
        public string propName;
        public float angle;
        public float x = 0;
        public float y = 0;
        public float z = 0;
        
        [NonSerialized]
        public Vector3 position;
        
        [XmlElement(IsNullable = true)]
        public string extras;

        [NonSerialized]
        public GameObject signObject;

        public ScreenObj( Vector3 position, float angle, string propName, string extras = null)
        {
            this.x = position.x;
            this.y = position.y;
            this.z = position.z;
            this.position = position;

            this.propName = propName;
            this.angle = angle;
            this.extras = extras;

        }
    }
}
