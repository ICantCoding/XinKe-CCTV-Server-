

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum PlayerActorType
{
    U3DPlayerActorType,
    StationPlayerActorType,
}

//PlayerActor跟Agent相互绑定在一起
public class PlayerActor : Actor
{
    #region 字段
    private PlayerActorType m_playerActorType; //PlayerActor类型
    private UInt16 m_stationIndex;          //如果PlayerActor是StationPlayerActorType, 该字段有意义, 表示站台索引
    private UInt16 m_stationSocketType;     //如果PlayerActor是StationPlayerActorType, 该字段有意义, 表示站台socket连接类型

    private UInt16 m_u3dId; //U3D客户端ID唯一标识
    private uint m_agentId; //Agent的Id
    private Agent m_agent; //Agent
    private WorldActor m_worldActor; //世界Actor
    #endregion

    #region 属性
    public PlayerActorType PlayerActorType
    {
        get { return m_playerActorType; }
        set { m_playerActorType = value; }
    }
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    public UInt16 StationSocketType
    {
        get { return m_stationSocketType; }
        set { m_stationSocketType = value; }
    }
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
    private void HandlePacket(Packet packet)
    {
        UInt16 firstId = packet.m_firstId;
        UInt16 secondId = packet.m_secondId;
        if (firstId == 0 && secondId == 0)
        {
            //一个客户端上线，并携带了客户端信息
            Debug.Log("U3D客户端上线!");
            U3DClientLoginHandle handle = new U3DClientLoginHandle(m_agent, m_worldActor, this);
            handle.ReceiveU3DClientLoginRequest(packet);
        }
        else if (firstId == 0 && secondId == 1)
        {
            //一个Station连接上线，并携带了Station信息
            StationClientLoginHandle handle = new StationClientLoginHandle(m_agent, m_worldActor, this);
            handle.ReceiveStationLoginRequest(packet);
        }
    }
    #endregion
}


