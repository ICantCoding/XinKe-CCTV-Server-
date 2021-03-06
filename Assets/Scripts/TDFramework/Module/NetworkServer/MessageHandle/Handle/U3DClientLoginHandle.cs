﻿

using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;

public class U3DClientLoginHandle : BaseHandle
{
    #region 构造函数
    public U3DClientLoginHandle(Agent agent, WorldActor worldActor, PlayerActor playerActor) :
        base(agent, worldActor, playerActor)
    {
        Name = "U3DClientLoginHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
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
            // //默认登录的U3D客户端都属于站台0，CCTV视频监控都看向站台0的情况
            // m_playerActor.BelongStationIndex = 0; 
            // //默认登录的U3D客户端都属于站台0，CCTV视频监控都看向站台0的情况
            // m_playerActor.AddBelongStationIndex2List(0);
            //默认登录的U3D客户端都属于站台0，CCTV视频监控都看向站台0的情况, 优化版本
            TDFramework.SingletonMgr.ModuleMgr.StationModule.AddPlayerActor(5, m_playerActor);
            m_worldActor.AddPlayerActor2U3DDict(m_playerActor);
            // m_worldActor.AddU3DPlayerActor2BelongStationDict(m_playerActor);
            SendU3DClientLoginSuccessResponse();
            m_playerActor.SendNotification(EventID_Cmd.U3DClientOnLine, objs, null); //通知UI Command
        }
    }
    #endregion

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
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet responsePacket = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.U3DClientLoginMessageID, msgLen, bytes);
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
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet responsePacket = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.U3DClientLoginMessageID, msgLen, bytes);
        m_agent.SendPacket(responsePacket.Packet2Bytes()); //返回登录失败，已经有对应的U3DID客户端登录.
    }
}
