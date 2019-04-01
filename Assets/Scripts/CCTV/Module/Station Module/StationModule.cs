using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework
{
    public class StationModule : IModule
    {
        #region 字段
        private StationEngine m_stationEngine;
        #endregion

        #region IModule抽象函数实现
        public override void Init()
        {
            m_stationEngine = StationEngine.Instance;
            m_stationEngine.Init();
        }
        public override void Release()
        {

        }
        #endregion

        #region 位置点相关

        #endregion

        #region Npc相关
        public void AddNpcAction(UInt16 stationIndex, NpcAction npcAction)
        {
            m_stationEngine.AddNpcAction(stationIndex, npcAction);
        }
        public void RemoveNpcAction(UInt16 stationIndex, NpcAction npcAction)
        {
            if(System.Object.ReferenceEquals(npcAction, null)) return;
            m_stationEngine.RemoveNpcAction(stationIndex, npcAction);
        }
        public int GetNpcCount(UInt16 stationIndex, NpcActionStatus npcActionStatus)
        {
            return m_stationEngine.GetNpcCount(stationIndex, npcActionStatus);
        }
        public Transform GetNpcParentTransform(UInt16 stationIndex, NpcActionStatus npcActionStatus)
        {
            return m_stationEngine.GetNpcParentTransform(stationIndex, npcActionStatus);
        }
        #endregion

        #region 设备相关
        public DeviceMgr GetDeviceMgr(UInt16 stationIndex, DeviceType deviceType)
        {
            return m_stationEngine.GetDeviceMgr(stationIndex, deviceType);
        }
        #endregion
    }
}


