using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBindCameraHandle : BaseHandle {
    #region 字段

    #endregion

    #region 构造函数
    public ScreenBindCameraHandle (BaseClientNetworkEngine networkEngine) : base (networkEngine) {
        Name = "ScreenBindCameraHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket (Packet packet) 
    {
        if (packet == null) return;
        System.UInt16 sendId = packet.m_sendId;
        System.UInt16 nodeId = packet.m_nodeId;
        // ScreenBindCamera response = new ScreenBindCamera (packet.m_data);
        // if (response == null) return;
        UnityEngine.Debug.Log("接收到ScreenBindCamera消息.");
        //转发给对应的客户端
        Broadcast2U3DPlayerActor(nodeId, packet);
    }
    #endregion
}