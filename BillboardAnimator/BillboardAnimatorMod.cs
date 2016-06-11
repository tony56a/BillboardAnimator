using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillboardAnimator
{
    public class BillboardAnimatorMod : IUserMod
    {
        //private MarkARouteOptions mOptions = null;

        public string Name
        {
            get
            {
                return "Billboard Animator";
            }
        }

        public string Description
        {
            get
            {
                return "A mod that lets you add dynamic images to your props!";
            }
        }

        /*public void OnSettingsUI(UIHelperBase helper)
        {
            if (mOptions == null)
            {
                mOptions = new GameObject("RoadNamerOptions").AddComponent<MarkARouteOptions>();
            }
            mOptions.generateSettings(helper);
        }*/
    }
}

