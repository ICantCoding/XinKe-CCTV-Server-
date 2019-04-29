/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-07 10:23:09

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum StationType : System.UInt16 {
    ChiDong, //赵营
    QianDaYan, //前大彦
    YuanBoYuan, //园博园
    DaXueCheng, //大学城
    ZiWeiLu, //紫薇路
    ZhaoYing, //赵营
    YuFuHe, //玉符河
    WangFuZhuang, //王府庄
    DaYangZhuang, //大杨庄
    JiNanXi, //济南西
    YanMaZhuangXi, //演马庄西
}

public class Station {
    #region Station管理U3DPlayerActor
    /*
        缓存，有哪些U3DPlayerActor是在通过CCTV观察当前Station
    */
    #region 字段
    private static readonly object U3dPlayerActorLocker = new object ();
    private Dictionary<UInt16, PlayerActor> m_playerActorDict = new Dictionary<UInt16, PlayerActor> ();
    #endregion

    #region 属性
    public Dictionary<UInt16, PlayerActor> PlayerActorDict {
        get { return m_playerActorDict; }
    }
    #endregion

    #region 方法
    public void AddPlayerActor (PlayerActor playerActor) {
        if (playerActor == null) return;
        if (PlayerActorDict.ContainsKey (playerActor.U3DId) == false) {
            lock (U3dPlayerActorLocker) {
                PlayerActorDict.Add (playerActor.U3DId, playerActor);
            }
        }
    }
    public void RemovePlayerActor (PlayerActor playerActor) {
        if (playerActor == null) return;
        if (PlayerActorDict.ContainsKey (playerActor.U3DId)) {
            lock (U3dPlayerActorLocker) {
                PlayerActorDict.Remove (playerActor.U3DId);
            }
        }
    }
    public void GetU3DPlayerActor (ref List<PlayerActor> list) {
        if (list == null) {
            list = new List<PlayerActor> ();
        }
        lock (U3dPlayerActorLocker) {
            foreach (var item in PlayerActorDict) {
                list.Add (item.Value);
            }
        }
    }
    #endregion
    #endregion

    #region Station管理位置点信息
    #region 字段
    //Key为位置点类别, int对应PointStatus枚举值大小
    public Dictionary<int, PointQueueHash> m_pointQueueHashDict = new Dictionary<int, PointQueueHash> ();
    #endregion

    #region 属性
    public Dictionary<int, PointQueueHash> PointQueueHashDict {
        get { return m_pointQueueHashDict; }
    }
    #endregion

    #region 索引器
    public PointQueueHash this [int index] {
        get { return m_pointQueueHashDict[index]; }
    }
    #endregion

    #region 方法

    #region 获取PointQueueHash
    public PointQueueHash GetPointQueueHash (int pointStatus) {
        PointQueueHash pointQueueHash = null;
        m_pointQueueHashDict.TryGetValue (pointStatus, out pointQueueHash);
        return pointQueueHash;
    }
    #endregion

