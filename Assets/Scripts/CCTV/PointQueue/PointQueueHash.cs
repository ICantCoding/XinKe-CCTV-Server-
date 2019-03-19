/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-08 10:32:30

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

//队列形成Hash结构队列
public class PointQueueHash
{
    #region 字段
    //是否有未预约的Point
    public bool m_isReservation = false;
    //是否有为空的Point
    public bool m_isEmpty = true;
    //Key为队列的queueIndex
    public Dictionary<int, PointQueue> m_pointQueueDict = new Dictionary<int, PointQueue>();
    #endregion

    #region 属性
    public int Count
    {
        get { return m_pointQueueDict.Count; }
    }
    public Dictionary<int, PointQueue> PointQueueDict
    {
        get { return m_pointQueueDict; }
    }
    public bool IsReservation
    {
        get { return m_isReservation; }
        set { m_isReservation = value; }
    }
    public bool IsEmpty
    {
        get { return m_isEmpty; }
        set { m_isEmpty = value; }
    }
    #endregion

    #region 方法

    #region 获取PointQueue
    //根据指定queueIndex来获取对应的PointQueue
    public PointQueue GetPointQueueByQueueIndex(int queueIndex)
    {
        PointQueue pointQueue = null;
        m_pointQueueDict.TryGetValue(queueIndex, out pointQueue);
        return pointQueue;
    }
    //获取PointQueue，随机
    public PointQueue GetRandomPointQueue()
    {
        int queueIndex = UnityEngine.Random.Range(0, Count);
        return m_pointQueueDict[queueIndex];
    }
    //获取未被预约的PointQueue, 顺序
    public PointQueue GetNoReservationPointQueue()
    {
        if (IsReservation) return null;
        var enumerator = m_pointQueueDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            PointQueue pointQueue = enumerator.Current.Value;
            if (pointQueue.IsProhibited == false && pointQueue.IsReservation == false)
            {
                return pointQueue;
            }
        }
        enumerator.Dispose();
        return null;
    }
    //获取未被预约的PointQueue, 随机
    public PointQueue GetNoReservationPointQueue4Random()
    {
        if (IsReservation) return null;
        List<int> tempList = new List<int>();
        for (int i = 0; i < Count; ++i)
        {
            tempList.Add(i);
        }
        PointQueue pointQueue = null;
        while ((pointQueue == null || pointQueue.IsReservation == true) && tempList.Count > 0)
        {
            int queueIndex = UnityEngine.Random.Range(0, tempList.Count);
            pointQueue = m_pointQueueDict[tempList[queueIndex]];
            tempList.RemoveAt(queueIndex);
        }
        if(pointQueue != null && pointQueue.m_isReservation)
        {
            return null;
        }
        return pointQueue;
    }
    //获取为空的PointQueue, 顺序
    public PointQueue GetEmptyPointQueue()
    {
        if (IsEmpty == false) return null;
        var enumerator = m_pointQueueDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            PointQueue pointQueue = enumerator.Current.Value;
            if (pointQueue.IsProhibited == false && pointQueue.IsEmpty)
            {
                return pointQueue;
            }
        }
        enumerator.Dispose();
        return null;
    }
    //获取为空的PointQueue, 随机
    public PointQueue GetEmptyPointQueue4Random()
    {
        if (IsReservation) return null;
        List<int> tempList = new List<int>();
        for (int i = 0; i < Count; ++i)
        {
            tempList.Add(i);
        }
        PointQueue pointQueue = null;
        while ((pointQueue == null || pointQueue.IsEmpty == false) && tempList.Count > 0)
        {
            int queueIndex = UnityEngine.Random.Range(0, tempList.Count);
            pointQueue = m_pointQueueDict[tempList[queueIndex]];
            tempList.RemoveAt(queueIndex);
        }
        if(pointQueue != null && pointQueue.m_isEmpty == false)
        {
            return null;
        }
        return pointQueue;
    }
    #endregion

    #region 获取Point
    //获取指定队列queueIndex中的未预约位置点, 顺序
    public Point GetNoReservationPointByQueueIndex(int queueIndex)
    {
        if (IsReservation) return null;
        PointQueue pointQueue = GetPointQueueByQueueIndex(queueIndex);
        if (pointQueue == null) return null;
        return pointQueue.GetNoReservationPoint();
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPointByQueueIndex(int queueIndex)
    {
        if (IsReservation) return null;
        PointQueue pointQueue = GetPointQueueByQueueIndex(queueIndex);
        if (pointQueue == null) return null;
        return pointQueue.GetReverseNoReservationPoint();
    }
    //获取未被预约的Point, 返回的是随机的位置点, 这对休息区
    public Point GetRandomNoReservationPointAtRestArea()
    {
        //这里优化处理，因为随机位置点的获取只针对休息区，而休息区仅仅只有一个队列
        PointQueue pointQueue = GetPointQueueByQueueIndex(0); //只为0
        if (pointQueue == null) return null;
        return pointQueue.GetRandomNoReservationPoint();
    }
    //获取顺序PointQueue中的顺序Point
    public Point GetNoReservationPoint()
    {
        if (IsReservation) return null;
        PointQueue pointQueue = GetNoReservationPointQueue();
        if (pointQueue == null) return null;
        return pointQueue.GetNoReservationPoint();
    }
    //随机获取一个PointQueue队列，取出顺序NoReservationPoint
    public Point GetNoReservationPoint2RandomPointQueue()
    {
        if (IsReservation)
        {
            return null;
        }
        PointQueue pointQueue = GetNoReservationPointQueue4Random();
        if (pointQueue == null) 
        {
            return null;
        }
        return pointQueue.GetNoReservationPoint();
    }
    //随机获取一个PointQueue队列，并取出该PointQueue中的第一个位置点
    public Point GetFirstPoint2RandomPointQueue()
    {
        PointQueue pointQueue = GetRandomPointQueue();
        if (pointQueue == null) return null;
        return pointQueue.FirstPoint;
    }
    //获取指定队列queueIndex中的空位置点, 顺序
    public Point GetEmptyPointByQueueIndex(int queueIndex)
    {
        if (IsEmpty == false) return null;
        PointQueue pointQueue = GetPointQueueByQueueIndex(queueIndex);
        if (pointQueue == null) return null;
        return pointQueue.GetEmptyPoint();
    }
    //获取顺序PointQueue中的顺序EmptyPoint
    public Point GetEmptyPoint()
    {
        if (IsEmpty == false) return null;
        PointQueue pointQueue = GetEmptyPointQueue();
        if (pointQueue == null) return null;
        return pointQueue.GetEmptyPoint();
    }
    //获取指定队列的第一个位置点
    public Point GetFirstPoint(int queueIndex)
    {
        PointQueue pointQueue = GetPointQueueByQueueIndex(queueIndex);
        return pointQueue.FirstPoint;
    }
    //获取指定队列的最后一个位置点
    public Point GetLastPoint(int queueIndex)
    {
        PointQueue pointQueue = GetPointQueueByQueueIndex(queueIndex);
        return pointQueue.LastPoint;
    }
    #endregion

    #region PointQueue添加到Hash
    public void AddPointQueue2Hash(int index, PointQueue pointQueue)
    {
        if (pointQueue == null) return;
        if (m_pointQueueDict.ContainsKey(index) == false)
        {
            m_pointQueueDict.Add(index, pointQueue);
        }
    }
    #endregion

    #endregion
}
