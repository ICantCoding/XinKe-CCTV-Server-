

using System;
using System.Collections;
using System.Collections.Generic;

public class U3DClientLoginHandle
{
    #region 字段
    private Agent m_agent;
    private WorldActor m_worldActor;
    private PlayerActor m_playerActor;
    #endregion

    #region 构造函数
    public U3DClientLoginHandle(Agent agent, WorldActor worldActor, PlayerActor playerActor)
    {
        m_agent = agent;
        m_worldActor = worldActor;
        m_playerActor = playerActor;
    }
    #endregion

    #region 方法
    public void ReceiveU3DClientLoginRequest(Packet packet)
    {
        if(packet == null) return;
        U3DClientLogin u3dClientLogin = new U3DClientLogin(packet.m_data);
        System.Net.IPEndPoint ipEndPoint = (System.Net.IPEndPoint)m_agent.EndPoint;
        object[] objs = new object[2];
        objs[0] = u3dClientLogin;
        objs[1] = ipEndPoint;
        if (m_worldActor.PlayerActorIsExitsByU3dId(u3dClientLogin.m_clientId))
        {
            //表示同一个U3DID的客户端登录到服务器，返回登录失败
            SendU3DClientLoginFailResponse();
        }
        else
        {
            m_playerActor.PlayerActorType = PlayerActorType.U3DPlayerActorType;
            m_playerActor.U3DId = u3dClientLogin.m_clientId;
            m_worldActor.AddPlayerActor2U3DDict(m_playerActor);
            SendU3DClientLoginSuccessResponse();
            m_playerActor.SendNotification(EventID_Cmd.U3DClientOnLine, objs, null);
        }
    }
    private void SendU3DClientLoginSuccessResponse()
    {
        U3DClientLoginResponse u3dClientLoginResponse = new U3DClientLoginResponse()
        {
            m_resultId = ResultID.Success_ResultId,
            m_msg = ResultReason.Success_ResultReason
        };
        byte[] bytes = u3dClientLoginResponse.Packet2Bytes();
        UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
        UInt16 u3dId = m_playerActor.U3DId;
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
        UInt16 u3dId = m_playerActor.U3DId;
        UInt16 firstId = 0;
        UInt16 secondId = 0;
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet responsePacket = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
        m_agent.SendPacket(responsePacket.Packet2Bytes()); //返回登录失败，已经有对应的U3DID客户端登录.
    }
    #endregion
}
