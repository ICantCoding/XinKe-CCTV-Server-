using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NpcSync : MonoBehaviour
{
    #region 字段
    private NetworkModule m_networkModule = null;
    private StationModule m_stationModule = null;
    #endregion

    #region Unity生命周期
    void Start()
    {
        m_networkModule = TDFramework.SingletonMgr.ModuleMgr.NetworkModule;
        m_stationModule = TDFramework.SingletonMgr.ModuleMgr.StationModule;
    }
    #endregion

    #region 同步Npc信息
    //发送Npc位置信息
    public void SendNpcPosition(float posX, float posY, float posZ,
        float angleX, float angleY, float angleZ,
        int npcId, int npcType, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_stationModule == null || m_networkModule == null) return;
        List<PlayerActor> u3dPlayerActorList = null;
        m_stationModule.GetU3DPlayerActors(stationIndex, ref u3dPlayerActorList);
        if (u3dPlayerActorList == null || u3dPlayerActorList.Count == 0) return;
        PlayerActor stationPlayerActor = null;
        for (int k = 0; k < u3dPlayerActorList.Count; k++)
        {
            stationPlayerActor = m_networkModule.GetStationPlayerActorAboutU3DPlayerActor(u3dPlayerActorList[k],
                stationIndex, stationClientType);
            if (stationPlayerActor != null)
            {
                Agent agent = stationPlayerActor.Agent;
                NpcPosition npcPos = new NpcPosition()
                {
                    m_posX = posX,
                    m_posY = posY,
                    m_posZ = posZ,
                    m_angleX = angleX,
                    m_angleY = angleY,
                    m_angleZ = angleZ,
                    m_npcId = npcId,
                    m_npcType = npcType,
                    m_stationIndex = stationIndex,
                    m_stationClientType = stationClientType,
                };
                byte[] bytes = npcPos.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 0;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, SingletonMgr.MessageIDMgr.NpcPositionMessageID, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
    }
    //客户端重连，同步Npc位置信息
    public void SendNpcPositionRelink(PlayerActor stationPlayerActor,
        float posX, float posY, float posZ,
        float angleX, float angleY, float angleZ,
        int npcId, int npcType, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_networkModule == null) return;
        if (stationPlayerActor == null) return;
        Agent agent = stationPlayerActor.Agent;
        NpcPosition npcPos = new NpcPosition()
        {
            m_posX = posX,
            m_posY = posY,
            m_posZ = posZ,
            m_angleX = angleX,
            m_angleY = angleY,
            m_angleZ = angleZ,
            m_npcId = npcId,
            m_npcType = npcType,
            m_stationIndex = stationIndex,
            m_stationClientType = stationClientType,
        };
        byte[] bytes = npcPos.Packet2Bytes();
        UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
        UInt16 u3dId = 0;
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet packet = new Packet(sendId, u3dId, SingletonMgr.MessageIDMgr.NpcPositionMessageID, msgLen, bytes);
        agent.SendPacket(packet.Packet2Bytes());
    }
    //发送Npc动作信息
    public void SendNpcAnimation(UInt16 npcAnimationType, int npcId, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_stationModule == null || m_networkModule == null) return;
        List<PlayerActor> u3dPlayerActorList = null;
        m_stationModule.GetU3DPlayerActors(stationIndex, ref u3dPlayerActorList);
        if (u3dPlayerActorList == null || u3dPlayerActorList.Count == 0) return;
        PlayerActor stationPlayerActor = null;
        for (int k = 0; k < u3dPlayerActorList.Count; k++)
        {
            stationPlayerActor = m_networkModule.GetStationPlayerActorAboutU3DPlayerActor(u3dPlayerActorList[k], stationIndex,
                stationClientType);
            if (stationPlayerActor != null)
            {
                Agent agent = stationPlayerActor.Agent;
                NpcAnimation npcAnimation = new NpcAnimation()
                {
                    m_npcAnimationType = npcAnimationType,
                    m_npcId = npcId,
                    m_stationIndex = stationIndex,
                    m_stationClientType = stationClientType,
                };
                byte[] bytes = npcAnimation.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 0;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.NpcAnimationMessageID, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
    }
    //发送Npc销毁信息
    public void SendNpcDestroy(int npcId, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_stationModule == null || m_networkModule == null) return;
        List<PlayerActor> u3dPlayerActorList = null;
        m_stationModule.GetU3DPlayerActors(stationIndex, ref u3dPlayerActorList);
        if (u3dPlayerActorList == null || u3dPlayerActorList.Count == 0) return;
        PlayerActor stationPlayerActor = null;
        for (int k = 0; k < u3dPlayerActorList.Count; k++)
        {
            stationPlayerActor = m_networkModule.GetStationPlayerActorAboutU3DPlayerActor(u3dPlayerActorList[k], stationIndex, stationClientType);
            if (stationPlayerActor != null)
            {
                Agent agent = stationPlayerActor.Agent;
                NpcDestroy npcDestroy = new NpcDestroy()
                {
                    m_npcId = npcId,
                    m_stationIndex = stationIndex,
                    m_stationClientType = stationClientType,
                };
                byte[] bytes = npcDestroy.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 0;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.NpcDestroyMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
    }
    #endregion
}
