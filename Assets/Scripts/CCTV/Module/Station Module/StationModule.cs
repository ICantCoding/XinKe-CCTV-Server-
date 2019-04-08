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
            if (System.Object.ReferenceEquals(npcAction, null)) return;
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
        //站台的上行屏蔽门是否打开
        public bool IsOpenShangXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
        {
            return m_stationEngine.IsOpenShangXingPingBiMen(stationIndex, deviceType);
        }
        //站台的上行屏蔽门状态设置为关闭
        public void CloseShangXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
        {
            m_stationEngine.CloseShangXingPingBiMen(stationIndex, deviceType);
        }
        //站台的下行屏蔽门是否打开
        public bool IsOpenXiaXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
        {
            return m_stationEngine.IsOpenXiaXingPingBiMen(stationIndex, deviceType);
        }
        //站台的下行屏蔽门状态设置为关闭
        public void CloseXiaXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
        {
            m_stationEngine.CloseXiaXingPingBiMen(stationIndex, deviceType);
        }
        #endregion
    }
}


