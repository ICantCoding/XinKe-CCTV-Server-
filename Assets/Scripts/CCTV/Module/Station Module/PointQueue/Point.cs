/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-06 14:34:54

** 功能描述:  站台中某个点的类表示

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//点状态
public enum PointStatus
{
    EnterStation = 0,               //进站点
    BuyTicket,      		        //购票排队点
    BuyTicket_RestArea,             //购票排队满员的时候，有一个休息区
    EnterCheckTicket,    	        //进站检票排队点
    EnterCheckTicket_RestArea,      //进站检票排队满员的时候，有一个休息区
    EnterCheckTicketAfter,          //进站检票之后排队点
    WaitTrain_Up,        	        //等待列车，上行上车排队点
    WaitTrain_Up_RestArea,          //等待雷车, 上行上车排队满员的时候，有一个休息区
    WaitTrain_Down,                 //等待列车，下行上车排队点
    WaitTrain_Down_RestArea,        //等待列车，下行上车排队满员的时候，有一个休息区
    Train_Up,                       //列车上，上行车上NPC生成消失点
    Train_Down,                     //列车上，下行车上NPC生成消失点
    NpcDownTrain_Up,                //Npc 上行方向下车排队点
    NpcDownTrain_Down,              //Npc 下行方向下车排队点
    ExitCheckTicket,		        //出站检票排队点
    ExitCheckTicket_RestArea,       //出站检票排队满员的时候，有一个休息区
    ExitCheckTicketAfter,           //出站检票之后排队点
    ExitStation, 			        //出站点 

    None = 10000,                   //表示不是任何位置点状态
}

public class PointStatusRelationship
{
    public PointStatus m_prePointStatus;    //上一个衔接位置点状态
    public PointStatus m_curPointStatus;    //当前位置点状态
    public PointStatus m_nextPointStatus;   //下一个衔接位置点状态
}
public class PointSatusRelationshipMap
{
    #region 字段
    public Dictionary<PointStatus, PointStatusRelationship> m_pointStatusRelatioinshipDict =
        new Dictionary<PointStatus, PointStatusRelationship>();
    #endregion

    #region 构造函数
    public PointSatusRelationshipMap()
    {
        //进站位置点 衔接位置点映射
        PointStatusRelationship ship = new PointStatusRelationship()
        {
            m_prePointStatus = PointStatus.None,
            m_curPointStatus = PointStatus.EnterCheckTicket,
            m_nextPointStatus = PointStatus.BuyTicket,
        };
        m_pointStatusRelatioinshipDict.Add(ship.m_curPointStatus, ship);
        //买票位置点 衔接位置点映射
        ship = new PointStatusRelationship()
        {
            m_prePointStatus = PointStatus.EnterCheckTicket,
            m_curPointStatus = PointStatus.BuyTicket,
            m_nextPointStatus = PointStatus.EnterCheckTicket,
        };
        m_pointStatusRelatioinshipDict.Add(ship.m_curPointStatus, ship);
        //检票位置点 衔接位置点映射
        ship = new PointStatusRelationship()
        {
            m_prePointStatus = PointStatus.BuyTicket,
            m_curPointStatus = PointStatus.EnterCheckTicket,
            m_nextPointStatus = PointStatus.EnterCheckTicketAfter,
        };
        m_pointStatusRelatioinshipDict.Add(ship.m_curPointStatus, ship);
        //检票后位置点 衔接位置点映射
        ship = new PointStatusRelationship()
        {
            m_prePointStatus = PointStatus.EnterCheckTicket,
            m_curPointStatus = PointStatus.EnterCheckTicketAfter,
            m_nextPointStatus = PointStatus.WaitTrain_Up,
        };
        m_pointStatusRelatioinshipDict.Add(ship.m_curPointStatus, ship);
        //等待列车，上行上车排队点 衔接位置点映射
        ship = new PointStatusRelationship()
        {
            m_prePointStatus = PointStatus.EnterCheckTicketAfter,
            m_curPointStatus = PointStatus.WaitTrain_Up,
            m_nextPointStatus = PointStatus.Train_Up,
        };
        m_pointStatusRelatioinshipDict.Add(ship.m_curPointStatus, ship);
        //列车上，上行车上NPC生成消失点 衔接位置点映射
        ship = new PointStatusRelationship()
        {
            m_prePointStatus = PointStatus.WaitTrain_Up,
            m_curPointStatus = PointStatus.Train_Up,
            m_nextPointStatus = PointStatus.None,
        };
        m_pointStatusRelatioinshipDict.Add(ship.m_curPointStatus, ship);
    }
    #endregion

