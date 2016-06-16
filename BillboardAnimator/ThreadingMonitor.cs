using BillboardAnimator.Managers;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillboardAnimator
{
    public class ThreadingMonitor : ThreadingExtensionBase
    {
        BuildingManager buildingManager;
        PropManager propManager;

        public override void OnCreated(IThreading threading)
        {
            buildingManager = BuildingManager.instance;
            propManager = PropManager.instance;

            base.OnCreated(threading);
        }

        public override void OnAfterSimulationTick()
        {
            List<ushort> buildingKeys = new List<ushort>(ScreenManager.instance.buildingDict.Keys);
            List<ushort> propKeys = new List<ushort>(ScreenManager.instance.propDict.Keys);

            foreach (ushort segment in buildingKeys)
            {
                if ((BuildingManager.instance.m_buildings.m_buffer[segment].m_flags) == Building.Flags.None)
                {
                    ScreenManager.instance.delScreenObj(segment, true);
                }
            }

            foreach (ushort segment in propKeys)
            {
                if (PropManager.instance.m_props.m_buffer[segment].m_flags == (ushort)PropInstance.Flags.None)
                {
                    ScreenManager.instance.delScreenObj(segment, false);
                }
            }

            base.OnAfterSimulationTick();
        }
    }
}
