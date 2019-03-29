using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TDFramework;

public class PingBiMenDeviceInfo : DeviceInfo
{
    [XmlElement("LeftOpenMoveX")]
    public float LeftOpenMoveX;
    [XmlElement("LeftOpenMoveY")]
    public float LeftOpenMoveY;
    [XmlElement("LeftOpenMoveZ")]
    public float LeftOpenMoveZ;
    [XmlElement("RightOpenMoveX")]
    public float RightOpenMoveX;
    [XmlElement("RightOpenMoveY")]
    public float RightOpenMoveY;
    [XmlElement("RightOpenMoveZ")]
    public float RightOpenMoveZ;
}

public class PingBiMenDeviceInfoCollection
{
    [XmlElement("PingBiMenDeviceInfo")]
    public List<PingBiMenDeviceInfo> PingBiMenDeviceInfoList = new List<PingBiMenDeviceInfo>();

    #region 方法
    public void AddPingBiMenDeviceInfo(PingBiMenDeviceInfo deviceInfo)
    {
        if (deviceInfo == null) return;
        PingBiMenDeviceInfoList.Add(deviceInfo);
    }
    public DeviceInfo GetPingBiMenDeviceInfo(int deviceId)
    {
        for (int i = 0; i < PingBiMenDeviceInfoList.Count; i++)
        {
            if (PingBiMenDeviceInfoList[i].DeviceId == deviceId)
            {
                return PingBiMenDeviceInfoList[i];
            }
        }
        return null;
    }
    #endregion

    #region 序列化和反序列化
    private const int station0_count = 48;
    private const int station1_count = 14;
    //序列化站台0闸机设备
    public static void SerializedPingBiMenDeviceInfoCollectionAtStation0()
    {
        float[,] moves = new float[station0_count, 6]
        {
            {0.8887167f, 0.0f, 0.0f, -0.66024f, 0.0f, 0.0f},

            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},
            {0.8887167f, 0.0f, 0.0f, -0.917167f, 0.0f, 0.0f},

            {-0.6952439f, 0.0f, 0.0f, 0.908717f, 0.0f, 0.0f},

            {-0.6952439f, 0.0f, 0.0f, 0.908717f, 0.0f, 0.0f},

            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},
            {-0.8887167f, 0.0f, 0.0f, 0.917167f, 0.0f, 0.0f},

            {0.8887167f, 0.0f, 0.0f, -0.66024f, 0.0f, 0.0f},
        };
        SerializedPingBiMenDeviceInfoCollection(0, DeviceType.PingBiMen, station0_count, moves, AppConfigPath.Station0PingBiMenXmlPath);
    }
    //反序列化站台0闸机设备
    public static PingBiMenDeviceInfoCollection DeserializedPingBiMenDeviceInfoCollectionAtStation0()
    {
        return DeserializedPingBiMenDeviceInfoCollection(AppConfigPath.Station0PingBiMenXmlPath);
    }
    //序列化站台1闸机设备
    public static void SerializedZhaJiDeviceInfoCollectionAtStation1()
    {

    }
    //反序列化站台1闸机设备
    public static PingBiMenDeviceInfoCollection DeserializedZhaJiDeviceInfoCollectionAtStation1()
    {
        return null;
    }
















    //序列化
    public static void SerializedPingBiMenDeviceInfoCollection(UInt16 stationIndex, DeviceType deviceType, int count, float[,] moves, string filePath)
    {
        PingBiMenDeviceInfoCollection collection = new PingBiMenDeviceInfoCollection();
        for (int i = 1; i <= moves.GetLength(0); ++i)
        {
            //闸机
            PingBiMenDeviceInfo deviceInfo = new PingBiMenDeviceInfo();
            deviceInfo.Name = "屏蔽门";
            deviceInfo.StationIndex = stationIndex;
            deviceInfo.DeviceId = (int)deviceType + i;
            deviceInfo.DeviceType = (int)deviceType;
            deviceInfo.LeftOpenMoveX = moves[i - 1, 0];
            deviceInfo.LeftOpenMoveY = moves[i - 1, 1];
            deviceInfo.LeftOpenMoveZ = moves[i - 1, 2];
            deviceInfo.RightOpenMoveX = moves[i - 1, 3];
            deviceInfo.RightOpenMoveY = moves[i - 1, 4];
            deviceInfo.RightOpenMoveZ = moves[i - 1, 5];
            collection.AddPingBiMenDeviceInfo(deviceInfo);
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
    public static PingBiMenDeviceInfoCollection DeserializedPingBiMenDeviceInfoCollection(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(PingBiMenDeviceInfoCollection));
        PingBiMenDeviceInfoCollection collection = (PingBiMenDeviceInfoCollection)xml.Deserialize(fs);
        fs.Close();
        return collection;
    }
    #endregion
}