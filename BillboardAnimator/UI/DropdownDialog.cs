using BillboardAnimator.Managers;
using BillboardAnimator.Utils;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BillboardAnimator.UI
{
    class DropdownGroup
    {
        public UIDropDown dropDown;
        public UILabel label;
    }

    public class DropdownDialog : UIPanel
    {

        public static readonly int NUM_DROPDOWNS = 3;

        protected RectOffset m_UIPadding = new RectOffset(5, 5, 5, 5);

        private int titleOffset = 40;
        private TitleBar m_panelTitle;
        private Vector2 offset = Vector2.zero;

        private float yCursor;
        private DropdownGroup[] dropDownGroups = new DropdownGroup[3];
        private UIButton setButton;

        public bool isProp = true;
        public ushort entityId = 0;
        public Vector3 entityPosition = Vector3.zero;
        public float entityAngle = 0;
        public string entityName;
        
        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 180;
            this.height = 300;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();

            m_panelTitle = this.AddUIComponent<TitleBar>();
            m_panelTitle.title = "Panel Screens";
            m_panelTitle.m_closeActions.Add("unsetTools");

            CreatePanelComponents();
            this.relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2) + width, Mathf.Floor((GetUIView().fixedHeight - height) * 0.33f));
            this.backgroundSprite = "MenuPanel2";
        }

        public void updateDropdowns()
        {
            PropPositioningInfo config = PropConfig.Instance().propPositioningDict[entityName];
            for (int i = 0; i < NUM_DROPDOWNS; i++)
            {
                if(i < config.numSigns)
                {
                    dropDownGroups[i].label.Show();
                    dropDownGroups[i].dropDown.Show();
                }
                else
                {
                    dropDownGroups[i].label.Hide();
                    dropDownGroups[i].dropDown.Hide();
                }
               
            }
           
        }

        private void CreatePanelComponents()
        {
            List<string> values = new List<string> { "None", "Random" };
            values.AddRange(TextureUtils.m_screenTextureStore.Keys);

            yCursor = titleOffset + m_UIPadding.top;
            for (int i=0; i< NUM_DROPDOWNS; i++)
            {
                UILabel descLabel = this.AddUIComponent<UILabel>();
                descLabel.text = "Screen " + i;
                descLabel.autoSize = true;
                descLabel.size = new Vector2(this.width - m_UIPadding.left - m_UIPadding.right, 50);
                descLabel.padding = m_UIPadding;
                descLabel.relativePosition = new Vector2(m_UIPadding.left, yCursor);
                descLabel.textAlignment = UIHorizontalAlignment.Left;
                descLabel.verticalAlignment = UIVerticalAlignment.Middle;

                yCursor += (descLabel.height + 2 * m_UIPadding.top);

                UIDropDown dropDown = UIUtils.CreateDropDown(this, new Vector2(((this.width - m_UIPadding.left - m_UIPadding.right)), 25));
                foreach (string name in values)
                {
                    dropDown.AddItem(name);
                }
                dropDown.relativePosition = new Vector2(m_UIPadding.left, yCursor);
                dropDown.selectedIndex = 0;

                DropdownGroup group = new DropdownGroup();
                group.dropDown = dropDown;
                group.label = descLabel;
                group.dropDown.Hide();
                group.label.Hide();
                dropDownGroups[i] = group;

                yCursor += (dropDown.height + 2 * m_UIPadding.top);

            }

            setButton = UIUtils.CreateButton(this);
            setButton.text = "Set";
            setButton.size = new Vector2(this.width - m_UIPadding.left - m_UIPadding.right, 50);
            setButton.relativePosition = new Vector2(m_UIPadding.left, yCursor);
            setButton.eventClicked += SetButton_eventClicked;
            this.height = yCursor + setButton.height + m_UIPadding.bottom;

        }

        private void SetButton_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            List<bool> isActive = new List<bool>();
            List<bool> isStatic = new List<bool>();
            List<string> staticStrings = new List<string>();
            for (int i = 0; i < NUM_DROPDOWNS; i++) 
            {
                isActive.Add(dropDownGroups[i].dropDown.selectedValue != "None");
                isStatic.Add(dropDownGroups[i].dropDown.selectedValue != "Random");
                staticStrings.Add(dropDownGroups[i].dropDown.selectedValue);
            }
            ScreenManager.instance.SetPropScreen(isProp, entityId, entityPosition, entityAngle, entityName, isActive, isStatic, staticStrings);
            this.Hide();
        }
    }
}