    #region 获取Point
    //指定位置点类型，指定特定的queueIndex, 来获取未预约的位置点
    public Point GetNoReservationPointByQueueIndex (int pointStatus, int queueIndex) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetNoReservationPointByQueueIndex (queueIndex);
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPointByQueueIndex (int pointStatus, int queueIndex) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetReverseNoReservationPointByQueueIndex (queueIndex);
    }
    //指定位置点类型，来获取随机未预约位置点, 针对休息区
    public Point GetRandomNoReservationPointAtRestArea (int pointStatus) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetRandomNoReservationPointAtRestArea ();
    }
    //指定位置点类型，来获取随机PointQueue中的顺序未预约位置点
    public Point GetNoReservationPoint2RandomPointQueue (int pointStatus) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetNoReservationPoint2RandomPointQueue ();
    }
    //指定位置点类型，来获取随机PointQueue(指定队列的索引范围)中的顺序未预约位置点
    public Point GetNoReservationPoint2RandomPointQueue (int pointStatus, int minQueueIndex, int maxQueueIndex) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetNoReservationPoint2RandomPointQueue (minQueueIndex, maxQueueIndex);
    }
    //指定位置点类型，指定特定的queueIndex, 来获取空位置点
    public Point GetEmptyPointByQueueIndex (int pointStatus, int queueIndex) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetEmptyPointByQueueIndex (queueIndex);
    }
    //指定位置点类型，来获取空位置点
    public Point GetEmptyPoint (int pointStatus) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetEmptyPoint ();
    }
    //获取队列第一个位置点
    public Point GetFirstPoint (int pointStatus, int queueIndex) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetFirstPoint (queueIndex);
    }
    //获取队列最后一个位置点
    public Point GetLastPoint (int pointStatus, int queueIndex) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetLastPoint (queueIndex);
    }
    #endregion

    #region 将PointQueueHash添加到站台中管理
    public void AddPointQueueHash2Station (int pointStatus, PointQueueHash pointQueueHash) {
        if (pointQueueHash == null) return;
        if (m_pointQueueHashDict.ContainsKey (pointStatus) == false) {
            m_pointQueueHashDict.Add (pointStatus, pointQueueHash);
        }
    }
    #endregion

    #region 新增
    //随机获取一个PointQueue队列，并取出该PointQueue中的第一个位置点
    public Point GetFirstPoint2RandomPointQueue (int pointStatus) {
        PointQueueHash pointQueueHash = GetPointQueueHash (pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetFirstPoint2RandomPointQueue ();
    }
    #endregion
    #endregion
    #endregion

    #region Station管理设备信息
    #region 字段
    //用于管理不同类型设备
    private Dictionary<DeviceType, DeviceMgr> m_deviceMgrDict = new Dictionary<DeviceType, DeviceMgr> ();
    #endregion

    #region 属性
    public Dictionary<DeviceType, DeviceMgr> DeviceMgrDict {
        get {
            return m_deviceMgrDict;
        }
    }
    #endregion

    #region 方法 
    public void AddDeviceMgr (DeviceMgr mgr) {
        if (mgr == null) return;
        if (m_deviceMgrDict.ContainsKey (mgr.DeviceType) == false) {
            m_deviceMgrDict.Add (mgr.DeviceType, mgr);
        }
    }
    public DeviceMgr GetDeviceMgr (DeviceType deviceType) {
        DeviceMgr mgr = null;
        m_deviceMgrDict.TryGetValue (deviceType, out mgr);
        return mgr;
    }
    public Device GetDevice (DeviceType deviceType, int deviceId) {
        DeviceMgr mgr = GetDeviceMgr (deviceType);
        if (mgr == null) return null;
        return mgr.GetDevice (deviceId);
    }
    //站台的上行屏蔽门是否打开
    public bool IsOpenShangXingPingBiMen (DeviceType deviceType) {
        DeviceMgr mgr = GetDeviceMgr (deviceType);
        if (mgr == null) return false;
        PingBiMenMgr pingBiMenMgr = (PingBiMenMgr) mgr;
        return pingBiMenMgr.ShangXingPingBiMenIsOpen;
    }
    //站台的上行屏蔽门状态设置为关闭
    public void CloseShangXingPingBiMen (DeviceType deviceType) {
        DeviceMgr mgr = GetDeviceMgr (deviceType);
        if (mgr == null) return;
        PingBiMenMgr pingBiMenMgr = (PingBiMenMgr) mgr;
        pingBiMenMgr.ShangXingPingBiMenIsOpen = false;
    }
    //站台的下行屏蔽门是否打开
    public bool IsOpenXiaXingPingBiMen (DeviceType deviceType) {
        DeviceMgr mgr = GetDeviceMgr (deviceType);
        if (mgr == null) return false;
        PingBiMenMgr pingBiMenMgr = (PingBiMenMgr) mgr;
        return pingBiMenMgr.XiaXingPingBiMenIsOpen;
    }
    //站台的下行屏蔽门状态设置为关闭
    public void CloseXiaXingPingBiMen (DeviceType deviceType) {
        DeviceMgr mgr = GetDeviceMgr (deviceType);
        if (mgr == null) return;
        PingBiMenMgr pingBiMenMgr = (PingBiMenMgr) mgr;
        pingBiMenMgr.XiaXingPingBiMenIsOpen = false;
    }
    //同步所有设备信息
    public void SyncDeviceInfo (PlayerActor playerActor) {
        var enumerator = m_deviceMgrDict.GetEnumerator ();
        while (enumerator.MoveNext ()) {
            DeviceMgr mgr = enumerator.Current.Value;
            if (mgr != null)
            mgr.SyncDeviceInfo (playerActor);
        }
        enumerator.Dispose ();
    }
    //同步指定类型的设备信息
    public void SyncDeviceInfoByDeviceType (DeviceType deviceType, PlayerActor playerActor) {
        DeviceMgr mgr = m_deviceMgrDict[deviceType];
        if (mgr != null)
        mgr.SyncDeviceInfo (playerActor);
    }
    #endregion

    #endregion

    #region Station管理Npc信息
    #region 字段
    private Dictionary<NpcActionStatus, NpcMgr> m_npcMgrDict = new Dictionary<NpcActionStatus, NpcMgr> ();
    #endregion

    #region 方法
    public void AddNpcMgr (NpcMgr npcMgr) {
        if (npcMgr == null) return;
        if (m_npcMgrDict.ContainsKey (npcMgr.NpcActionStatus) == false) {
            m_npcMgrDict.Add (npcMgr.NpcActionStatus, npcMgr);
        }
    }
    public NpcMgr GetNpcMgr (NpcActionStatus npcActionStatus) {
        NpcMgr npcMgr = null;
        m_npcMgrDict.TryGetValue (npcActionStatus, out npcMgr);
        return npcMgr;
    }
    public void AddNpcAction (NpcAction npcAction) {
        if (System.Object.ReferenceEquals (npcAction, null)) return;
        NpcMgr npcMgr = GetNpcMgr (npcAction.NpcActionStatus);
        if (System.Object.ReferenceEquals (npcMgr, null)) return;
        npcMgr.AddNpcAction (npcAction);
    }
    public void RemoveNpcAction (NpcActionStatus npcActionStatus, int npcId) {
        NpcMgr npcMgr = GetNpcMgr (npcActionStatus);
        if (npcMgr == null) return;
        npcMgr.RemoveNpcAction (npcId);
    }
    public int GetNpcCount (NpcActionStatus npcActionStatus) {
        NpcMgr npcMgr = GetNpcMgr (npcActionStatus);
        if (npcMgr == null) return 0;
        return npcMgr.Count;
    }
    public Transform GetNpcParentTransform (NpcActionStatus npcActionStatus) {
        NpcMgr npcMgr = GetNpcMgr (npcActionStatus);
        if (npcMgr == null) return null;
        return npcMgr.NpcParentTransform;
    }
    //同步Npc信息
    public void SyncNpcInfo (PlayerActor[] stationPlayerActor) {
        NpcMgr mgr = m_npcMgrDict[NpcActionStatus.EnterStationTrainUp_NpcActionStatus];
        if (mgr != null)
            mgr.SyncNpcInfo (stationPlayerActor[0]);
        mgr = m_npcMgrDict[NpcActionStatus.EnterStationTrainDown_NpcActionStatus];
        if (mgr != null)
            mgr.SyncNpcInfo (stationPlayerActor[1]);
        mgr = m_npcMgrDict[NpcActionStatus.ExitStationTrainUp_NpcActionStatus];
        if (mgr != null)
            mgr.SyncNpcInfo (stationPlayerActor[2]);
        mgr = m_npcMgrDict[NpcActionStatus.ExitStationTrainDown_NpcActionStatus];
        if (mgr != null)
            mgr.SyncNpcInfo (stationPlayerActor[3]);
    }
    public void SyncNpcInfo () {
        var enumerator = m_npcMgrDict.GetEnumerator ();
        while (enumerator.MoveNext ()) {
            NpcMgr mgr = enumerator.Current.Value;
            if (mgr != null)
            mgr.SyncNpcInfo ();
        }
        enumerator.Dispose ();
    }
    #endregion

    #endregion

    #region Station管理摄像头Camera信息
    #region 字段
    //保存所有站台的所有摄像头
    private Dictionary<UInt16, Camera> m_allCameraDict =
        new Dictionary<UInt16, Camera> ();
    //保存站台启用的摄像头，所谓启用就是有客户端正在使用该摄像头
    private List<UInt16> m_showCameraIndexList = new List<UInt16> ();
    #endregion

    #region 属性
    public Dictionary<UInt16, Camera> AllCameraDict {
        get { return m_allCameraDict; }
    }
    public List<UInt16> ShowCameraIndexList {
        get { return m_showCameraIndexList; }
    }
    #endregion
    #region 方法
    public void AddCamera2ShowCameraIndexList (UInt16 cameraIndex) {
        if(m_showCameraIndexList.Contains(cameraIndex)) return;
        m_showCameraIndexList.Add(cameraIndex);
    }
    public void RemoveCamera4ShowCameraIndexList (UInt16 cameraIndex) {
        m_showCameraIndexList.Remove(cameraIndex);
    }
    public Camera GetCamera (UInt16 cameraIndex) {
        Camera tempCamera = null;
        m_allCameraDict.TryGetValue (cameraIndex, out tempCamera);
        return tempCamera;
    }
    #endregion
    #endregion
}