using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class DeviceSync : MonoBehaviour
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

    #region 同步设备状态信令
    //闸机设备打开消息
    public void SendDeviceStatus(int status, UInt16 stationIndex, DeviceType deviceType, int deviceId)
    {
        if (m_networkModule == null) return;
        List<PlayerActor> u3dPlayerActorList = m_networkModule.GetU3DPlayerActorListAtXXStation(stationIndex);
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
                UInt16 firstId = 0;
                UInt16 secondId = 5;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, firstId, secondId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        u3dPlayerActorList = null;
    }
    #endregion
}
