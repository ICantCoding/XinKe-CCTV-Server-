using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;

public class BaseHandle
{
    #region 字段
    public string Name;
    protected Agent m_agent;
    protected WorldActor m_worldActor;
    protected PlayerActor m_playerActor;
    protected BaseClientNetworkEngine m_clientNetworkEngine;
    #endregion

    #region 构造函数
    public BaseHandle(Agent agent, WorldActor worldActor, PlayerActor playerActor)
    {
        m_agent = agent;
        m_worldActor = worldActor;
        m_playerActor = playerActor;
    }
    public BaseHandle(BaseClientNetworkEngine networkEngine)
    {
        m_clientNetworkEngine = networkEngine;
    }
    #endregion

    #region 虚方法
    public virtual void ReceivePacket(Packet packet)
    {

    }
    //广播给所有的U3D客户端
    public virtual void Broadcast2AllU3DPlayerActor(Packet packet)
    {
        NetworkModule module =  (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
        List<PlayerActor> u3dPlayerActorList = module.GetU3DPlayerActors();
        if(u3dPlayerActorList == null) return;
        foreach(var playerActor in u3dPlayerActorList)
        {
            playerActor.Agent.SendPacket(packet.Packet2Bytes());
        }
    }
    //转发给指定U3DID的客户端
    public virtual void Broadcast2U3DPlayerActor(UInt16 u3dId, Packet packet)
    {
        NetworkModule module =  SingletonMgr.ModuleMgr.NetworkModule;
        PlayerActor playerActor = module.GetPlayerActorByU3dId(u3dId);
        if(playerActor != null)
        {
            playerActor.Agent.SendPacket(packet.Packet2Bytes());
        }
    }
    #endregion
}
