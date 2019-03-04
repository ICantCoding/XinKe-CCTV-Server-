

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//PlayerActor跟Agent相互绑定在一起
public class PlayerActor : Actor
{

    #region 常量
    private const string StopSelfActorStr = "StopSelfActor";
    private const char SplitChar = '|';
    #endregion

    #region 字段
    private UInt16 m_u3dId; //U3D客户端ID唯一标识
    private uint m_agentId; //Agent的Id
    private Agent m_agent; //Agent
    private WorldActor m_worldActor; //世界Actor
    #endregion

    #region 属性
    public UInt16 U3DId
    {
        get { return m_u3dId; }
        set
        {
            m_u3dId = value;
        }
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
    private static int count = 0;
    protected override void ReceiveMsg(ActorMessage actorMsg)
    {
        if (actorMsg == null) return;
        if (!string.IsNullOrEmpty(actorMsg.msg))
        {
            //处理ActorMessage中携带string内容的情况
            // var cmds = actorMsg.msg.Split(SplitChar);
            // if (cmds[0] == StopSelfActorStr)
            // {
            //     m_isStop = true;
            // }
        }
        if (actorMsg.packet != null)
        {
            //处理ActorMessage中携带Packet内容的情况
            Packet packet = actorMsg.packet;
            count++;
            HandlePacket(packet);
        }
    }
    #endregion

    #region 接收到客户端消息处理
    private void HandlePacket(Packet packet)
    {
        UInt16 sendId = packet.m_sendId;
        UInt16 nodeId = packet.m_nodeId;
        UInt16 firstId = packet.m_firstId;
        UInt16 secondId = packet.m_secondId;
        UInt16 msgLen = packet.m_msgLen;
        byte[] data = packet.m_data;

        if (firstId == 0 && secondId == 0) //一个客户端上线，并携带了客户端信息
        {
            U3DClientLogin u3dClientLogin = new U3DClientLogin(packet.m_data);
            U3DId = u3dClientLogin.m_clientId;
            System.Net.EndPoint endPoint = m_agent.EndPoint;
            System.Net.IPEndPoint ipEndPoint = (System.Net.IPEndPoint)endPoint;
            object[] objs = new object[2];
            objs[0] = u3dClientLogin;
            objs[1] = ipEndPoint;
            SendNotification(EventID_Cmd.U3DClientOnLine, objs, null);
        }
    }
    #endregion
}


