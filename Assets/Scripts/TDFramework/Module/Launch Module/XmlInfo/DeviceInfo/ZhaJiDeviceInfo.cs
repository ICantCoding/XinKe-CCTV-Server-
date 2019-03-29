using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TDFramework;


public class ZhaJiDeviceInfo : DeviceInfo
{
    [XmlElement("LeftOpenAngleX")]
    public float LeftOpenAngleX;
    [XmlElement("LeftOpenAngleY")]
    public float LeftOpenAngleY;
    [XmlElement("LeftOpenAngleZ")]
    public float LeftOpenAngleZ;
    [XmlElement("RightOpenAngleX")]
    public float RightOpenAngleX;
    [XmlElement("RightOpenAngleY")]
    public float RightOpenAngleY;
    [XmlElement("RightOpenAngleZ")]
    public float RightOpenAngleZ;
}

public class ZhaJiDeviceInfoCollection
{
    [XmlElement("ZhaJiDeviceInfo")]
    public List<ZhaJiDeviceInfo> ZhaJiDeviceInfoList = new List<ZhaJiDeviceInfo>();

    #region 方法
    public void AddZhaJiDeviceInfo(ZhaJiDeviceInfo deviceInfo)
    {
        if (deviceInfo == null) return;
        ZhaJiDeviceInfoList.Add(deviceInfo);
    }
    public DeviceInfo GetZhaJiDeviceInfo(int deviceId)
    {
        for(int i = 0; i < ZhaJiDeviceInfoList.Count; i++)
        {
            if(ZhaJiDeviceInfoList[i].DeviceId == deviceId)
            {
                return ZhaJiDeviceInfoList[i];
            }
        }
        return null;
    }
    #endregion

    #region 序列化和反序列化
    private const int station0_count = 14;
    private const int station1_count = 14;
    //序列化站台0闸机设备
    public static void SerializedZhaJiDeviceInfoCollectionAtStation0()
    {
        float[,] angles = new float[station0_count, 6]
        {
            {-23.321f, 0.0f, 0.0f, 23.321f, 0.0f, 0.0f},
            {-21.837f, 0.0f, 0.0f, 21.837f, 0.0f, 0.0f},
            {-21.837f, 0.0f, 0.0f, 21.837f, 0.0f, 0.0f},
            {-21.837f, 0.0f, 0.0f, 21.837f, 0.0f, 0.0f},
            {-21.837f, 0.0f, 0.0f, 21.837f, 0.0f, 0.0f},
            {-21.837f, 0.0f, 0.0f, 21.837f, 0.0f, 0.0f},
            {-21.837f, 0.0f, 0.0f, 21.837f, 0.0f, 0.0f},
            {45.163f, 0.0f, 0.0f, -45.163f, 0.0f, 0.0f},
            {23.916f, 0.0f, 0.0f, -23.916f, 0.0f, 0.0f},
            {23.916f, 0.0f, 0.0f, -23.916f, 0.0f, 0.0f},
            {23.916f, 0.0f, 0.0f, -23.916f, 0.0f, 0.0f},
            {23.916f, 0.0f, 0.0f, -23.916f, 0.0f, 0.0f},
            {23.916f, 0.0f, 0.0f, -23.916f, 0.0f, 0.0f},
            {23.916f, 0.0f, 0.0f, -23.916f, 0.0f, 0.0f}
        };
        SerializedZhaJiDeviceInfoCollection(0, DeviceType.ZhaJi, station0_count, angles, AppConfigPath.Station0ZhaJiXmlPath);
    }
    //反序列化站台0闸机设备
    public static ZhaJiDeviceInfoCollection DeserializedZhaJiDeviceInfoCollectionAtStation0()
    {
        return DeserializedZhaJiDeviceInfoCollection(AppConfigPath.Station0ZhaJiXmlPath);
    }
    //序列化站台1闸机设备
    public static void SerializedZhaJiDeviceInfoCollectionAtStation1()
    {

    }
    //反序列化站台1闸机设备
    public static ZhaJiDeviceInfoCollection DeserializedZhaJiDeviceInfoCollectionAtStation1()
    {
        return null;
    }









































    //序列化
    public static void SerializedZhaJiDeviceInfoCollection(UInt16 stationIndex, DeviceType deviceType, int count, float[,] angles, string filePath)
    {
        ZhaJiDeviceInfoCollection collection = new ZhaJiDeviceInfoCollection();
        for (int i = 1; i <= angles.GetLength(0); ++i)
        {
            //闸机
            ZhaJiDeviceInfo zhaJiDeviceInfo = new ZhaJiDeviceInfo();
            zhaJiDeviceInfo.Name = "闸机";
            zhaJiDeviceInfo.StationIndex = stationIndex;
            zhaJiDeviceInfo.DeviceId = (int)deviceType + i;
            zhaJiDeviceInfo.DeviceType = (int)deviceType;
            zhaJiDeviceInfo.LeftOpenAngleX = angles[i-1, 0];
            zhaJiDeviceInfo.LeftOpenAngleY = angles[i-1, 1];
            zhaJiDeviceInfo.LeftOpenAngleZ = angles[i-1, 2];
            zhaJiDeviceInfo.RightOpenAngleX = angles[i-1,3];
            zhaJiDeviceInfo.RightOpenAngleY = angles[i-1,4];
            zhaJiDeviceInfo.RightOpenAngleZ = angles[i-1,5];
            collection.AddZhaJiDeviceInfo(zhaJiDeviceInfo);
        }
        FileStream fileStream = new FileStream(filePath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(collection.GetType());
        xml.Serialize(sw, collection);
        sw.Close();
        fileStream.Close();
    }
    //反序列化
    public static ZhaJiDeviceInfoCollection DeserializedZhaJiDeviceInfoCollection(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(ZhaJiDeviceInfoCollection));
        ZhaJiDeviceInfoCollection collection = (ZhaJiDeviceInfoCollection)xml.Deserialize(fs);
        fs.Close();
        return collection;
    }
    #endregion
}