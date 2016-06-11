using BillboardAnimator.Managers;
using BillboardAnimator.UI;
using BillboardAnimator.Utils;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BillboardAnimator
{
    public class BillboardAnimatorLoading : LoadingExtensionBase
    {

        /*private MarkARouteSerializer m_saveUtility = new MarkARouteSerializer();
        private RenderingManager m_renderingManager = null;*/
        private RenderingManager renderingManager;
        public MainPanel UI { get; set; }

        public static readonly string IMAGES_DIR = "images";
        public static readonly string VIDEOS_DIR = "videos";


        public override void OnCreated(ILoading loading)
        {
            try //So we don't fuck up loading the city
            {
                LoadSprites();
            }
            catch (Exception ex)
            {
                LoggerUtils.LogException(ex);
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame || mode == LoadMode.NewMap || mode == LoadMode.LoadMap)
            {
                UIView view = UIView.GetAView();
                UI = ToolsModifierControl.toolController.gameObject.AddComponent<MainPanel>();

                renderingManager = RenderingManager.instance;
                renderingManager.enabled = true;
                if (renderingManager != null && !renderingManager.registered)
                {
                    RenderManager.RegisterRenderableManager(renderingManager);
                    renderingManager.registered = true;
                }
                /*ModSettings.LoadSettings();

                

              

                MarkARouteOptions.mInGame = true;
                MarkARouteOptions.update();*/

            }
        }

        public override void OnLevelUnloading()
        {
            // First disable dynamic sign updates
            /*RenderingManager.instance.disableTimer();
            ModSettings.SaveSettings();

            DynamicSignConfig.SaveVmsMsgList();
            MarkARouteOptions.mInGame = false;*/
            RenderingManager.instance.disableTimer();
        }

        /// <summary>
        /// Loads all custom sprites
        /// </summary>
        private void LoadSprites()
        {

            bool spriteSuccess = true;

            String[] files = Directory.GetFiles(FileUtils.GetModPath() + Path.DirectorySeparatorChar + IMAGES_DIR);
            foreach (string file in files)
            {
                string[] splitValues = file[0] == Path.DirectorySeparatorChar ? file.Substring(1).Split(Path.DirectorySeparatorChar) : file.Split(Path.DirectorySeparatorChar);
                string fileName = splitValues[splitValues.Length - 1];
                string fileKey = fileName.Split('.')[0];
                spriteSuccess = TextureUtils.AddTexture(file, fileKey) && spriteSuccess;
               
            }
            /*
            if (!spriteSuccess)
            {
                LoggerUtils.LogError("Failed to load some sprites!");
            }
            else
            {
                //RouteShieldConfig.SaveRouteShieldInfo();
            }*/

        }
    }
}
