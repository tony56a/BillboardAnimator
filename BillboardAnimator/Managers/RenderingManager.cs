using BillboardAnimator.Utils;
using System;
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
            messageUpdateTimer.Elapsed += MessageUpdateTimer_Elapsed;
            messageUpdateTimer.AutoReset = true;
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
            initTimer();
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
                foreach( ScreenObj obj in ScreenManager.instance.buildingDict.Values)
                {
                    if (obj.signObject)
                    {
                        MeshRenderer[] renderers = obj.signObject.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer renderer in renderers)
                        {
                            Material mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                            renderer.material = mat;
                        }
                    }
                }
                foreach (ScreenObj obj in ScreenManager.instance.propDict.Values)
                {
                    if (obj.signObject)
                    {
                        MeshRenderer[] renderers = obj.signObject.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer renderer in renderers)
                        {
                            Material mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                            renderer.material = mat;
                        }
                    }
                 
                }
            }
        }

        private void renderGameObjects()
        {
            foreach( KeyValuePair<ushort,ScreenObj> obj in ScreenManager.instance.propDict)
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

                    GameObject screenPaneObj = new GameObject();
                    screenPaneObj.AddComponent<MeshRenderer>();
                    screenPaneObj.AddComponent<MeshFilter>();
                    screenPaneObj.transform.parent = screenObj.signObject.transform;
                    screenPaneObj.transform.localPosition = new Vector3(-0.05f, 26f, -4.6f);
                    screenPaneObj.transform.localScale = new Vector3(1.93f, 2.7f, 1f);
                    screenPaneObj.transform.Rotate(0, (screenObj.angle) + 60f, 0);

                    Material mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                    screenPaneObj.GetComponent<Renderer>().material = mat;
                    screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);

                    screenPaneObj = new GameObject();
                    screenPaneObj.AddComponent<MeshRenderer>();
                    screenPaneObj.AddComponent<MeshFilter>();
                    screenPaneObj.transform.parent = screenObj.signObject.transform;
                    screenPaneObj.transform.localPosition = new Vector3(4f, 26f, 2.4f);
                    screenPaneObj.transform.localScale = new Vector3(1.93f, 2.7f, 1f);
                    screenPaneObj.transform.Rotate(0, (screenObj.angle) - 60f, 0);

                    mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                    screenPaneObj.GetComponent<Renderer>().material = mat;
                    screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);


                    screenPaneObj = new GameObject();
                    screenPaneObj.AddComponent<MeshRenderer>();
                    screenPaneObj.AddComponent<MeshFilter>();
                    screenPaneObj.transform.parent = screenObj.signObject.transform;
                    screenPaneObj.transform.localPosition = new Vector3(4f, 26f, 2.4f);
                    screenPaneObj.transform.localScale = new Vector3(1.93f, 2.7f, 1f);
                    screenPaneObj.transform.Rotate(0, (screenObj.angle) + 180f, 0);

                    mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                    screenPaneObj.GetComponent<Renderer>().material = mat;
                    screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);
                    //TODO: Create child objects to hold renderers for each screen for the prop

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

                    GameObject screenPaneObj = new GameObject();
                    screenPaneObj.AddComponent<MeshRenderer>();
                    screenPaneObj.AddComponent<MeshFilter>();
                    screenPaneObj.transform.parent = screenObj.signObject.transform;
                    screenPaneObj.transform.localPosition = new Vector3(-0.05f, 26f, -4.6f);
                    screenPaneObj.transform.localScale = new Vector3(1.93f, 2.7f, 1f);
                    screenPaneObj.transform.Rotate(0, (screenObj.angle) + 60f, 0);

                    Material mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                    screenPaneObj.GetComponent<Renderer>().material = mat;
                    screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);

                    screenPaneObj = new GameObject();
                    screenPaneObj.AddComponent<MeshRenderer>();
                    screenPaneObj.AddComponent<MeshFilter>();
                    screenPaneObj.transform.parent = screenObj.signObject.transform;
                    screenPaneObj.transform.localPosition = new Vector3(4f, 26f, 2.4f);
                    screenPaneObj.transform.localScale = new Vector3(1.93f, 2.7f, 1f);
                    screenPaneObj.transform.Rotate(0, (screenObj.angle) - 60f, 0);

                    mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                    screenPaneObj.GetComponent<Renderer>().material = mat;
                    screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);


                    screenPaneObj = new GameObject();
                    screenPaneObj.AddComponent<MeshRenderer>();
                    screenPaneObj.AddComponent<MeshFilter>();
                    screenPaneObj.transform.parent = screenObj.signObject.transform;
                    screenPaneObj.transform.localPosition = new Vector3(-4.1f, 26f, 2.3f);
                    screenPaneObj.transform.localScale = new Vector3(1.93f, 2.7f, 1f);
                    screenPaneObj.transform.Rotate(0, (screenObj.angle) + 180f, 0);

                    mat = TextureUtils.m_screenTextureStore[messageRandom.Next(TextureUtils.m_screenTextureStore.Count)];
                    screenPaneObj.GetComponent<Renderer>().material = mat;
                    screenPaneObj.GetComponent<MeshFilter>().mesh = MeshUtils.CreateRectMesh(mat.mainTexture.width, mat.mainTexture.height);
                    //TODO: Create child objects to hold renderers for each screen for the prop

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
