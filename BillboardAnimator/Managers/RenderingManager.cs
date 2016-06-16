using BillboardAnimator.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UnityEngine;

namespace BillboardAnimator.Managers
{
    class RenderingManager : SimulationManagerBase<RenderingManager, DistrictProperties>, IRenderableManager, ISimulationManager
    {
        private bool refreshFlag = false;
        private volatile bool screenRefreshFlag = false;
        public float renderHeight = 1000f;
        private bool objHidden = false;

        private float intervalVal;
        public float interval
        {
            get
            {
                return intervalVal;
            }
            set
            {
                if (messageUpdateTimer!= null)
                {
                    messageUpdateTimer.Stop();
                    intervalVal = value;
                    messageUpdateTimer.Interval = intervalVal;
                    messageUpdateTimer.Start();
                }
            }
        }

        Timer messageUpdateTimer = new Timer();
        System.Random messageRandom = new System.Random();
        public bool registered;

        public void initTimer()
        {

            interval = 5000;
            LoggerUtils.Log("Timer started!");
        }

        public void disableTimer()
        {
            messageUpdateTimer.Enabled = false;
            messageUpdateTimer.Stop();
            messageUpdateTimer.Dispose();

        }

        private void MessageUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!screenRefreshFlag)
            {
                screenRefreshFlag = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            messageUpdateTimer.Elapsed += MessageUpdateTimer_Elapsed;
            messageUpdateTimer.AutoReset = true;
        }

        protected override void BeginOverlayImpl(RenderManager.CameraInfo cameraInfo)
        {
            if (refreshFlag)
            {
                refreshFlag = false;
                try
                {
                    renderGameObjects();
                }
                catch (Exception ex)
                {
                    LoggerUtils.LogException(ex);
                }
            }

            if (!objHidden && cameraInfo.m_height > renderHeight)
            {

                foreach ( ScreenObj screen in ScreenManager.instance.propDict.Values)
                {
                    if( screen.signObject != null)
                    {
                        screen.signObject.SetActive(false);
                    }
                }
                foreach (ScreenObj screen in ScreenManager.instance.buildingDict.Values)
                {
                    if (screen.signObject != null)
                    {
                        screen.signObject.SetActive(false);
                    }
                }
                objHidden = true;
            }
            else if (objHidden && cameraInfo.m_height <= renderHeight) //This is a mess, and I'll sort it soon :)
            {

                foreach (ScreenObj screen in ScreenManager.instance.propDict.Values)
                {
                    if (screen.signObject != null)
                    {
                        screen.signObject.SetActive(true);
                    }
                }
                foreach (ScreenObj screen in ScreenManager.instance.buildingDict.Values)
                {
                    if (screen.signObject != null)
                    {
                        screen.signObject.SetActive(true);
                    }
                }
                objHidden = false;
            }

            if (screenRefreshFlag)
            {
                screenRefreshFlag = false;
                List<Material> materials = TextureUtils.m_screenTextureStore.Values.ToList();

                foreach ( ScreenObj obj in ScreenManager.instance.buildingDict.Values)
                {
                    if (obj.signObject)
                    {
                        PropPositioningInfo positioningInfo = PropConfig.Instance().propPositioningDict[obj.propName];

                        for (byte i = 0; i < positioningInfo.numSigns; i++)
                        {
                            Transform child = obj.signObject.transform.Find(i.ToString());
                            if( child != null && !obj.isStatic[i] )
                            {
                                Material mat = materials[messageRandom.Next(materials.Count)];
                                child.gameObject.GetComponent<MeshRenderer>().material = mat;
                            }
                        }
                    }
                }
                foreach (ScreenObj obj in ScreenManager.instance.propDict.Values)
                {
                    if (obj.signObject)
                    {
                        PropPositioningInfo positioningInfo = PropConfig.Instance().propPositioningDict[obj.propName];

                        for (byte i = 0; i < positioningInfo.numSigns; i++)
                        {
                            Transform child = obj.signObject.transform.Find(i.ToString());
                            if (child != null && !obj.isStatic[i])
                            {
                                Material mat = materials[messageRandom.Next(materials.Count)];
                                child.gameObject.GetComponent<MeshRenderer>().material = mat;
                            }
                        }
                    }

                }
            }
        }

