using System;
using System.Collections;
using System.Collections.Generic;

public class TrainDoorCtrlHandle : BaseHandle
{
     #region 构造函数
    public TrainDoorCtrlHandle(BaseClientNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "TrainDoorCtrlHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        UnityEngine.Debug.Log("接收到TrainDoorCtrl消息.");
        //转发给所有的客户端
        Broadcast2AllU3DPlayerActor(packet);
    }
    #endregion
}
