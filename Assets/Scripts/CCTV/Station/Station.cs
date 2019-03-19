/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-07 10:23:09

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType
{
    WenShuYuan,				//文殊院
}

public class Station
{

    #region Station管理位置点信息
    #region 字段
    //Key为位置点类别, int对应PointStatus枚举值大小
    public Dictionary<int, PointQueueHash> m_pointQueueHashDict = new Dictionary<int, PointQueueHash>();
    #endregion

    #region 属性
    public Dictionary<int, PointQueueHash> PointQueueHashDict
    {
        get { return m_pointQueueHashDict; }
    }
    #endregion

    #region 索引器
    public PointQueueHash this[int index]
    {
        get { return m_pointQueueHashDict[index]; }
    }
    #endregion

    #region 方法

    #region 获取PointQueueHash
    public PointQueueHash GetPointQueueHash(int pointStatus)
    {
        PointQueueHash pointQueueHash = null;
        m_pointQueueHashDict.TryGetValue(pointStatus, out pointQueueHash);
        return pointQueueHash;
    }
    #endregion

    #region 获取Point
    //指定位置点类型，指定特定的queueIndex, 来获取未预约的位置点
    public Point GetNoReservationPointByQueueIndex(int pointStatus, int queueIndex)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetNoReservationPointByQueueIndex(queueIndex);
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPointByQueueIndex(int pointStatus, int queueIndex)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetReverseNoReservationPointByQueueIndex(queueIndex);
    }
    //指定位置点类型，来获取随机未预约位置点, 针对休息区
    public Point GetRandomNoReservationPointAtRestArea(int pointStatus)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetRandomNoReservationPointAtRestArea();
    }
    //指定位置点类型，来获取随机PointQueue中的顺序未预约位置点
    public Point GetNoReservationPoint2RandomPointQueue(int pointStatus)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetNoReservationPoint2RandomPointQueue();
    }
    //指定位置点类型，指定特定的queueIndex, 来获取空位置点
    public Point GetEmptyPointByQueueIndex(int pointStatus, int queueIndex)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetEmptyPointByQueueIndex(queueIndex);
    }
    //指定位置点类型，来获取空位置点
    public Point GetEmptyPoint(int pointStatus)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetEmptyPoint();
    }
    //获取队列第一个位置点
    public Point GetFirstPoint(int pointStatus, int queueIndex)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetFirstPoint(queueIndex);
    }
    //获取队列最后一个位置点
    public Point GetLastPoint(int pointStatus, int queueIndex)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetLastPoint(queueIndex);
    }
    #endregion

    #region 将PointQueueHash添加到站台中管理
    public void AddPointQueueHash2Station(int pointStatus, PointQueueHash pointQueueHash)
    {
        if (pointQueueHash == null) return;
        if (m_pointQueueHashDict.ContainsKey(pointStatus) == false)
        {
            m_pointQueueHashDict.Add(pointStatus, pointQueueHash);
        }
    }
    #endregion

    #region 新增
    //随机获取一个PointQueue队列，并取出该PointQueue中的第一个位置点
    public Point GetFirstPoint2RandomPointQueue(int pointStatus)
    {
        PointQueueHash pointQueueHash = GetPointQueueHash(pointStatus);
        if (pointQueueHash == null) return null;
        return pointQueueHash.GetFirstPoint2RandomPointQueue();
    }
    #endregion
    #endregion
    #endregion

    #region Station管理设备信息

    #region 字段
    //用于管理不同类型设备
    private Dictionary<DeviceType, DeviceMgr> m_deviceMgrDict = new Dictionary<DeviceType, DeviceMgr>();
    #endregion

    #region 属性
    public Dictionary<DeviceType, DeviceMgr> DeviceMgrDict
    {
        get
        {
            return m_deviceMgrDict;
        }
    }
    #endregion

    #region 方法 
    public void AddDeviceMgr(DeviceMgr mgr)
    {
        if (mgr == null) return;
        if (m_deviceMgrDict.ContainsKey(mgr.DeviceType))
        {
            m_deviceMgrDict.Add(mgr.DeviceType, mgr);
        }
    }
    public DeviceMgr GetDeviceMgr(DeviceType deviceType)
    {
        DeviceMgr mgr = null;
        m_deviceMgrDict.TryGetValue(deviceType, out mgr);
        return mgr;
    }
    public Device GetDevice(DeviceType deviceType, int deviceId)
    {
        DeviceMgr mgr = GetDeviceMgr(deviceType);
        if(mgr == null) return null;
        return mgr.GetDevice(deviceId);
    }
    #endregion

    #endregion
}
