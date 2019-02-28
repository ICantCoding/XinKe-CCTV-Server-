

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//PlayerActor跟Agent相互绑定在一起
public class PlayerActor : Actor
{
    #region 字段
    private uint m_agentId; //Agent的Id
    private Agent m_agent; //Agent
    private WorldActor m_worldActor; //世界Actor
    #endregion

    #region 构造函数
    public PlayerActor(uint id, MonoBehaviour mono) : base(mono)
    {
        m_agentId = id;
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
        }
        if (actorMsg.packet != null)
        {
            //处理ActorMessage中携带Packet内容的情况
            Packet packet = actorMsg.packet;
            count ++;
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

        if(firstId == 102 && secondId == 103) //一个客户端上线，并携带了客户端信息
        {
            SendNotification(EventID_Cmd.U3DClientOnLine, packet, null);
        }
    }
    #endregion
}