        private void renderGameObjects()
        {
            List<Material> materials = TextureUtils.m_screenTextureStore.Values.ToList();

            foreach ( KeyValuePair<ushort,ScreenObj> obj in ScreenManager.instance.propDict)
            {
                ScreenObj screenObj = obj.Value;
                if (screenObj.signObject != null)
                {
                    continue;
                }
                else
                {
                    screenObj.signObject = new GameObject(obj.Key + "animatedsign");

                    screenObj.signObject.transform.position = screenObj.position;
                    screenObj.signObject.transform.Rotate(0, (screenObj.angle) + 60f, 0);

                    PropPositioningInfo positioningInfo = PropConfig.Instance().propPositioningDict[screenObj.propName];
                    for (byte i = 0; i < positioningInfo.numSigns; i++)
                    {
                        if (screenObj.isActive[i])
                        {
                            if( screenObj.isStatic[i] && !TextureUtils.m_screenTextureStore.ContainsKey(screenObj.staticImageString[i]))
                            {
                                continue;
                            }

                            GameObject screenPaneObj = new GameObject(i.ToString());
                            screenPaneObj.AddComponent<MeshRenderer>();
                            screenPaneObj.AddComponent<MeshFilter>();
                            screenPaneObj.transform.parent = screenObj.signObject.transform;
                            screenPaneObj.transform.localPosition = new Vector3(positioningInfo.xPos[i], positioningInfo.yPos[i], positioningInfo.zPos[i]);
                            screenPaneObj.transform.localScale = new Vector3(positioningInfo.xSize[i], positioningInfo.ySize[i], 1f);
                            screenPaneObj.transform.Rotate(0, (screenObj.angle) + positioningInfo.rotations[i], 0);
                            
                            Material mat = screenObj.isStatic[i] ? TextureUtils.m_screenTextureStore[screenObj.staticImageString[i]] : materials[messageRandom.Next(materials.Count)];
                            screenPaneObj.GetComponent<Renderer>().material = mat;
                            screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);
                        }
                        
                    }
                    
                    ScreenManager.instance.propDict[obj.Key].signObject = screenObj.signObject;
                }

            }
            foreach (KeyValuePair<ushort, ScreenObj> obj in ScreenManager.instance.buildingDict)
            {
                ScreenObj screenObj = obj.Value;
                if (screenObj.signObject != null)
                {
                    continue;
                }
                else
                {
                    screenObj.signObject = new GameObject(obj.Key + "animatedsign");

                    screenObj.signObject.transform.position = screenObj.position;
                    screenObj.signObject.transform.Rotate(0, (screenObj.angle) + 60f, 0);

                    PropPositioningInfo positioningInfo = PropConfig.Instance().propPositioningDict[screenObj.propName];
                    for (byte i = 0; i < positioningInfo.numSigns; i++)
                    {
                        if(screenObj.isActive[i])
                        {
                            if (screenObj.isStatic[i] && !TextureUtils.m_screenTextureStore.ContainsKey(screenObj.staticImageString[i]))
                            {
                                continue;
                            }

                            GameObject screenPaneObj = new GameObject(i.ToString());
                            screenPaneObj.AddComponent<MeshRenderer>();
                            screenPaneObj.AddComponent<MeshFilter>();
                            screenPaneObj.transform.parent = screenObj.signObject.transform;
                            screenPaneObj.transform.localPosition = new Vector3(positioningInfo.xPos[i], positioningInfo.yPos[i], positioningInfo.zPos[i]);
                            screenPaneObj.transform.localScale = new Vector3(positioningInfo.xSize[i], positioningInfo.ySize[i], 1f);
                            screenPaneObj.transform.Rotate(0, (screenObj.angle) + positioningInfo.rotations[i], 0);

                            Material mat = screenObj.isStatic[i] ? TextureUtils.m_screenTextureStore[screenObj.staticImageString[i]] : materials[messageRandom.Next(materials.Count)];
                            screenPaneObj.GetComponent<Renderer>().material = mat;
                            screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);
                        }
                       
                    }

                    ScreenManager.instance.buildingDict[obj.Key].signObject = screenObj.signObject;
                }
                
            }
        }

        public void update()
        {
            refreshFlag = true;
        }

    }

}
