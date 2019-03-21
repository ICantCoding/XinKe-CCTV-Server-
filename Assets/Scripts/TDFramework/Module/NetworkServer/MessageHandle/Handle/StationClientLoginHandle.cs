using System;
using System.Collections;
using System.Collections.Generic;

public class StationClientLoginHandle : BaseHandle
{
    #region 构造函数
    public StationClientLoginHandle(Agent agent, WorldActor worldActor, PlayerActor playerActor) : 
        base(agent, worldActor, playerActor)
    {
        Name = "StationClientLoginHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket (Packet packet)
    {
        if (packet == null) return;
        StationClientLogin stationClientLogin = new StationClientLogin(packet.m_data);
        UInt16 stationIndex = stationClientLogin.m_stationIndex;
        UInt16 stationClientType = stationClientLogin.m_stationClientType;
        m_playerActor.PlayerActorType = PlayerActorType.StationPlayerActorType;
        m_playerActor.StationIndex = stationIndex;
        m_playerActor.StationClientType = stationClientType;
        m_worldActor.AddPlayerActor2StationDict(stationIndex, stationClientType, m_playerActor);
        SendStationClientLoginSuccessResponse();
        m_playerActor.SendNotification(EventID_Cmd.StationClientOnLine, null, null);
    }
    #endregion

    private void SendStationClientLoginSuccessResponse()
    {
        StationClientLoginResponse response = new StationClientLoginResponse()
        {
            m_resultId = ResultID.Success_ResultId,
            m_msg = ResultReason.Success_ResultReason
        };
        byte[] bytes = response.Packet2Bytes();
        UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
        UInt16 u3dId = m_playerActor.U3DId;
        UInt16 firstId = 0;
        UInt16 secondId = 1;
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet responsePacket = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
        m_agent.SendPacket(responsePacket.Packet2Bytes()); //返回U3D客户端登录成功.
    }
}
