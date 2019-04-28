using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework {
    public class StationModule : IModule {
        #region 字段
        private StationEngine m_stationEngine;
        #endregion

        #region IModule抽象函数实现
        public override void Init () {
            m_stationEngine = StationEngine.Instance;
            m_stationEngine.Init ();
        }
        public override void Release () {

        }
        #endregion

        #region U3DPlayerActor相关
        public void AddPlayerActor (UInt16 stationIndex, PlayerActor playerActor) {
            m_stationEngine.AddPlayerActor (stationIndex, playerActor);
        }
        public void RemovePlayerActor (UInt16 stationIndex, PlayerActor playerActor) {
            m_stationEngine.RemovePlayerActor (stationIndex, playerActor);
        }
        public void GetU3DPlayerActors (UInt16 stationIndex, ref List<PlayerActor> list) {
            m_stationEngine.GetU3DPlayerActor (stationIndex, ref list);
        }
        #endregion

        #region 位置点相关

        #endregion

        #region Npc相关
        public void AddNpcAction (UInt16 stationIndex, NpcAction npcAction) {
            m_stationEngine.AddNpcAction (stationIndex, npcAction);
        }
        public void RemoveNpcAction (UInt16 stationIndex, NpcAction npcAction) {
            if (System.Object.ReferenceEquals (npcAction, null)) return;
            m_stationEngine.RemoveNpcAction (stationIndex, npcAction);
        }
        public int GetNpcCount (UInt16 stationIndex, NpcActionStatus npcActionStatus) {
            return m_stationEngine.GetNpcCount (stationIndex, npcActionStatus);
        }
        public Transform GetNpcParentTransform (UInt16 stationIndex, NpcActionStatus npcActionStatus) {
            return m_stationEngine.GetNpcParentTransform (stationIndex, npcActionStatus);
        }
        //客户端重连，同步Npc信息, 数组顺序[进站上行上车Socket, 进站下行上车Socket, 出站上行下车Socket, 出站下行下车Socket]
        public void SyncNpcInfo (PlayerActor[] stationPlayerActor) {
            m_stationEngine.SyncNpcInfo (stationPlayerActor);
        }
        public void SyncNpcInfo () {
            m_stationEngine.SyncNpcInfo ();
        }
        #endregion

        #region 设备相关
        public DeviceMgr GetDeviceMgr (UInt16 stationIndex, DeviceType deviceType) {
            return m_stationEngine.GetDeviceMgr (stationIndex, deviceType);
        }
        //站台的上行屏蔽门是否打开
        public bool IsOpenShangXingPingBiMen (UInt16 stationIndex, DeviceType deviceType) {
            return m_stationEngine.IsOpenShangXingPingBiMen (stationIndex, deviceType);
        }
        //站台的上行屏蔽门状态设置为关闭
        public void CloseShangXingPingBiMen (UInt16 stationIndex, DeviceType deviceType) {
            m_stationEngine.CloseShangXingPingBiMen (stationIndex, deviceType);
        }
        //站台的下行屏蔽门是否打开
        public bool IsOpenXiaXingPingBiMen (UInt16 stationIndex, DeviceType deviceType) {
            return m_stationEngine.IsOpenXiaXingPingBiMen (stationIndex, deviceType);
        }
        //站台的下行屏蔽门状态设置为关闭
        public void CloseXiaXingPingBiMen (UInt16 stationIndex, DeviceType deviceType) {
            m_stationEngine.CloseXiaXingPingBiMen (stationIndex, deviceType);
        }
        //同步所有设备类型的设备信息
        public virtual void SyncDeviceInfo (PlayerActor playerActor) {
            m_stationEngine.SyncDeviceInfo (playerActor);
        }
        //同步指定设备类型的设备信息
        public void SyncDeviceInfoByDeviceType (DeviceType deviceType, PlayerActor playerActor) {
            m_stationEngine.SyncDeviceInfoByDeviceType (deviceType, playerActor);
        }
        #endregion

        #region 摄像头Camera相关
        public void AddCamera2ShowCameraIndexList (UInt16 stationIndex, UInt16 cameraIndex) {
            m_stationEngine.AddCamera2ShowCameraIndexList (stationIndex, cameraIndex);
        }
        public void RemoveCamera4ShowCameraIndexList (UInt16 stationIndex, UInt16 cameraIndex) {
            m_stationEngine.RemoveCamera4ShowCameraIndexList (stationIndex, cameraIndex);
        }
        public Camera GetCamera (UInt16 stationIndex, UInt16 cameraIndex) {
            return m_stationEngine.GetCamera (stationIndex, cameraIndex);
        }
        #endregion
    }
}