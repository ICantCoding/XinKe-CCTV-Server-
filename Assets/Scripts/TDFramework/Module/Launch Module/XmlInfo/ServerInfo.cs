using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using TDFramework;

public class ServerInfo
{
    [XmlElement("Id")]
    public System.UInt16 Id;
    [XmlElement("ServerName")]
    public string ServerName;
    [XmlElement("ServerPort")]
    public int ServerPort;

    #region 方法
    public static void SerializeServerInfo2Xml()
    {
        ServerInfo serverInfo = new ServerInfo();
        serverInfo.Id = 0; //服务器的Id为0, 用于Packet中的sendId或者nodeId
        serverInfo.ServerName = "CCTV服务器";
        serverInfo.ServerPort = 3322;

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
