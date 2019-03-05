

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//PlayerActor跟Agent相互绑定在一起
public class PlayerActor : Actor
{

    #region 常量

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
            ReceiveU3DClientLoginRequest(packet);
        }
        else
        {

        }
    }
    #endregion

    #region Send网络消息方法
    private void SendU3DClientLoginSuccessResponse()
    {
        U3DClientLoginResponse u3dClientLoginResponse = new U3DClientLoginResponse()
        {
            m_resultId = ResultID.Success_ResultId,
            m_msg = ResultReason.Success_ResultReason
        };
        byte[] bytes = u3dClientLoginResponse.Packet2Bytes();
        UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
        UInt16 u3dId = U3DId;
        UInt16 firstId = 0;
        UInt16 secondId = 0;
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet responsePacket = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
        m_agent.SendPacket(responsePacket.Packet2Bytes()); //返回U3D客户端登录成功.
    }
    private void SendU3DClientLoginFailResponse()
    {
        U3DClientLoginResponse u3dClientLoginResponse = new U3DClientLoginResponse()
        {
            m_resultId = ResultID.U3DClientOnLineFail_ResultId,
            m_msg = ResultReason.U3DClientOnLineFail_ResultReason
        };
        byte[] bytes = u3dClientLoginResponse.Packet2Bytes();
        UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
        UInt16 u3dId = U3DId;
        UInt16 firstId = 0;
        UInt16 secondId = 0;
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet responsePacket = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
        m_agent.SendPacket(responsePacket.Packet2Bytes()); //返回登录失败，已经有对应的U3DID客户端登录.
    }
    #endregion

    #region Receive网络消息处理
    private void ReceiveU3DClientLoginRequest(Packet packet)
    {
        if(packet == null) return;
        U3DClientLogin u3dClientLogin = new U3DClientLogin(packet.m_data);
        U3DId = u3dClientLogin.m_clientId;
        System.Net.IPEndPoint ipEndPoint = (System.Net.IPEndPoint)m_agent.EndPoint;
        object[] objs = new object[2];
        objs[0] = u3dClientLogin;
        objs[1] = ipEndPoint;
        if (m_worldActor.PlayerActorIsExitsByU3dId(U3DId))
        {
            //表示同一个U3DID的客户端登录到服务器，返回登录失败
            SendU3DClientLoginFailResponse();
        }
        else
        {
            m_worldActor.AddPlayerActor2Dict(this); //添加到WorldActor中被Dict管理
            SendU3DClientLoginSuccessResponse();
            SendNotification(EventID_Cmd.U3DClientOnLine, objs, null);
        }
    }
    #endregion
}


