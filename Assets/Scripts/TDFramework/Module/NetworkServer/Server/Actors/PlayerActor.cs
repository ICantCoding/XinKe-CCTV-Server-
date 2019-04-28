

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TDFramework;

public enum PlayerActorType
{
    U3DPlayerActorType,
    StationPlayerActorType,
}

//PlayerActor跟Agent相互绑定在一起
public class PlayerActor : Actor
{
    #region 字段
    //PlayerActor类型, 表明当前PlayerActor是U3D还是Station
    private PlayerActorType m_playerActorType;

    //如果PlayerActor是StationPlayerActorType, 该字段有意义，表示StationPlayerActor属于哪一个U3DPlayerActor
    private UInt16 m_belongU3DId; //指明StationPlayerActor属于哪一个U3DPlayerACtor关联
    //如果PlayerActor是StationPlayerActorType, 该字段有意义，表示PlayerActor所在站台
    private UInt16 m_stationIndex;
    //如果PlayerActor是StationPlayerActorType, 该字段有意义, 表示站台socket客户端连接类型， 上行，下行......        
    private UInt16 m_stationClientType;

    
    //U3D客户端ID唯一标识
    private UInt16 m_u3dId;
    //Agent的Id
    private uint m_agentId;
    //Agent
    private Agent m_agent;
    //世界Actor
    private WorldActor m_worldActor;
    #endregion

    #region 属性
    public PlayerActorType PlayerActorType
    {
        get { return m_playerActorType; }
        set { m_playerActorType = value; }
    }
    //StationPlayerActor有关
    public UInt16 BelongU3DId
    {
        get { return m_belongU3DId; }
        set { m_belongU3DId = value; }
    }
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    public UInt16 StationClientType
    {
        get { return m_stationClientType; }
        set { m_stationClientType = value; }
    }
    //U3DPlayerActor有关
    public UInt16 U3DId
    {
        get { return m_u3dId; }
        set { m_u3dId = value; }
    }
    public uint AgentId
    {
        get { return m_agentId; }
        set { m_agentId = value; }
    }
    public Agent Agent
    {
        get { return m_agent; }
    }
    #endregion

    #region 构造函数
    public PlayerActor(uint agentId, MonoBehaviour mono) : base(mono)
    {
        AgentId = agentId;
        ServerActor server = ActorManager.Instance.GetActor<ServerActor>();
        m_agent = server.GetAgent(m_agentId);
        m_agent.Actor = this;
        m_worldActor = ActorManager.Instance.GetActor<WorldActor>();
    }
    #endregion

    #region 重载方法
    protected override void ReceiveMsg(ActorMessage actorMsg)
    {
        if (actorMsg == null) return;
        if (!string.IsNullOrEmpty(actorMsg.msg))
        {
            HandleMsgStr(actorMsg.msg);
        }
        if (actorMsg.packet != null)
        {
            //处理ActorMessage中携带Packet内容的情况
            Packet packet = actorMsg.packet;
            HandlePacket(packet);
        }
    }
    #endregion

    #region 接收到客户端消息处理
    private void HandleMsgStr(string msg)
    {

    }
    private void HandlePacket(Packet packet)
    {
        UInt16 firstId = packet.m_firstId;
        UInt16 secondId = packet.m_secondId;
        BaseHandle handle = SingletonMgr.NetworkMsgHandleFuncMap.GetHandleInstantiateObj(firstId, secondId, m_agent, m_worldActor, this);
        if (handle != null)
        {
            handle.ReceivePacket(packet);
        }
    }
    #endregion
}


