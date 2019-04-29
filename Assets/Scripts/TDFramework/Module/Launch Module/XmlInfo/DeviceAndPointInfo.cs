using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TDFramework;

//站台中设备与位置点关联的Xml信息表
/*
<ZhaoYing>
    <ZhaJiList>
        <ZhaJi>
            <Name>ZhaJi1</Name>
            <PointList>
                <Point>
                    <Name>EnterCheckTicket</Name>
                    <QueueIndex>0</QueueIndex>
                </Point>
            </PointList>
        </ZhaJi>
        <ZhaJi>
            <Name>ZhaJi2</Name>
            <PointList>
                <Point>
                    <Name>EnterCheckTicket</Name>
                    <QueueIndex>0</QueueIndex>
                </Point>
            </PointList>
        </ZhaJi>
    </ZhaJiList>
    <PingBiMenList>
        <PingBiMen>
            <Name>PingBiMen1</Name>
            <PointList>
                <PointBindInfo>
                    <Name>WaitTrain_Down</Name>
                    <QueueIndex>46</QueueIndex>
                </PointBindInfo>
                <PointBindInfo>
                    <Name>WaitTrain_Down</Name>
                    <QueueIndex>47</QueueIndex>                    
                </PointBindInfo>
                <PointBindInfo>
                    <Name>DownTrain_Down></Name>
                    <QueueIndex>23</QueueIndex>
                </PointBindInfo>
                <PointBindInfo>
                    <Name>Train_Down_Birth</Name>
                    <QueueIndex>23<QueueIndex>
                </PointBindInfo>
            </PointList>
        </PingBiMen>
    </PingBiMenList>
</ZhaoYing>
*/
//与设备绑定的位置点信息，包括位置点类型，位置点队列索引
public class PointBindInfo
{
    [XmlElement("Name")]
    public string m_name;
    [XmlElement("QueueIndex")]
    public int m_queueIndex;
}
public class PointBindInfoList
{
    [XmlElement("PointBindInfo")]
    public List<PointBindInfo> m_pointBindInfoList;
}
public class PingBiMen
{
    [XmlElement("Name")]
    public string m_name;
    [XmlElement("PointBindInfoList")]
    public PointBindInfoList m_pointBindInfoList;
}
public class PingBiMenList
{
    [XmlElement("PingBiMen")]
    public List<PingBiMen> m_pingBiMenList;
}
public class ZhaJi
{
    [XmlElement("Name")]
    public string m_name;
    [XmlElement("PointBindInfoList")]
    public PointBindInfoList m_pointBindInfoList;
}
public class ZhaJiList
{
    [XmlElement("ZhaJi")]
    public List<ZhaJi> m_zhajiList;
}
public class OneStation
{
    [XmlElement("StationName")]
    public string m_stationName;
    [XmlElement("PingBiMenList")]
    public PingBiMenList m_pingBiMenList;
    [XmlElement("ZhaJiList")]
    public ZhaJiList m_zhaJiList;

    public PointBindInfoList GetPointBindInfoList(DeviceType deviceType, string deviceName)
    {
        if (deviceType == DeviceType.ZhaJi)
        {
            int count = m_zhaJiList.m_zhajiList.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_zhaJiList.m_zhajiList[i].m_name == deviceName)
                {
                    return m_zhaJiList.m_zhajiList[i].m_pointBindInfoList;
                }
            }
        }
        else if (deviceType == DeviceType.PingBiMen)
        {
            int count = m_pingBiMenList.m_pingBiMenList.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_pingBiMenList.m_pingBiMenList[i].m_name == deviceName)
                {
                    return m_pingBiMenList.m_pingBiMenList[i].m_pointBindInfoList;
                }
            }
        }
        return null;
    }
}
public class StationDeviceAndPointInfo
{
    [XmlElement("OneStation")]
    public List<OneStation> m_oneStationList;

    public OneStation GetStation(string stationName)
    {
        if (string.IsNullOrEmpty(stationName)) return null;
        int count = m_oneStationList.Count;
        for (int i = 0; i < count; i++)
        {
            if (m_oneStationList[i].m_stationName == stationName)
            {
                return m_oneStationList[i];
            }
        }
        return null;
    }
    public OneStation GetStation(System.UInt16 stationIndex)
    {
        StationType stationType = (StationType)stationIndex;
        string stationName = stationType.ToString();
        return GetStation(stationName);
    }
}

