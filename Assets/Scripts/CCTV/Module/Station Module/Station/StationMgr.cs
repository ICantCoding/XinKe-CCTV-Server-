/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-08 11:00:24

** 功能描述:  StationMgr管理Station

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class StationMgr
{
    #region 字段
    //Key为站台的索引, 与场景中Hierarchy面板中的顺序相关
    public Dictionary<int, Station> m_stationDict = new Dictionary<int, Station>();
    #endregion

    #region 属性
    public int Count
    {
        get { return m_stationDict.Count; }
    }
    #endregion

    #region 站台管理相关
    public Station GetStation(int stationIndex)
    {
        Station station = null;
        m_stationDict.TryGetValue(stationIndex, out station);
        return station;
    }
    public void AddStation2Mgr(int stationId, Station station)
    {
        if (station == null) return;
        if (m_stationDict.ContainsKey(stationId) == false)
        {
            m_stationDict.Add(stationId, station);
        }
    }
    #endregion

    #region Point位置点相关
    //指定站台索引, 位置点类型, PointQueue的queueIndex, 来获取未预约的位置点
    public Point GetNoReservationPoint(int stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetNoReservationPointByQueueIndex(pointStatus, queueIndex);
    }
    //指定站台索引, 位置点类型, PointQueue的queueIndex, 来获取空位置点
    public Point GetEmptyPoint(int stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetEmptyPointByQueueIndex(pointStatus, queueIndex);
    }
    //指定站台索引, 位置点类型 来获取空位置点
    public Point GetEmptyPoint(int stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetEmptyPoint(pointStatus);
    }
    //获取队列最后一个位置点
    public Point GetLastPoint(int stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetLastPoint(pointStatus, queueIndex);
    }
    //随机获取一个PointQueue队列，并取出该PointQueue中的第一个位置点
    public Point GetFirstPoint2RandomPointQueue(int stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetFirstPoint2RandomPointQueue(pointStatus);
    }
    //随机获取一个PointQueue队列, 并取出首个NoReservation的位置点
    public Point GetNoReservationPoint2RandomPointQueue(int stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetNoReservationPoint2RandomPointQueue(pointStatus);
    }
    //随机获取一个位置点, 针对休息区
    public Point GetRandomNoReservationPointAtRestArea(int stationIndex, int pointStatus)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetRandomNoReservationPointAtRestArea(pointStatus);
    }
    //获取队列第一个位置点
    public Point GetFirstPoint(int stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetFirstPoint(pointStatus, queueIndex);
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPointByQueueIndex(int stationIndex, int pointStatus, int queueIndex)
    {
        Station station = GetStation(stationIndex);
        if (station == null) return null;
        return station.GetReverseNoReservationPointByQueueIndex(pointStatus, queueIndex);
    }
    #endregion

    #region 设备相关

    #region 状态字段

    #endregion

    #region 方法

    #endregion

    #endregion

    #region Npc相关
    
    #region 状态字段
    
    #endregion

    #region 方法

    #endregion

    #endregion

}
