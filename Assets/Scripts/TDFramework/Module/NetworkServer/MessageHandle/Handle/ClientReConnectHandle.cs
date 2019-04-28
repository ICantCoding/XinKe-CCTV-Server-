using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientReConnectHandle : BaseHandle
{
    #region 构造函数
    public ClientReConnectHandle(Agent agent, WorldActor worldActor, PlayerActor playerActor) :
        base(agent, worldActor, playerActor)
    {
        Name = "ClientReConnectHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        ClientReConnect clientReConnect = new ClientReConnect(packet.m_data);
        UInt16 u3dId = packet.m_sendId;
        TDFramework.StationModule module = (TDFramework.SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName)) as TDFramework.StationModule;
        if (module != null)
        {
            //重连同步屏蔽门数据给客户端
            module.SyncDeviceInfoByDeviceType(DeviceType.PingBiMen, m_playerActor);
            //重连同步车数据给客户端
            
            //重连同步预案数据给客户端

            //重连同步Npc数据给客户端
            module.SyncNpcInfo();
        }
    }
    #endregion
}
