using BillboardAnimator.Managers;
using BillboardAnimator.UI;
using BillboardAnimator.Utils;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BillboardAnimator.Tools
{
    class PropSelectorTool : DefaultTool
    {
        public DropdownDialog dialog;

        protected override void Awake()
        {
            LoggerUtils.Log("Tool awake");

            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

        }

        protected override void OnToolUpdate()
        {
            if (m_toolController != null && !m_toolController.IsInsideUI && Cursor.visible)
            {
                RaycastOutput raycastOutput;

                if (RaycastProp(out raycastOutput))
                {
                    ushort propId = raycastOutput.m_propInstance;
                    ushort buildingId = raycastOutput.m_building;

                    if (propId != 0)
                    {
                        PropManager propManager = PropManager.instance;
                        PropInstance prop = propManager.m_props.m_buffer[(int)propId];

                        if ((prop.m_flags & (ushort)(PropInstance.Flags.Created)) != (ushort)PropInstance.Flags.None && PropConfig.Instance().propPositioningDict.ContainsKey(prop.Info.GetLocalizedTitle()) )
                        {
                            if (Event.current.type == EventType.MouseDown)
                            {
                                //unset tool
                                ShowToolInfo(false, null, new Vector3());

                                dialog.isProp = true;
                                dialog.entityId = propId;
                                dialog.entityPosition = prop.Position;
                                dialog.entityAngle = -1 * Mathf.Rad2Deg * prop.Angle;
                                dialog.entityName = prop.Info.GetLocalizedTitle();

                                dialog.updateDropdowns();
                                dialog.Show();

                                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                                ToolsModifierControl.SetTool<DefaultTool>();
                            }
                            else
                            {
                              
                              ShowToolInfo(true, "Make this sign dynamic!", prop.Position);
                             
                            }
                        }
                    }

                    else if( buildingId != 0)
                    {
                        BuildingManager buildingManager = BuildingManager.instance;
                        Building building = buildingManager.m_buildings.m_buffer[buildingId];
                        UIPanel panel = GameObject.Find("(Library) CityServiceWorldInfoPanel").GetComponent<UIPanel>();
                        if ((building.m_flags & Building.Flags.Created) != Building.Flags.None && PropConfig.Instance().propPositioningDict.ContainsKey(building.Info.GetLocalizedTitle())) 
                        {
                            Vector3 position;
                            Quaternion rotation;
                            building.CalculateMeshPosition(out position, out rotation);
                            if (Event.current.type == EventType.MouseDown)
                            {
                                //unset tool
                                ShowToolInfo(false, null, new Vector3());
                                panel.Hide();

                                dialog.isProp = false;
                                dialog.entityId = buildingId;
                                dialog.entityPosition = position;
                                dialog.entityAngle = rotation.eulerAngles.y;
                                dialog.entityName = building.Info.GetLocalizedTitle();

                                dialog.updateDropdowns();
                                dialog.Show();

                                //ScreenManager.instance.SetPropScreen(false, buildingId, position, rotation.eulerAngles.y, building.Info.GetLocalizedTitle());
                                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                                ToolsModifierControl.SetTool<DefaultTool>();
                            }
                            else
                            {

                                ShowToolInfo(true, "Make this sign dynamic!", position);

                            }
                        }
                    }
                    else
                    {
                        ShowToolInfo(false, null, new Vector3());
                    }
                }
                else
                {
                    ShowToolInfo(false, null, new Vector3());
                }
            }
            else
            {
                ShowToolInfo(false, null, new Vector3());
            }
        }

        bool RaycastProp(out RaycastOutput raycastOutput)
        {
            RaycastInput raycastInput = new RaycastInput(Camera.main.ScreenPointToRay(Input.mousePosition), Camera.main.farClipPlane);
            raycastInput.m_propService.m_service = ItemClass.Service.None;
            raycastInput.m_buildingService.m_service = ItemClass.Service.Beautification;
            raycastInput.m_netService.m_subService = ItemClass.SubService.None;
            raycastInput.m_netService.m_itemLayers = ItemClass.Layer.None;
            raycastInput.m_ignorePropFlags = PropInstance.Flags.None;
            raycastInput.m_ignoreBuildingFlags = Building.Flags.None;
            raycastInput.m_ignoreTerrain = true;

            return RayCast(raycastInput, out raycastOutput);
        }
    }
}
