using System;
using System.Collections;
using System.Collections.Generic;

public class StationClientLoginHandle
{
    #region 字段
    private Agent m_agent;
    private WorldActor m_worldActor;
    private PlayerActor m_playerActor;
    #endregion

    #region 构造函数
    public StationClientLoginHandle(Agent agent, WorldActor worldActor, PlayerActor playerActor)
    {
        m_agent = agent;
        m_worldActor = worldActor;
        m_playerActor = playerActor;
    }
    #endregion

    #region 方法
    public void ReceiveStationLoginRequest(Packet packet)
    {
        if (packet == null) return;
        StationClientLogin stationClientLogin = new StationClientLogin(packet.m_data);
        UInt16 stationIndex = stationClientLogin.m_stationIndex;
        UInt16 stationSocketType = stationClientLogin.m_stationSocketType;
        m_playerActor.PlayerActorType = PlayerActorType.StationPlayerActorType;
        m_playerActor.StationIndex = stationIndex;
        m_playerActor.StationSocketType = stationSocketType;
        m_worldActor.AddPlayerActor2StationDict(stationIndex, stationSocketType, m_playerActor);
        SendStationClientLoginSuccessResponse();
        m_playerActor.SendNotification(EventID_Cmd.StationClientOnLine, null, null);
    }
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
    #endregion
}
