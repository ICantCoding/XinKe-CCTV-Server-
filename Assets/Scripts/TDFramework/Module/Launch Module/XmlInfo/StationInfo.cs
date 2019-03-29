using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TDFramework;

public class StationInfo
{
    [XmlElement("Index")]
    public UInt16 Index;
    [XmlElement("Name")]
    public string Name;
    [XmlElement("EngName")]
    public string EngName;
    //进站购票位置点个数
    [XmlElement("BuyTicketPointCount")]
    public int BuyTicketPointCount;
    //进站购票休息区位置点个数
    [XmlElement("BuyTicketRestAreaPointCount")]
    public int BuyTicketRestAreaPointCount;
    //进站检票位置点个数
    [XmlElement("EnterCheckTicketPointCount")]
    public int EnterCheckTicketPointCount;
    //进站检票休息区位置点个数
    [XmlElement("EnterCheckTicketRestAreaPointCount")]
    public int EnterCheckTicketRestAreaPointCount;
    //进站上行排队位置点个数
    [XmlElement("WaitTrainUpPointCount")]
    public int WaitTrainUpPointCount;
    //进站下行排队位置点个数
    [XmlElement("WaitTrainDownPointCount")]
    public int WaitTrainDownPointCount;
    //出站上行排队位置点个数
    [XmlElement("DownTrainUpPointCount")]
    public int DownTrainUpPointCount;
    //出站下行排队位置点个数
    [XmlElement("DownTrainDownPointCount")]
    public int DownTrainDownPointCount;
    //出站检票位置点个数
    [XmlElement("ExitCheckTicketPointCount")]
    public int ExitCheckTicketPointCount;
    //出站检票休息区位置点个数
    [XmlElement("ExitCheckTicketRestAreaPointCount")]
    public int ExitCheckTicketRestAreaPointCount;
    //进站上行最大Npc个数
    [XmlElement("EnterStationUpMaxNpcCount")]
    public int EnterStationUpMaxNpcCount;
    //进站下行最大Npc个数
    [XmlElement("EnterStationDownMaxNpcCount")]
    public int EnterStationDownMaxNpcCount;
    //出站上行最大Npc个数
    [XmlElement("ExitStationUpMaxNpcCount")]
    public int ExitStationUpMaxNpcCount;
    //出站下行最大Npc个数
    [XmlElement("ExitStationDownMaxNpcCount")]
    public int ExitStationDownMaxNpcCount;
    //生成Npc时间间隔
    [XmlElement("GenerateNpcIntervalTime")]
    public float GenerateNpcIntervalTime;
}

public class StationInfoList
{
    [XmlElement("StationInfo")]
    public List<StationInfo> stationInfoList = new List<StationInfo>();

    
    public int Count
    {
        get { return stationInfoList.Count; }
    }

    #region 方法
    public StationInfo GetStationInfo(UInt16 index)
    {
        int count = stationInfoList.Count;
        for (int i = 0; i < count; ++i)
        {
            if (stationInfoList[i].Index == index)
            {
                return stationInfoList[i];
            }
        }
        return null;
    }
    public StationInfo GetStationInfo(string name)
    {
        int count = stationInfoList.Count;
        for (int i = 0; i < count; ++i)
        {
            if (stationInfoList[i].Name == name || stationInfoList[i].EngName == name)
            {
                return stationInfoList[i];
            }
        }
        return null;
    }
    #endregion

    #region 序列化，反序列化方法
    public static void SerializeStationInfoList2Xml()
    {
        StationInfoList list = new StationInfoList();

        StationInfo station = new StationInfo();
        station.Index = 0;
        station.Name = "赵营";
        station.EngName = "ZhaoYing";
        station.BuyTicketPointCount = 48;
        station.BuyTicketRestAreaPointCount = 8;
        station.EnterCheckTicketPointCount = 35;
        station.EnterCheckTicketRestAreaPointCount = 8;
        station.WaitTrainUpPointCount = 144;
        station.WaitTrainDownPointCount = 144;
        station.DownTrainUpPointCount = 72;
        station.DownTrainDownPointCount = 72;
        station.ExitCheckTicketPointCount = 35;
        station.ExitCheckTicketRestAreaPointCount = 8;
        station.EnterStationUpMaxNpcCount = 15;
        station.EnterStationDownMaxNpcCount = 15;
        station.ExitStationUpMaxNpcCount = 15;
        station.ExitStationDownMaxNpcCount = 15;
        station.GenerateNpcIntervalTime = 2.0f;

        list.stationInfoList.Add(station);

        if (File.Exists(AppConfigPath.StationInfoXmlPath))
        {
            File.Delete(AppConfigPath.StationInfoXmlPath);
        }

        FileStream fileStream = new FileStream(AppConfigPath.StationInfoXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(list.GetType());
        xml.Serialize(sw, list);
        sw.Close();
        fileStream.Close();
    }
    public static StationInfoList DeserializeStationInfoListFromXml()
    {
        FileStream fs = new FileStream(AppConfigPath.StationInfoXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(StationInfoList));
        StationInfoList stationInfoList = (StationInfoList)xml.Deserialize(fs);
        fs.Close();
        return stationInfoList;
    }
    #endregion
}


