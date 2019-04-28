/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-08 11:00:24

** 功能描述:  StationMgr管理Station

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StationMgr
{
    #region 字段
    //Key为站台的索引, 与场景中Hierarchy面板中的顺序相关
    public Dictionary<UInt16, Station> m_stationDict = new Dictionary<UInt16, Station>();
    #endregion

    #region 属性
    public int Count
    {
        get { return m_stationDict.Count; }
    }
    #endregion

    #region 站台管理相关
    public Station GetStation(UInt16 stationIndex)
    {
        Station station = null;
        m_stationDict.TryGetValue(stationIndex, out station);
        return station;
    }
    public void AddStation2Mgr(UInt16 stationIndex, Station station)
    {
        if (station == null) return;
        if (m_stationDict.ContainsKey(stationIndex) == false)
        {
            m_stationDict.Add(stationIndex, station);
        }
    }
    #endregion

    #region U3DPlayerActor相关
    public void AddPlayerActor(UInt16 stationIndex, PlayerActor playerActor)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.AddPlayerActor(playerActor);
    }
    public void RemovePlayerActor(UInt16 stationIndex, PlayerActor playerActor)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.RemovePlayerActor(playerActor);
    }
    public void GetU3DPlayerActor(UInt16 stationIndex, ref List<PlayerActor> list)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.GetU3DPlayerActor(ref list);
    }
    #endregion

    #region 位置点相关
    //指定站台索引, 位置点类型, PointQueue的queueIndex, 来获取未预约的位置点
    public Point GetNoReservationPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetNoReservationPointByQueueIndex(pointStatus, queueIndex);
    }
    //指定站台索引, 位置点类型, PointQueue的queueIndex, 来获取空位置点
    public Point GetEmptyPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetEmptyPointByQueueIndex(pointStatus, queueIndex);
    }
    //指定站台索引, 位置点类型 来获取空位置点
    public Point GetEmptyPoint(UInt16 stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetEmptyPoint(pointStatus);
    }
    //获取队列最后一个位置点
    public Point GetLastPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetLastPoint(pointStatus, queueIndex);
    }
    //随机获取一个PointQueue队列，并取出该PointQueue中的第一个位置点
    public Point GetFirstPoint2RandomPointQueue(UInt16 stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetFirstPoint2RandomPointQueue(pointStatus);
    }
    //随机获取一个PointQueue队列, 并取出首个NoReservation的位置点
    public Point GetNoReservationPoint2RandomPointQueue(UInt16 stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetNoReservationPoint2RandomPointQueue(pointStatus);
    }
    //随机获取一个位置点, 针对休息区
    public Point GetRandomNoReservationPointAtRestArea(UInt16 stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetRandomNoReservationPointAtRestArea(pointStatus);
    }
    //获取队列第一个位置点
    public Point GetFirstPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetFirstPoint(pointStatus, queueIndex);
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPointByQueueIndex(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetReverseNoReservationPointByQueueIndex(pointStatus, queueIndex);
    }
    #endregion

    #region 设备相关
    public DeviceMgr GetDeviceMgr(UInt16 stationIndex, DeviceType deviceType)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return null;
        return station.GetDeviceMgr(deviceType);
    }
    //站台的上行屏蔽门是否打开
    public bool IsOpenShangXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return false;
        return station.IsOpenShangXingPingBiMen(deviceType);
    }
    //站台的上行屏蔽门状态设置为关闭
    public void CloseShangXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return;
        station.CloseShangXingPingBiMen(deviceType);
    }
    //站台的下行屏蔽门是否打开
    public bool IsOpenXiaXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return false;
        return station.IsOpenXiaXingPingBiMen(deviceType);
    }
    //站台的下行屏蔽门状态设置为关闭
    public void CloseXiaXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return;
        station.CloseXiaXingPingBiMen(deviceType);
    }
    //同步所有类型设备信息
    public virtual void SyncDeviceInfo(PlayerActor playerActor)
    {
        var enumerator = m_stationDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Station station = enumerator.Current.Value;
            if(station != null)
                station.SyncDeviceInfo(playerActor);
        }
        enumerator.Dispose();
    }
    //同步指定设备类型的设备信息
    public void SyncDeviceInfoByDeviceType(DeviceType deviceType, PlayerActor playerActor)
    {
        var enumerator = m_stationDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Station station = enumerator.Current.Value;
            if(station != null)
                station.SyncDeviceInfoByDeviceType(deviceType, playerActor);
        }
        enumerator.Dispose();
    }
    #endregion

    #region Npc相关
    public void AddNpcAction(UInt16 stationIndex, NpcAction npcAction)
    {
        Station station = GetStation(stationIndex);
        if(System.Object.ReferenceEquals(station, null)) return;
        station.AddNpcAction(npcAction);
    }
    public void RemoveNpcAction(UInt16 stationIndex, NpcAction npcAction)
    {
        Station station = GetStation(stationIndex);
        if(System.Object.ReferenceEquals(station, null)) return;
        station.RemoveNpcAction(npcAction.NpcActionStatus, npcAction.NpcId);
    }
    public int GetNpcCount(UInt16 stationIndex, NpcActionStatus npcActionStatus)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return 0;
        return station.GetNpcCount(npcActionStatus);
    }
    public Transform GetNpcParentTransform(UInt16 stationIndex, NpcActionStatus npcActionStatus)
    {
        Station station = GetStation(stationIndex);
        if(station == null) return null;
        return station.GetNpcParentTransform(npcActionStatus);
    }
    //同步Npc信息
    public void SyncNpcInfo(PlayerActor[] stationPlayerActor)
    {
        var enumerator = m_stationDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Station station = enumerator.Current.Value;
            if(station != null)
                station.SyncNpcInfo(stationPlayerActor);
        }
        enumerator.Dispose();
    }
    public void SyncNpcInfo()
    {
        var enumerator = m_stationDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Station station = enumerator.Current.Value;
            if(station != null)
                station.SyncNpcInfo();
        }
        enumerator.Dispose();
    }
    #endregion

    #region Camera相关
    public void AddCamera2ShowCameraIndexList (UInt16 stationIndex, UInt16 cameraIndex) {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.AddCamera2ShowCameraIndexList(cameraIndex);
    }
    public void RemoveCamera4ShowCameraIndexList (UInt16 stationIndex, UInt16 cameraIndex) {
        Station station = GetStation(stationIndex);
        if (station == null) return;
        station.RemoveCamera4ShowCameraIndexList(cameraIndex);
    }
    public Camera GetCamera(UInt16 stationIndex, UInt16 cameraIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetCamera(cameraIndex);
    }
    #endregion
}
