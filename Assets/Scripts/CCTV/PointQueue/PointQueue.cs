/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-06 14:38:28

** 功能描述:  点组成的队列

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

//点形成队列
public class PointQueue
{
    #region 锁
    private object emptystatus_lock = new object();
    private object getemptypoint_lock = new object();
    #endregion

    #region 字段
    //队列所在Hash的索引值
    public int m_queueIndex = 0;
    //表示PointQueue所属的PointQueueHash
    public PointQueueHash m_pointQueueHash;
    //表示该队列中的点是否有被预约的
    public bool m_isReservation = false;
    //表示该队列是否有点为空
    public bool m_isEmpty = true;
    //表示该队列是否可以使用，如果出现意外事件，可以致命该位置处的队列是否可以使用, 默认为false, 表示没有禁止该队列
    public bool m_isProhibited = false;
    //表示该队列所有点Point的集合
    public List<Point> m_pointList = new List<Point>();
    //表示所有的空Point
    public List<Point> m_emptyPointList = new List<Point>();
    //表示所有的未预约Point
    public List<Point> m_noReservationPointList = new List<Point>();
    #endregion

    #region 属性
    public int Count
    {
        get { return m_pointList.Count; }
    }
    public List<Point> PointList
    {
        get { return m_pointList; }
    }
    public bool IsReservation
    {
        get { return m_isReservation; }
        set
        {
            m_isReservation = value;
            if (m_isReservation == false)
            {
                m_pointQueueHash.IsReservation = false;
            }
        }
    }
    public bool IsEmpty
    {
        get { return m_isEmpty; }
        set
        {
            m_isEmpty = value;
            if (m_isEmpty == true)
            {
                m_pointQueueHash.IsEmpty = true;
            }
        }
    }
    public bool IsProhibited
    {
        get { return m_isProhibited; }
        set { m_isProhibited = value; }
    }
    public Point FirstPoint
    {
        get { return m_pointList[0]; }
    }
    public Point LastPoint
    {
        get { return m_pointList[Count - 1]; }
    }
    #endregion

    #region 方法

    #region 将Point添加到队列中
    public void AddPoint2Queue(Point point)
    {
        if (point == null) return;
        m_pointList.Add(point);
    }
    #endregion

    #region 得到NoReservationPoint
    //获取未被预约的Point, 返回的是随机的位置点
    public Point GetRandomNoReservationPoint()
    {
        List<int> tempList = new List<int>();
        for (int i = 0; i < Count; ++i)
        {
            tempList.Add(i);
        }
        Point point = null;
        while ((point == null || point.IsReservation == true) && tempList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, tempList.Count);
            point = m_pointList[tempList[index]];
            tempList.RemoveAt(index);
        }
        if (point != null && point.m_isReservation)
        {
            return null;
        }
        return point;
    }
    public Point GetRandomNoReservationPoint_XXXXXXX()
    {
        int index = UnityEngine.Random.Range(0, m_noReservationPointList.Count);
        return m_noReservationPointList[index];
    }
    //获取未被预约的Point, 返回的首先是队列最前面的位置点
    public Point GetNoReservationPoint()
    {
        for (int i = 0; i < Count; ++i)
        {
            Point point = m_pointList[i];
            if (point.IsReservation == false)
            {
                return point;
            }
        }
        return null;
    }
    //获取未被预约的Point, 返回的首先是队列最后边没有被预约的位置点
    public Point GetReverseNoReservationPoint()
    {
        for (int i = Count - 1; i > 0; --i)
        {
            Point point = m_pointList[i];
            if (point.IsReservation == false)
            {
                return point;
            }
        }
        return null;
    }
    #endregion

    #region 得到EmptyPoint
    //获取空位置点, 返回的是随机的位置点
    public Point GetRandomEmptyPoint()
    {
        List<int> tempList = new List<int>();
        for (int i = 0; i < Count; ++i)
        {
            tempList.Add(i);
        }
        Point point = null;
        while ((point == null || point.IsEmpty == false) && tempList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, tempList.Count);
            point = m_pointList[tempList[index]];
            tempList.RemoveAt(index);
        }
        if(point != null && point.m_isEmpty == false)
        {
            return null;
        }
        return point;
    }
    public Point GetRandomEmptyPoint_XXXXXX()
    {
        int index = UnityEngine.Random.Range(0, m_emptyPointList.Count);
        return m_emptyPointList[index];
    }
    //获取空位置点, 返回的首先是队列最前面的位置点
    public Point GetEmptyPoint()
    {
        for (int i = 0; i < Count; ++i)
        {
            Point point = m_pointList[i];
            if (point.IsEmpty)
            {
                return point;
            }
        }
        return null;
    }
    #endregion

    #region 额外方法
    //判断PointQueue是否拥有空位置点的方法
    public bool IsEmptyFunc()
    {
        for (int i = 0; i < Count; ++i)
        {
            Point point = m_pointList[i];
            if (point.IsEmpty)
            {
                return true;
            }
        }
        return false;
    }
    //判断PointQueue是否拥有未预约的位置点的方法
    public bool IsReservationFunc()
    {
        for (int i = 0; i < Count; ++i)
        {
            Point point = m_pointList[i];
            if (point.IsReservation == false)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #endregion
}






