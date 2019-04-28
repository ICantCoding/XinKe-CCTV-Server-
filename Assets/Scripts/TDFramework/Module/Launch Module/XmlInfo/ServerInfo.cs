using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using TDFramework;

public class ServerInfo
{
    [XmlElement("Id")]
    public System.UInt16 Id;            //U3D服务器Id唯一标识 为 0
    [XmlElement("ServerName")]
    public string ServerName;           //U3D服务器名称
    [XmlElement("ServerIpAddress")]
    public string ServerIpAddress;      //U3D服务器Ip地址
    [XmlElement("ServerPort")]
    public int ServerPort;              //U3D服务器端口号
    [XmlElement("AtsServerIpAddress")]
    public string AtsServerIpAddress;   //ATS服务器Ip地址
    [XmlElement("AtsServerPort")]
    public int AtsServerPort;           //ATS服务器端口号
    [XmlElement("CctvServerIpAddress")]
    public string CctvServerIpAddress;  //CCTV服务器Ip地址
    [XmlElement("CctvServerPort")]
    public int CctvServerPort;          //CCTV服务器端口号

    #region 方法
    public static void SerializeServerInfo2Xml()
    {
        ServerInfo serverInfo = new ServerInfo();
        serverInfo.Id = 0; //服务器的Id为0, 用于Packet中的sendId或者nodeId
        serverInfo.ServerName = "CCTV服务器";
        serverInfo.ServerIpAddress = Util.GetIpAddress();
        serverInfo.ServerPort = 3322;
        serverInfo.AtsServerIpAddress = "192.168.0.51";
        serverInfo.AtsServerPort = 40000;
        serverInfo.CctvServerIpAddress = "192.168.0.51";
        serverInfo.CctvServerPort = 30000;

        if (File.Exists(AppConfigPath.ServerInfoXmlPath))
        {
            File.Delete(AppConfigPath.ServerInfoXmlPath);
        }

        FileStream fileStream = new FileStream(AppConfigPath.ServerInfoXmlPath,
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(serverInfo.GetType());
        xml.Serialize(sw, serverInfo);
        sw.Close();
        fileStream.Close();
    }
    public static ServerInfo DeserializeServerInfoFromXml()
    {
        FileStream fs = new FileStream(AppConfigPath.ServerInfoXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xml = new XmlSerializer(typeof(ServerInfo));
        ServerInfo serverInfo = (ServerInfo)xml.Deserialize(fs);
        fs.Close();
        return serverInfo;
    }
    #endregion

}
