using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NpcSync : MonoBehaviour
{
    #region 字段
    private NetworkModule m_networkModule = null;
    #endregion

    #region Unity生命周期
    void Start()
    {
        m_networkModule = (NetworkModule)TDFramework.SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
    }
    #endregion

    #region 同步Npc信息
    //发送Npc位置信息
    public void SendNpcPosition(List<PlayerActor> stationPlayerActorList,
        float posX, float posY, float posZ,
        float angleX, float angleY, float angleZ,
        int npcId, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_networkModule == null) return;
        m_networkModule.GetStationPlayerActorAtXXStation(stationIndex,
                            stationClientType, stationPlayerActorList);
        if (stationPlayerActorList.Count > 0)
        {
            int count = stationPlayerActorList.Count;
            for (int i = 0; i < count; ++i)
            {
                Agent agent = stationPlayerActorList[i].Agent;
                NpcPosition npcPos = new NpcPosition()
                {
                    m_posX = posX,
                    m_posY = posY,
                    m_posZ = posZ,
                    m_angleX = angleX,
                    m_angleY = angleY,
                    m_angleZ = angleZ,
                    m_npcId = npcId,
                    m_stationIndex = stationIndex,
                    m_stationClientType = stationClientType,
                };
                byte[] bytes = npcPos.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 0;
                UInt16 firstId = 0;
                UInt16 secondId = 2;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
    }
    //发送Npc动作信息
    public void SendNpcAnimation(UInt16 npcAnimationType, int npcId, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_networkModule == null) return;
        List<PlayerActor> stationPlayerActorList = new List<PlayerActor>();
        m_networkModule.GetStationPlayerActorAtXXStation(stationIndex,
            stationClientType, stationPlayerActorList);
        if (stationPlayerActorList != null && stationPlayerActorList.Count > 0)
        {
            for (int i = 0; i < stationPlayerActorList.Count; i++)
            {
                Agent agent = stationPlayerActorList[i].Agent;
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
                UInt16 firstId = 0;
                UInt16 secondId = 3;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        stationPlayerActorList = null;
    }
    //发送Npc销毁信息
    public void SendNpcDestroy(int npcId, UInt16 stationIndex, UInt16 stationClientType)
    {
        if (m_networkModule == null) return;
        List<PlayerActor> stationPlayerActorList = new List<PlayerActor>();
        m_networkModule.GetStationPlayerActorAtXXStation(stationIndex,
            stationClientType, stationPlayerActorList);
        if (stationPlayerActorList != null && stationPlayerActorList.Count > 0)
        {
            for (int i = 0; i < stationPlayerActorList.Count; i++)
            {
                Agent agent = stationPlayerActorList[i].Agent;
                NpcDestroy npcDestroy = new NpcDestroy()
                {
                    m_npcId = npcId,
                    m_stationIndex = stationIndex,
                    m_stationClientType = stationClientType,
                };
                byte[] bytes = npcDestroy.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 0;
                UInt16 firstId = 0;
                UInt16 secondId = 4;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        stationPlayerActorList = null;
    }
    #endregion
}
