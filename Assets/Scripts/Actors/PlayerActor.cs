

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
        Debug.Log("PlayerActor Receive Message....");
        if (actorMsg == null) return;
        if (!string.IsNullOrEmpty(actorMsg.msg))
        {
            //处理ActorMessage中携带string内容的情况
            Debug.Log("PlayerActor 接收到了String形式的数据.");
        }
        if (actorMsg.packet != null)
        {
            //处理ActorMessage中携带Packet内容的情况
            Debug.Log("PlayerActor接收到了Packet形式的数据.");
            Packet packet = actorMsg.packet;
            Debug.Log("SendId: " + packet.m_sendId.ToString() + ", NodeId: " + packet.m_nodeId.ToString() + 
            ", FirstId: " + packet.m_firstId.ToString() + ", SecondId: " + packet.m_secondId.ToString() +
            ", MsgLen: " + packet.m_msgLen.ToString());
            count ++;
        }
        Debug.Log("Count: " + count);
    }
    #endregion
}


