using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class DeviceSync : MonoBehaviour
{
    #region 字段
    private StationModule m_stationModule = null;
    #endregion

    #region Unity生命周期
    void Start()
    {
        m_stationModule = TDFramework.SingletonMgr.ModuleMgr.StationModule;
    }
    #endregion

    #region 同步设备状态信令
    //设备打开消息
    public void SendDeviceStatus(int status, UInt16 stationIndex, DeviceType deviceType, int deviceId)
    {
        if (m_stationModule == null) return;
        List<PlayerActor> u3dPlayerActorList = new List<PlayerActor>();
        m_stationModule.GetU3DPlayerActors(stationIndex, ref u3dPlayerActorList);
        if (u3dPlayerActorList != null && u3dPlayerActorList.Count > 0)
        {
            for (int i = 0; i < u3dPlayerActorList.Count; i++)
            {
                Agent agent = u3dPlayerActorList[i].Agent;
                DeviceAction deviceAction = new DeviceAction()
                {
                    m_deviceId = deviceId,
                    m_deviceType = (int)deviceType,
                    m_stationIndex = stationIndex,
                    m_deviceStatus = (byte)status,
                };
                byte[] bytes = deviceAction.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 0;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.DeviceActionMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        u3dPlayerActorList = null;
    }
    //设备打开消息，同步给重连Socket
    public void SendDeviceStatusRelink(int status, UInt16 stationIndex, DeviceType deviceType, int deviceId, PlayerActor u3dPlayerActor)
    {
        if (u3dPlayerActor == null) return;
        Agent agent = u3dPlayerActor.Agent;
        DeviceAction deviceAction = new DeviceAction()
        {
            m_deviceId = deviceId,
            m_deviceType = (int)deviceType,
            m_stationIndex = stationIndex,
            m_deviceStatus = (byte)status,
        };
        byte[] bytes = deviceAction.Packet2Bytes();
        UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
        UInt16 u3dId = 0;
        UInt16 msgLen = (UInt16)bytes.Length;
        Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.DeviceActionMessageId, msgLen, bytes);
        agent.SendPacket(packet.Packet2Bytes());
    }
    #endregion
}
