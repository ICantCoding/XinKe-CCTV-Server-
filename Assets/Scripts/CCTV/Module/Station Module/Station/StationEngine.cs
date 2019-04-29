/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-07 11:13:38

** 功能描述:  站台管理引擎

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TDFramework;
using UnityEngine;

public class StationEngine : Singleton<StationEngine>
{
    #region 静态字段
    //用于Npc自增标识
    public static int StartNpcId = 0;
    #endregion

    #region 字段
    private StationMgr m_stationMgr = null;
    #endregion

    #region 属性
    public StationMgr StationMgr
    {
        get { return m_stationMgr; }
    }
    #endregion

    #region 初始化方法
    public void Init()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        m_stationMgr = ReadStationPoint.BuildStationPoint();
        ReadStationPoint.BuildPointInfo(m_stationMgr);
        ReadStationPoint.BuildStationDevices(m_stationMgr);
        ReadStationPoint.BuildStationNpc(m_stationMgr);
        ReadStationPoint.BuildStationCamera(m_stationMgr);
        sw.Stop();
        TimeSpan ts2 = sw.Elapsed;
        UnityEngine.Debug.Log("sw总共花费" + ts2.TotalMilliseconds + "ms.");
    }
    public void Release()
    {

    }
    #endregion

    #region U3DPlayerActor相关
    public void AddPlayerActor(UInt16 stationIndex, PlayerActor playerActor)
    {
        m_stationMgr.AddPlayerActor(stationIndex, playerActor);
    }
    public void RemovePlayerActor(UInt16 stationIndex, PlayerActor playerActor)
    {
        m_stationMgr.RemovePlayerActor(stationIndex, playerActor);
    }
    public void GetU3DPlayerActor(UInt16 stationIndex, ref List<PlayerActor> list)
    {
        m_stationMgr.GetU3DPlayerActor(stationIndex, ref list);
    }
    #endregion

    #region 位置点相关
    public Point GetNoReservationPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Point point = m_stationMgr.GetNoReservationPoint(stationIndex, pointStatus, queueIndex);
        return point;
    }
    public Point GetEmptyPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Point point = m_stationMgr.GetEmptyPoint(stationIndex, pointStatus, queueIndex);
        return point;
    }
    public Point GetEmptyPoint(UInt16 stationIndex, int pointStatus)
    {
        Point point = m_stationMgr.GetEmptyPoint(stationIndex, pointStatus);
        return point;
    }
    //获取队列最后一个位置点
    public Point GetLastPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Point point = m_stationMgr.GetLastPoint(stationIndex, pointStatus, queueIndex);
        return point;
    }
    //获取某个指定的站台
    public Station GetStation(UInt16 stationIndex)
    {
        Station station = m_stationMgr.GetStation(stationIndex);
        return station;
    }
    //获取指定站台的某个PointStatus的PointQueueHash
    public PointQueueHash GetPointQueueHash(UInt16 stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        PointQueueHash pointQueueHash = station.GetPointQueueHash(pointStatus);
        return pointQueueHash;
    }
    //随机获取一个PointQueue队列，并取出该PointQueue中的第一个位置点
    public Point GetFirstPoint2RandomPointQueue(UInt16 stationIndex, int pointStatus)
    {
        return m_stationMgr.GetFirstPoint2RandomPointQueue(stationIndex, pointStatus);
    }
    //随机获取一个PointQueue队列, 并取出首个NoReservation的位置点
    public Point GetNoReservationPoint2RandomPointQueue(UInt16 stationIndex, int pointStatus)
    {
        return m_stationMgr.GetNoReservationPoint2RandomPointQueue(stationIndex, pointStatus);
    }
    //随机获取一个PointQueue队列，指定队列索引范围, 并取出首个NoReservation的位置点
    public Point GetNoReservationPoint2RandomPointQueue(UInt16 stationIndex, int pointStatus, int minQueueIndex, int maxQueueIndex)
    {
        return m_stationMgr.GetNoReservationPoint2RandomPointQueue(stationIndex, pointStatus, minQueueIndex, maxQueueIndex);
    }
    //随机获取一个位置点, 针对休息区
    public Point GetRandomNoReservationPointAtRestArea(UInt16 stationIndex, int pointStatus)
    {
        return m_stationMgr.GetRandomNoReservationPointAtRestArea(stationIndex, pointStatus);
    }
    //获取队列第一个位置点
    public Point GetFirstPoint(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        return m_stationMgr.GetFirstPoint(stationIndex, pointStatus, queueIndex);
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPointByQueueIndex(UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        return m_stationMgr.GetReverseNoReservationPointByQueueIndex(stationIndex, pointStatus, queueIndex);
    }
    #endregion

    #region Npc相关
    public void AddNpcAction(UInt16 stationIndex, NpcAction npcAction)
    {
        m_stationMgr.AddNpcAction(stationIndex, npcAction);
    }
    public void RemoveNpcAction(UInt16 stationIndex, NpcAction npcAction)
    {
        m_stationMgr.RemoveNpcAction(stationIndex, npcAction);
    }
    public int GetNpcCount(UInt16 stationIndex, NpcActionStatus npcActionStatus)
    {
        return m_stationMgr.GetNpcCount(stationIndex, npcActionStatus);
    }
    public Transform GetNpcParentTransform(UInt16 stationIndex, NpcActionStatus npcActionStatus)
    {
        return m_stationMgr.GetNpcParentTransform(stationIndex, npcActionStatus);
    }
    //客户端重连，同步Npc信息
    public void SyncNpcInfo(PlayerActor[] stationPlayerActor)
    {
        m_stationMgr.SyncNpcInfo(stationPlayerActor);
    }
    public void SyncNpcInfo()
    {
        m_stationMgr.SyncNpcInfo();
    }
    #endregion

    #region 设备相关
    public DeviceMgr GetDeviceMgr(UInt16 stationIndex, DeviceType deviceType)
    {
        return m_stationMgr.GetDeviceMgr(stationIndex, deviceType);
    }
    //站台的上行屏蔽门是否打开
    public bool IsOpenShangXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        return m_stationMgr.IsOpenShangXingPingBiMen(stationIndex, deviceType);
    }
    //站台的上行屏蔽门状态设置为关闭
    public void CloseShangXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        m_stationMgr.CloseShangXingPingBiMen(stationIndex, deviceType);
    }
    //站台的下行屏蔽门是否打开
    public bool IsOpenXiaXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        return m_stationMgr.IsOpenXiaXingPingBiMen(stationIndex, deviceType);
    }
    //站台的下行屏蔽门状态设置为关闭
    public void CloseXiaXingPingBiMen(UInt16 stationIndex, DeviceType deviceType)
    {
        m_stationMgr.CloseXiaXingPingBiMen(stationIndex, deviceType);
    }
    //同步所有设备类型的设备信息
    public virtual void SyncDeviceInfo(PlayerActor playerActor)
    {
        m_stationMgr.SyncDeviceInfo(playerActor);
    }
    //同步指定设备类型的设备信息
    public void SyncDeviceInfoByDeviceType(DeviceType deviceType, PlayerActor playerActor)
    {
        m_stationMgr.SyncDeviceInfoByDeviceType(deviceType, playerActor);
    }
    #endregion

    #region 摄像头Camera相关
    public void AddCamera2ShowCameraIndexList (UInt16 stationIndex, UInt16 cameraIndex) {
        m_stationMgr.AddCamera2ShowCameraIndexList(stationIndex, cameraIndex);
    }
    public void RemoveCamera4ShowCameraIndexList (UInt16 stationIndex, UInt16 cameraIndex) {
        m_stationMgr.RemoveCamera4ShowCameraIndexList(stationIndex, cameraIndex);
    }
    public Camera GetCamera(UInt16 stationIndex, UInt16 cameraIndex)
    {
        return m_stationMgr.GetCamera(stationIndex, cameraIndex);
    }
    #endregion
}
