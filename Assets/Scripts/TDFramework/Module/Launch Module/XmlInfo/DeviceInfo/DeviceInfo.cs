using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TDFramework;

public class DeviceInfo
{
    [XmlElement("Name")]
    public string Name;
    [XmlElement("StationIndex")]
    public UInt16 StationIndex;
    [XmlElement("DeviceId")]
    public int DeviceId;
    [XmlElement("DeviceType")]
    public int DeviceType;
}