    #region 方法
    public PointStatusRelationship GetPointStatusRelationship(PointStatus curPointStatus)
    {
        return m_pointStatusRelatioinshipDict[curPointStatus];
    }
    #endregion
}

public class Point
{

    #region 锁
    private object empty_lock = new object();
    private object reservation_lock = new object();
    #endregion

    public PointBehaviour m_pb;
    public PointQueue m_belongPointQueue;   //当前点所属的PointQueue
    public int m_queueIndex;                //当前点所属的PointQueue的queueIndex
    public PointStatus m_pointStatus;       //当前点的NPC所处行为状态
    public bool m_isReservation;            //当前点有提前被预约
    public bool m_isEmpty;                  //当前点是否为被占用
    public Point m_prePoint;                //当前点的上一个点
    public Point m_nextPoint;               //当前点的下一个点
    public Point m_afterPoint;              //之后一定要到达的点，比如过闸机后，一定要到达的位置点
    public int m_npcId;                     //当前点被某个Npc占用, 该Npc的Id
    public NpcAction m_npcAction;           //当前点被占用Npc的Action
    public bool m_isDeviceCtrl;               //当前点是否与设备相关
    public int m_deviceId;                  //当前点是否与某个设备关联
    public bool m_isRestAreaPoint;          //当前点是否是休息区的某个位置点
    public Device m_device;                 //当前点关联的设备

    public float PosX;
    public float PosY;
    public float PosZ;
    public float EulerAngleX;
    public float EulerAngleY;
    public float EulerAngleZ;

    #region 属性
    public bool IsReservation
    {
        get
        {
            lock (reservation_lock)
            {
                return m_isReservation;
            }
        }
        set
        {
            lock (reservation_lock)
            {
                m_isReservation = value;
                // m_pb.IsReservation = value;
                //Point未预约，那么该Point所在队列的PointQueue也为未预约
                if (m_isReservation == false)
                {
                    m_belongPointQueue.IsReservation = false;
                    m_belongPointQueue.m_noReservationPointList.Add(this);
                }
                else
                {
                    m_belongPointQueue.m_noReservationPointList.Remove(this);
                    m_belongPointQueue.IsReservation = m_belongPointQueue.IsReservationFunc();
                }
            }
        }
    }
    public bool IsEmpty
    {
        get
        {
            lock (empty_lock)
            {
                return m_isEmpty;
            }
        }
        set
        {
            lock (empty_lock)
            {
                m_isEmpty = value;
                // m_pb.IsEmpty = value;
                //Point为空, 那么该Point所在的队列的PointQueue也为空
                if (m_isEmpty == true)
                {
                    m_belongPointQueue.IsEmpty = true;
                    m_belongPointQueue.m_emptyPointList.Add(this);
                }
                else
                {
                    m_belongPointQueue.m_emptyPointList.Remove(this);
                    m_belongPointQueue.IsEmpty = m_belongPointQueue.IsEmptyFunc();
                }
            }
        }
    }
    public bool IsRestAreaPoint
    {
        get { return m_isRestAreaPoint; }
        set
        {
            m_isRestAreaPoint = value;
            m_pb.IsRestArea = value;
        }
    }
    public bool IsDeviceCtrl
    {
        get { return m_isDeviceCtrl; }
        set { m_isDeviceCtrl = value; }
    }
    public void Reset()
    {

    }
    #endregion
}

