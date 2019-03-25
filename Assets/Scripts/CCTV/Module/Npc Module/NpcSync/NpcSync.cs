using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class NpcSync : MonoBehaviour
{
    #region 字段
    private NpcAction m_npcAction = null;
    private NetworkModule m_networkModule = null;
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_npcAction = GetComponent<NpcAction>();
    }
    void Start()
    {
        //开启Npc同步
        StartCoroutine(SyncNpcPosition());
    }
    #endregion

    #region 同步Npc信息
    IEnumerator SyncNpcPosition()
    {
        Vector3 prePos = transform.localPosition;
        Vector3 nowPos = transform.localPosition;
        float posX, posY, posZ, angleX, angleY, angleZ = 0.0f;
        int npcId = m_npcAction.NpcId;
        List<PlayerActor> stationPlayerActorList = new List<PlayerActor>();
        //先尝试获取NetworkModule, 获取之后才能开始同步Npc
        while (m_networkModule == null)
        {
            m_networkModule = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            yield return null;
        }
        //随后开始发送Npc同步信息
        while (true)
        {
            posX = transform.localPosition.x;
            posY = transform.localPosition.y;
            posZ = transform.localPosition.z;
            angleX = transform.localEulerAngles.x;
            angleY = transform.localEulerAngles.y;
            angleZ = transform.localEulerAngles.z;

            if (Vector3.Distance(prePos, nowPos) > 0.2f) //NPC位置发生改变时才，去同步
            {
                prePos.x = posX;
                prePos.y = posY;
                prePos.z = posZ;
                SendNpcPosition(stationPlayerActorList,
                    posX, posY, posZ,
                    angleX, angleY, angleZ,
                    npcId, m_npcAction.StationIndex, (UInt16)m_npcAction.NpcActionStatus);
            }
            else
            {
                nowPos.x = transform.localPosition.x;
                nowPos.y = transform.localPosition.y;
                nowPos.z = transform.localPosition.z;
            }
            yield return null;
        }
    }
    //发送Npc位置信息
    public void SendNpcPosition(List<PlayerActor> stationPlayerActorList,
        float posX, float posY, float posZ,
        float angleX, float angleY, float angleZ,
        int npcId, UInt16 stationIndex, UInt16 stationClientType)
    {
        m_networkModule.GetStationPlayerActorAtXXStation(stationIndex,
                            (UInt16)m_npcAction.NpcActionStatus, stationPlayerActorList);
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
    public void SendNpcAnimation(UInt16 npcAnimationType)
    {
        if (m_networkModule != null)
        {
            List<PlayerActor> stationPlayerActorList = new List<PlayerActor>();
            m_networkModule.GetStationPlayerActorAtXXStation(m_npcAction.StationIndex,
                (UInt16)m_npcAction.NpcActionStatus, stationPlayerActorList);
            if (stationPlayerActorList != null && stationPlayerActorList.Count > 0)
            {
                for (int i = 0; i < stationPlayerActorList.Count; i++)
                {
                    Agent agent = stationPlayerActorList[i].Agent;
                    NpcAnimation npcAnimation = new NpcAnimation()
                    {
                        m_npcAnimationType = npcAnimationType,
                        m_npcId = m_npcAction.NpcId,
                        m_stationIndex = m_npcAction.StationIndex,
                        m_stationClientType = (UInt16)m_npcAction.NpcActionStatus,
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
    }
    #endregion
}