public class DeviceAndPointInfo
{
    #region 序列化和反序列化
    public static void SerializationDeviceAndPointInfo2Xml()
    {
        StationDeviceAndPointInfo stationDeviceAndPointInfo = new StationDeviceAndPointInfo();
        stationDeviceAndPointInfo.m_oneStationList = new List<OneStation>();

        #region 赵营
        OneStation zhaoying = new OneStation();
        zhaoying.m_stationName = "ZhaoYing";
        ZhaJiList zhaJiList = new ZhaJiList();
        zhaJiList.m_zhajiList = new List<ZhaJi>();
        zhaoying.m_zhaJiList = zhaJiList;
        PingBiMenList pingBiMenList = new PingBiMenList();
        pingBiMenList.m_pingBiMenList = new List<PingBiMen>();
        zhaoying.m_pingBiMenList = pingBiMenList;
        stationDeviceAndPointInfo.m_oneStationList.Add(zhaoying);
        #region 闸机
        #region 闸机1
        ZhaJi zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi1";
        PointBindInfoList pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        PointBindInfo pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机2
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi2";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机3
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi3";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机4
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi4";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机5
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi5";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机6
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi6";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机7
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi7";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "EnterCheckTicket";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机8
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi8";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机9
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi9";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机10
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi10";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机11
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi11";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机12
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi12";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机13
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi13";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 闸机14
        zhaJi = new ZhaJi();
        zhaJi.m_name = "ZhaJi14";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        zhaJi.m_pointBindInfoList = pointBindInfoList;
        zhaJiList.m_zhajiList.Add(zhaJi);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "ExitCheckTicket";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #endregion

        #region 屏蔽门
        #region 屏蔽门1
        PingBiMen pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen1";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 46;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 47;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 23;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 23;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门2
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen2";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 44;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 45;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 22;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 22;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门3
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen3";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 42;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 43;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 21;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 21;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门4
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen4";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 40;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 41;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 20;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 20;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门5
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen5";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 38;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 39;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 19;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 19;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门6
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen6";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 36;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 37;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 18;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 18;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门7
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen7";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 34;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 35;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 17;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 17;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门8
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen8";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 32;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 33;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 16;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 16;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门9
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen9";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 30;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 31;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 15;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 15;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门10
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen10";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 28;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 29;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 14;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 14;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门11
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen11";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 26;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 27;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 13;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 13;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门12
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen12";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 24;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 25;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 12;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 12;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门13
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen13";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 22;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 23;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 11;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 11;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门14
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen14";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 20;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 21;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 10;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 10;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门15
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen15";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 18;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 19;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 9;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 9;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门16
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen16";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 16;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 17;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 8;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 8;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门17
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen17";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 14;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 15;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 7;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 7;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门18
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen18";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 12;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 13;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门19
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen19";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 10;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 11;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门20
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen20";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 8;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 9;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门21
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen21";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 7;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门22
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen22";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门23
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen23";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门24
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen24";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Down";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Down";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Down_Birth";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门25
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen25";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 0;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门26
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen26";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 1;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门27
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen27";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 2;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门28
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen28";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 7;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 3;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门29
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen29";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 8;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 9;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 4;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门30
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen30";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 10;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 11;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 5;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门31
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen31";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 12;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 13;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 6;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门32
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen32";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 14;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 15;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 7;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 7;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门33
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen33";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 16;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 17;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 8;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 8;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门34
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen34";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 18;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 19;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 9;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 9;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门35
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen35";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 20;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 21;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 10;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 10;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门36
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen36";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 22;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 23;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 11;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 11;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门37
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen37";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 24;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 25;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 12;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 12;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门38
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen38";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 26;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 27;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 13;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 13;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门39
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen39";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 28;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 29;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 14;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 14;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门40
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen40";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 30;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 31;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 15;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 15;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门41
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen41";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 32;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 33;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 16;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 16;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门42
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen42";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 34;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 35;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 17;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 17;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门43
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen43";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 36;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 37;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 18;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 18;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门44
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen44";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 38;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 39;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 19;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 19;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门45
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen45";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 40;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 41;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 20;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 20;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门46
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen46";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 42;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 43;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 21;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 21;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        #endregion
        #region 屏蔽门47
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen47";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 44;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 45;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 22;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 22;
        #endregion
        #region 屏蔽门48
        pingBiMen = new PingBiMen();
        pingBiMen.m_name = "PingBiMen48";
        pointBindInfoList = new PointBindInfoList();
        pointBindInfoList.m_pointBindInfoList = new List<PointBindInfo>();
        pingBiMen.m_pointBindInfoList = pointBindInfoList;
        pingBiMenList.m_pingBiMenList.Add(pingBiMen);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 46;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "WaitTrain_Up";
        pointBindInfo.m_queueIndex = 47;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "DownTrain_Up";
        pointBindInfo.m_queueIndex = 23;
        pointBindInfoList.m_pointBindInfoList.Add(pointBindInfo);
        pointBindInfo = new PointBindInfo();
        pointBindInfo.m_name = "Train_Up_Birth";
        pointBindInfo.m_queueIndex = 23;
        #endregion
        #endregion
        #endregion 
        
        FileStream fileStream = new FileStream(AppConfigPath.StationDeviceAndPointXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(stationDeviceAndPointInfo.GetType());
        xml.Serialize(sw, stationDeviceAndPointInfo);
        sw.Close();
        fileStream.Close();
    }
    public static StationDeviceAndPointInfo DeserializationDeviceAndPointInfo4Xml()
    {
        FileStream fs = new FileStream(AppConfigPath.StationDeviceAndPointXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(StationDeviceAndPointInfo));
        StationDeviceAndPointInfo stationDeviceAndPointInfo = (StationDeviceAndPointInfo)xml.Deserialize(fs);
        fs.Close();
        return stationDeviceAndPointInfo;
    }
    #endregion  
}
