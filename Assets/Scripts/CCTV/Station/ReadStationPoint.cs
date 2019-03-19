/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-07 10:14:07

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReadStationPoint
{
    public static StationMgr BuildStationPoint()
    {
        Transform pointRootTrans = GameObject.Find("Cctv/PointRoot").transform;
        if (pointRootTrans == null) return null;
        StationMgr stationMgr = new StationMgr();
        int stationCount = pointRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = pointRootTrans.GetChild(i);
            int stationIndex = (int)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = XXX(stationTrans);
            stationMgr.AddStation2Mgr(stationIndex, station);
        }
        return stationMgr;
    }
    private static Station XXX(Transform stationTrans)
    {
        if (stationTrans == null) return null;
        Station station = new Station();
        int pointStatusCount = stationTrans.childCount;
        for (int i = 0; i < pointStatusCount; i++)
        {
            Transform pointStatusTrans = stationTrans.GetChild(i);
            int pointStatus = (int)System.Enum.Parse(typeof(PointStatus), pointStatusTrans.gameObject.name);
            PointQueueHash pointQueueHash = YYY(pointStatusTrans);
            station.AddPointQueueHash2Station(pointStatus, pointQueueHash);
        }
        return station;
    }
    private static PointQueueHash YYY(Transform pointStatusTrans)
    {
        if (pointStatusTrans == null) return null;
        PointQueueHash pointQueueHash = new PointQueueHash();
        int queueCount = pointStatusTrans.childCount;
        for (int i = 0; i < queueCount; i++)
        {
            Transform queueTrans = pointStatusTrans.GetChild(i);
            int queueIndex = int.Parse(queueTrans.gameObject.name);
            PointQueue pointQueue = ZZZ(queueTrans, queueIndex);
            pointQueue.m_pointQueueHash = pointQueueHash;
            pointQueueHash.AddPointQueue2Hash(queueIndex, pointQueue);
        }
        return pointQueueHash;
    }
    private static PointQueue ZZZ(Transform queueTrans, int queueIndex)
    {
        if (queueTrans == null) return null;
        PointQueue pointQueue = new PointQueue();
        pointQueue.m_queueIndex = queueIndex;
        int pointCount = queueTrans.childCount;
        for (int i = 0; i < pointCount; i++)
        {
            Transform pointTrans = queueTrans.GetChild(i);
            string pointName = pointTrans.gameObject.name;
            pointName = pointName.Substring(0, pointName.Length - 6);
            PointStatus pointStatus = (PointStatus)System.Enum.Parse(typeof(PointStatus), pointName);
            bool isRestAreaPoint = false;
            if (pointName.Contains("RestArea"))
            {
                isRestAreaPoint = true;
            }
            bool isDeviceCtrl = false;
            if (i == 0 && (pointName == "EnterCheckTicket" ||
                pointName == "ExitCheckTicket" ||
                pointName == "WaitTrain_Up" ||
                pointName == "WaitTrain_Down"))
            {
                isDeviceCtrl = true;
            }
            float posX = pointTrans.position.x;
            float posY = pointTrans.position.y;
            float posZ = pointTrans.position.z;
            float eulerAngleX = pointTrans.eulerAngles.x;
            float eulerAngleY = pointTrans.eulerAngles.y;
            float eulerAngleZ = pointTrans.eulerAngles.z;
            Point point = new Point()
            {
                m_belongPointQueue = pointQueue,
                m_queueIndex = queueIndex,
                m_pointStatus = pointStatus,
                m_isReservation = false,  //未提前被预约
                m_isEmpty = true,   //为空
                m_isRestAreaPoint = isRestAreaPoint,
                m_isDeviceCtrl = isDeviceCtrl,
                PosX = posX,
                PosY = posY,
                PosZ = posZ,
                EulerAngleX = eulerAngleX,
                EulerAngleY = eulerAngleY,
                EulerAngleZ = eulerAngleZ,
            };
            pointQueue.AddPoint2Queue(point);
            PointBehaviour pb = pointTrans.GetComponent<PointBehaviour>();
            point.m_pb = pb;
            point.m_pb.IsRestArea = isRestAreaPoint;
        }
        return pointQueue;
    }

    public static void AAA(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        foreach (var item in System.Enum.GetValues(typeof(StationType)))
        {
            Station station = stationMgr.GetStation((int)item);
            BBB(station);
        }
    }
    private static void BBB(Station station)
    {
        if (station == null) return;
        var enumerator = station.PointQueueHashDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            PointQueueHash pointQueueHash = enumerator.Current.Value;
            CCC(pointQueueHash);
        }
        enumerator.Dispose();
    }
    private static void CCC(PointQueueHash pointQueueHash)
    {
        if (pointQueueHash == null) return;
        var enumerator = pointQueueHash.PointQueueDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            PointQueue pointQueue = enumerator.Current.Value;
            DDD(pointQueue);
        }
        enumerator.Dispose();
    }
    private static void DDD(PointQueue pointQueue)
    {
        if (pointQueue == null) return;
        int startIndex = 0;
        int endIndex = pointQueue.Count - 1;
        Point nextPoint = null;
        for (int i = 0; i < pointQueue.Count; ++i)
        {
            Point point = pointQueue.PointList[i];
            if ((startIndex == endIndex))
            {
                point.m_prePoint = null;
                point.m_nextPoint = null;
            }
            else if (startIndex == i)
            {
                //表示是队列的第一个点
                nextPoint = point;
                point.m_nextPoint = null;
            }
            else if (endIndex == i)
            {
                //表示是队列的最后一个点
                point.m_nextPoint = nextPoint;
                point.m_prePoint = null;
            }
            else
            {
                //表示是除队列首尾点的其他所有点
                nextPoint.m_prePoint = point;
                point.m_nextPoint = nextPoint;
                nextPoint = point;
            }
        }
    }

    #region 读取场景中各个站台的设备
    public static void BuildStationDevices(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        Transform deviceRootTrans = GameObject.Find("Device/DeviceRoot").transform;
        if (deviceRootTrans == null) return;
        int stationCount = deviceRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = deviceRootTrans.GetChild(i);
            int stationIndex = (int)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = stationMgr.GetStation(stationIndex);
            if (station == null) return;
            DeviceMgr deviceMgr = HHH(stationMgr, stationTrans, stationIndex);
            station.AddDeviceMgr(deviceMgr);
        }
    }
    private static DeviceMgr HHH(StationMgr stationMgr, Transform stationTrans, int stationIndex)
    {
        DeviceMgr deviceMgr = new DeviceMgr();
        int deviceTypeCount = stationTrans.childCount;
        for (int i = 0; i < deviceTypeCount; ++i)
        {
            Transform deviceTypeTrans = stationTrans.GetChild(i);
            DeviceType deviceType = (DeviceType)System.Enum.Parse(typeof(DeviceType), deviceTypeTrans.gameObject.name);
            deviceMgr.DeviceType = deviceType;
            JJJ(stationMgr, deviceMgr, deviceTypeTrans, stationIndex, deviceType);
        }
        return deviceMgr;
    }
    private static void JJJ(StationMgr stationMgr, DeviceMgr deviceMgr, Transform deviceTypeTrans, int stationIndex, DeviceType deviceType)
    {
        int deviceCount = deviceTypeTrans.childCount;
        for (int i = 0; i < deviceCount; ++i)
        {
            Transform deviceTrans = deviceTypeTrans.GetChild(i);
            Device deviceCom = deviceTrans.GetComponent<Device>();
            if (deviceCom == null) continue;
            deviceCom.DeviceId = (int)deviceType + i + 1;
            deviceMgr.AddDevice(deviceCom);

            #region 设备关联Point
            string name = deviceTrans.gameObject.name;
            string[] str = name.Split('|');
            if (str.Length >= 3)
            {
                int pointStatus = (int)System.Enum.Parse(typeof(PointStatus), str[1]);
                int queueIndex1 = int.Parse(str[2]);
                Point point = GetFirstPoint(stationMgr, stationIndex, pointStatus, queueIndex1);
                point.m_device = deviceCom;
                point.m_pb.Device = deviceCom;
                if (str.Length == 4)
                {
                    //有两个点关联同一个设备
                    int queueIndex2 = int.Parse(str[3]);
                    point = GetFirstPoint(stationMgr, stationIndex, pointStatus, queueIndex2);
                    point.m_device = deviceCom;
                    point.m_pb.Device = deviceCom;
                }
            }
            #endregion
        }
    }
    private static Point GetFirstPoint(StationMgr stationMgr, int stationIndex, int pointStatus, int queueIndex)
    {
        Point point = stationMgr.GetFirstPoint(stationIndex, pointStatus, queueIndex);
        return point;
    }
    #endregion
}
