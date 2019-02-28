
using System;
using System.Collections;
using System.Collections.Generic;

public class Packet
{
    public System.UInt16 m_sendId;
    public System.UInt16 m_nodeId;
    public System.UInt16 m_firstId;
    public System.UInt16 m_secondId;
    public System.UInt16 m_msgLen;
    public byte[] data;//服务器响应报文的具体数据内容

    public string protobufMessageClassName = string.Empty;   //用来记录当前Packet对应的Protobuf的Message类名字

    public Packet(UInt16 sendId, UInt16 nodeId, UInt16 firstId, UInt16 secondId, UInt16 msgLen, byte[] bytes)
    {
        this.m_sendId = sendId;
        this.m_nodeId = nodeId;
        this.m_firstId = firstId;
        this.m_secondId = secondId;
        this.m_msgLen = msgLen;
        this.data = bytes;
    }
    public Packet()
    {

    }
}
