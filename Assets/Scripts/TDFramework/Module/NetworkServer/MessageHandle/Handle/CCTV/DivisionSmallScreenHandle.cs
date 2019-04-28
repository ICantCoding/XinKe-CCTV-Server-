
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionSmallScreenHandle : BaseHandle
{
    #region 字段

    #endregion

    #region 构造函数
    public DivisionSmallScreenHandle(BaseClientNetworkEngine networkEngine) : base(networkEngine)
    {
        Name = "DivisionSmallScreenHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        System.UInt16 sendId = packet.m_sendId;
        System.UInt16 nodeId = packet.m_nodeId;
        // DivisionSmallScreen response = new DivisionSmallScreen(packet.m_data);
        // if (response == null) return;
        UnityEngine.Debug.Log("接收到DivisionSmallScreen消息.");
        //转发给对应的客户端
        Broadcast2U3DPlayerActor(nodeId, packet);
    }
    #endregion
}
