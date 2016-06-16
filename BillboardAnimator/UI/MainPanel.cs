using BillboardAnimator.Tools;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BillboardAnimator.UI
{
    public class MainPanel : UICustomControl
    {
        private PropSelectorTool propSelectorTool;


        UIButton placeSignBtn;

        private bool isUiShowing;

        DropdownDialog dialog;

        public MainPanel()
        {
            UIView uiView = UIView.GetAView();

            dialog = uiView.AddUIComponent(typeof(DropdownDialog)) as DropdownDialog;
            dialog.Hide();

            placeSignBtn = (UIButton)uiView.AddUIComponent(typeof(UIButton));
            
            placeSignBtn.text = "DB";
            placeSignBtn.width = 75;
            placeSignBtn.height = 30;
            placeSignBtn.normalBgSprite = "ButtonMenu";
            placeSignBtn.disabledBgSprite = "ButtonMenuDisabled";
            placeSignBtn.hoveredBgSprite = "ButtonMenuHovered";
            placeSignBtn.focusedBgSprite = "ButtonMenuFocused";
            placeSignBtn.pressedBgSprite = "ButtonMenuPressed";
            placeSignBtn.textColor = new Color32(255, 255, 255, 255);
            placeSignBtn.disabledTextColor = new Color32(7, 7, 7, 255);
            placeSignBtn.hoveredTextColor = new Color32(7, 132, 255, 255);
            placeSignBtn.focusedTextColor = new Color32(255, 255, 255, 255);
            placeSignBtn.pressedTextColor = new Color32(30, 30, 44, 255);
            placeSignBtn.eventClick += propSelectorToolBtn_eventClick;
            placeSignBtn.relativePosition = new Vector3(30, 60);

            propSelectorTool = ToolsModifierControl.toolController.gameObject.AddComponent<PropSelectorTool>();

            ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
            ToolsModifierControl.SetTool<DefaultTool>();

            propSelectorTool.dialog = dialog;
        }

        private void propSelectorToolBtn_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {

            if (ToolsModifierControl.toolController.CurrentTool == propSelectorTool)
            {
                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                ToolsModifierControl.SetTool<DefaultTool>();
            }
            else
            {
                ToolsModifierControl.toolController.CurrentTool = propSelectorTool;
                ToolsModifierControl.SetTool<PropSelectorTool>();
            }

        }
    }
}
