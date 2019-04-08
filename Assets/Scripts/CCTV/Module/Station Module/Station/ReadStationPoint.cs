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
using System.Threading;

public class ReadStationPoint
{
    #region 读取点位置
    public static StationMgr BuildStationPoint()
    {
        GameObject go = GameObject.Find("Point/PointRoot");
        if (go == null) return null;
        Transform pointRootTrans = go.transform;
        if (pointRootTrans == null) return null;
        StationMgr stationMgr = new StationMgr();
        int stationCount = pointRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = pointRootTrans.GetChild(i);
            System.UInt16 stationIndex = (System.UInt16)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = XXX(stationTrans);
            stationMgr.AddStation2Mgr(stationIndex, station);
        }
        go.transform.parent.gameObject.SetActive(false);
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
                pointName == "WaitTrain_Down" ||
                pointName == "Train_Up_Birth" ||
                pointName == "Train_Down_Birth"))
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
            // PointBehaviour pb = pointTrans.GetComponent<PointBehaviour>();
            // point.m_pb = pb;
            // point.m_pb.IsRestArea = isRestAreaPoint;
        }
        return pointQueue;
    }
    #endregion

    #region 赋值点位置重要信息
    public static void BuildPointInfo(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        foreach (var item in System.Enum.GetValues(typeof(StationType)))
        {
            Station station = stationMgr.GetStation((System.UInt16)item);
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
    #endregion

    #region 读取场景中各个站台的设备
    public static void BuildStationDevices(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        GameObject go = GameObject.Find("Device/DeviceRoot");
        if (go == null) return;
        Transform deviceRootTrans = go.transform;
        if (deviceRootTrans == null) return;
        int stationCount = deviceRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = deviceRootTrans.GetChild(i);
            System.UInt16 stationIndex = (System.UInt16)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = stationMgr.GetStation(stationIndex);
            if (station == null) return;
            HHH(stationMgr, station, stationTrans, stationIndex);
        }
    }
    private static void HHH(StationMgr stationMgr, Station station, Transform stationTrans, System.UInt16 stationIndex)
    {
        int deviceTypeCount = stationTrans.childCount;
        for (int i = 0; i < deviceTypeCount; ++i)
        {
            DeviceMgr deviceMgr = null;
            Transform deviceTypeTrans = stationTrans.GetChild(i);
            DeviceType deviceType = (DeviceType)System.Enum.Parse(typeof(DeviceType), deviceTypeTrans.gameObject.name);
            if (deviceType == DeviceType.ZhaJi)
            {
                deviceMgr = new ZhaJiMgr();
            }
            else if (deviceType == DeviceType.PingBiMen)
            {
                deviceMgr = new PingBiMenMgr();
            }
            //deviceTypeTrans是闸机父容器或者屏蔽门父容器
            JJJ(stationMgr, deviceMgr, deviceTypeTrans, stationIndex, deviceType);
            station.AddDeviceMgr(deviceMgr);
        }
    }
    private static void JJJ(StationMgr stationMgr, DeviceMgr deviceMgr, Transform deviceTypeTrans, System.UInt16 stationIndex, DeviceType deviceType)
    {
        int deviceCount = deviceTypeTrans.childCount;
        for (int i = 0; i < deviceCount; ++i)
        {
            Transform deviceTrans = deviceTypeTrans.GetChild(i);
            string deviceName = deviceTrans.gameObject.name;
            Device deviceCom = deviceTrans.GetComponent<Device>();
            if (deviceCom == null) continue;
            deviceCom.DeviceId = (int)deviceType + i + 1;
            deviceCom.StationIndex = stationIndex;
            deviceCom.DeviceType = deviceType;
            deviceMgr.AddDevice(deviceCom);
            OneStation station = TDFramework.SingletonMgr.GameGlobalInfo.StationDeviceAndPointInfo.GetStation(stationIndex);
            // deviceType, deviceName
            PointBindInfoList list = station.GetPointBindInfoList(deviceType, deviceName);
            int pointCount = list.m_pointBindInfoList.Count;
            for (int j = 0; j < pointCount; ++j)
            {
                PointBindInfo pointBindInfo = list.m_pointBindInfoList[j];
                int pointStatus = (int)System.Enum.Parse(typeof(PointStatus), pointBindInfo.m_name);
                int queueIndex = pointBindInfo.m_queueIndex;
                Point point = GetFirstPoint(stationMgr, stationIndex, pointStatus, queueIndex);
                point.m_device = deviceCom;
            }
            #region 设备是屏蔽门，需管理上行和下行屏蔽门
            if (deviceCom is PingBiMenDevice)
            {
                PingBiMenMgr pingBiMenMgr = (PingBiMenMgr)deviceMgr;
                PingBiMenDevice device = (PingBiMenDevice)deviceCom;
                if (device.PingBiMenType == PingBiMenType.Down)
                {
                    //下行屏蔽门
                    pingBiMenMgr.AddDevice2XiaXingPingBiMenList(deviceCom);
                }
                else if (device.PingBiMenType == PingBiMenType.Up)
                {
                    //上行屏蔽门
                    pingBiMenMgr.AddDevice2ShangXingPingBiMenList(deviceCom);
                }
            }
            #endregion
        }
    }
    private static void PointBindDevice(string[] str, StationMgr stationMgr, System.UInt16 stationIndex, Device deviceCom)
    {
        if (str.Length >= 3)
        {
            int pointStatus = (int)System.Enum.Parse(typeof(PointStatus), str[1]);
            int queueIndex1 = int.Parse(str[2]);
            Point point = GetFirstPoint(stationMgr, stationIndex, pointStatus, queueIndex1);
            point.m_device = deviceCom;
            if (str.Length == 4)
            {
                //有两个点关联同一个设备
                int queueIndex2 = int.Parse(str[3]);
                point = GetFirstPoint(stationMgr, stationIndex, pointStatus, queueIndex2);
                point.m_device = deviceCom;
            }
        }
    }
    private static Point GetFirstPoint(StationMgr stationMgr, System.UInt16 stationIndex, int pointStatus, int queueIndex)
    {
        Point point = stationMgr.GetFirstPoint(stationIndex, pointStatus, queueIndex);
        return point;
    }
    #endregion


    #region 读取场景中预先存在的Npc
    public static void BuildStationNpc(StationMgr stationMgr)
    {
        if (stationMgr == null) return;
        GameObject go = GameObject.Find("Npc/NpcRoot");
        if (go == null) return;
        Transform npcRootTrans = go.transform;
        if (npcRootTrans == null) return;
        int stationCount = npcRootTrans.childCount;
        for (int i = 0; i < stationCount; ++i)
        {
            Transform stationTrans = npcRootTrans.GetChild(i);
            System.UInt16 stationIndex = (System.UInt16)System.Enum.Parse(typeof(StationType), stationTrans.gameObject.name);
            Station station = stationMgr.GetStation(stationIndex);
            if (station == null) return;
            VVV(station, stationTrans, stationIndex);
        }
    }
    private static void VVV(Station station, Transform stationTrans, System.UInt16 stationIndex)
    {
        NpcMgr npcMgr = null;
        int npcActionStatusCount = stationTrans.childCount;
        for (int i = 0; i < npcActionStatusCount; ++i)
        {
            npcMgr = new NpcMgr();
            Transform npcActionStatusTrans = stationTrans.GetChild(i);
            NpcActionStatus npcActionStatus = (NpcActionStatus)System.Enum.Parse(typeof(NpcActionStatus), npcActionStatusTrans.gameObject.name);
            npcMgr.NpcActionStatus = npcActionStatus;
            npcMgr.NpcParentTransform = npcActionStatusTrans;
            station.AddNpcMgr(npcMgr);
            NNN(npcMgr, npcActionStatusTrans, stationIndex);
        }
    }
    private static void NNN(NpcMgr npcMgr, Transform npcActionStatusTrans, System.UInt16 stationIndex)
    {
        int npcCount = npcActionStatusTrans.childCount;
        for (int i = 0; i < npcCount; ++i)
        {
            Transform npcTrans = npcActionStatusTrans.GetChild(i);
            NpcAction npcAction = npcTrans.GetComponent<NpcAction>();
            if (npcAction == null) continue;
            // int npcId = Interlocked.Increment(ref StationEngine.StartNpcId); //原子操作
            // npcAction.NpcId = npcId;
            npcAction.StationIndex = stationIndex;
            npcMgr.AddNpcAction(npcAction);
        }
    }
    #endregion
}
