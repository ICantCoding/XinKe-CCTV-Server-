using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class Test : MonoBehaviour
{

    #region Unity生命周期
    void OnGUI()
    {
        if (GUILayout.Button("上行屏蔽门打开"))
        {
            StationModule module = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
            PingBiMenMgr mgr = (PingBiMenMgr)module.GetDeviceMgr(0, DeviceType.PingBiMen);
            mgr.ShangXingPingBiMenIsOpen = true;
            for (int i = 0; i < mgr.ShangXingPingBiMenList.Count; ++i)
            {
                mgr.ShangXingPingBiMenList[i].Open(null);
            }
        }
        if (GUILayout.Button("上行屏蔽门关闭"))
        {
            StationModule module = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
            PingBiMenMgr mgr = (PingBiMenMgr)module.GetDeviceMgr(0, DeviceType.PingBiMen);
            mgr.ShangXingPingBiMenIsOpen = false;
            for (int i = 0; i < mgr.ShangXingPingBiMenList.Count; ++i)
            {
                mgr.ShangXingPingBiMenList[i].Close(null);
            }
        }
        if (GUILayout.Button("下行屏蔽门打开"))
        {
            StationModule module = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
            PingBiMenMgr mgr = (PingBiMenMgr)module.GetDeviceMgr(0, DeviceType.PingBiMen);
            mgr.XiaXingPingBiMenIsOpen = true;
            for (int i = 0; i < mgr.XiaXingPingBiMenList.Count; ++i)
            {
                mgr.XiaXingPingBiMenList[i].Open(null);
            }
        }
        if (GUILayout.Button("下行屏蔽门关闭"))
        {
            StationModule module = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
            PingBiMenMgr mgr = (PingBiMenMgr)module.GetDeviceMgr(0, DeviceType.PingBiMen);
            mgr.XiaXingPingBiMenIsOpen = false;
            for (int i = 0; i < mgr.XiaXingPingBiMenList.Count; ++i)
            {
                mgr.XiaXingPingBiMenList[i].Close(null);
            }
        }
        if (GUILayout.Button("单屏"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                ScreenBindCamera screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenIndex = 0,
                    m_cameraIndex = 5,
                    m_cameraName = "哈哈哈哈XXXX",
                    m_stationIndex = 5,
                };
                byte[] bytes = screenBindCamera.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("切割大屏"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                DivisionBigScreen divisionBigScreen = new DivisionBigScreen()
                {
                    m_bigScreenXDivisionCount = 2,
                    m_bigScreenYDivisionCount = 2
                };
                byte[] bytes = divisionBigScreen.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.DivisionBigScreenMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("切割小屏1"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                DivisionSmallScreen divisionSmallScreen = new DivisionSmallScreen()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenXDivisionCount = 2,
                    m_smallScreenYDivisionCount = 2,
                };
                byte[] bytes = divisionSmallScreen.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.DivisionSmallScreenMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("切割小屏4"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                DivisionSmallScreen divisionSmallScreen = new DivisionSmallScreen()
                {
                    m_bigScreenIndex = 4,
                    m_smallScreenXDivisionCount = 2,
                    m_smallScreenYDivisionCount = 2,
                };
                byte[] bytes = divisionSmallScreen.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.DivisionSmallScreenMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("绑定屏幕小屏四分屏1"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                ScreenBindCamera screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenIndex = 1,
                    m_cameraIndex = 1,
                    m_cameraName = "哈哈哈哈1",
                    m_stationIndex = 5,
                };
                byte[] bytes = screenBindCamera.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());

                screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenIndex = 2,
                    m_cameraIndex = 2,
                    m_cameraName = "哈哈哈哈2",
                    m_stationIndex = 5,
                };
                bytes = screenBindCamera.Packet2Bytes();
                sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                u3dId = 111;
                msgLen = (UInt16)bytes.Length;
                packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());

                screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenIndex = 3,
                    m_cameraIndex = 3,
                    m_cameraName = "哈哈哈哈3",
                    m_stationIndex = 5,
                };
                bytes = screenBindCamera.Packet2Bytes();
                sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                u3dId = 111;
                msgLen = (UInt16)bytes.Length;
                packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());

                screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenIndex = 4,
                    m_cameraIndex = 4,
                    m_cameraName = "哈哈哈哈4",
                    m_stationIndex = 5,
                };
                bytes = screenBindCamera.Packet2Bytes();
                sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                u3dId = 111;
                msgLen = (UInt16)bytes.Length;
                packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("绑定屏幕大屏2"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                ScreenBindCamera screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 2,
                    m_smallScreenIndex = 0,
                    m_cameraIndex = 10,
                    m_cameraName = "哈哈哈哈XXXX",
                    m_stationIndex = 5,
                };
                byte[] bytes = screenBindCamera.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("绑定屏幕大屏3"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                ScreenBindCamera screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 3,
                    m_smallScreenIndex = 0,
                    m_cameraIndex = 14,
                    m_cameraName = "哈哈哈哈XXXX",
                    m_stationIndex = 5,
                };
                byte[] bytes = screenBindCamera.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("绑定屏幕小屏四分屏4"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                ScreenBindCamera screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 4,
                    m_smallScreenIndex = 1,
                    m_cameraIndex = 6,
                    m_cameraName = "哈哈哈哈1",
                    m_stationIndex = 5,
                };
                byte[] bytes = screenBindCamera.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());

                screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 4,
                    m_smallScreenIndex = 2,
                    m_cameraIndex = 7,
                    m_cameraName = "哈哈哈哈2",
                    m_stationIndex = 5,
                };
                bytes = screenBindCamera.Packet2Bytes();
                sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                u3dId = 111;
                msgLen = (UInt16)bytes.Length;
                packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());

                screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 4,
                    m_smallScreenIndex = 3,
                    m_cameraIndex = 8,
                    m_cameraName = "哈哈哈哈3",
                    m_stationIndex = 5,
                };
                bytes = screenBindCamera.Packet2Bytes();
                sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                u3dId = 111;
                msgLen = (UInt16)bytes.Length;
                packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());

                screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 4,
                    m_smallScreenIndex = 4,
                    m_cameraIndex = 9,
                    m_cameraName = "哈哈哈哈4",
                    m_stationIndex = 5,
                };
                bytes = screenBindCamera.Packet2Bytes();
                sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                u3dId = 111;
                msgLen = (UInt16)bytes.Length;
                packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("切割大屏8分屏"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                DivisionBigScreen divisionBigScreen = new DivisionBigScreen()
                {
                    m_bigScreenXDivisionCount = 4,
                    m_bigScreenYDivisionCount = 2
                };
                byte[] bytes = divisionBigScreen.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.DivisionBigScreenMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }
        if (GUILayout.Button("绑定屏幕大屏1"))
        {
            NetworkModule module = (NetworkModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
            if (module == null) return;
            PlayerActor u3dPlayerActor = module.GetPlayerActorByU3dId(111);
            if (u3dPlayerActor != null)
            {
                Agent agent = u3dPlayerActor.Agent;
                ScreenBindCamera screenBindCamera = new ScreenBindCamera()
                {
                    m_bigScreenIndex = 1,
                    m_smallScreenIndex = 0,
                    m_cameraIndex = 20,
                    m_cameraName = "哈哈哈哈XXXX",
                    m_stationIndex = 5,
                };
                byte[] bytes = screenBindCamera.Packet2Bytes();
                UInt16 sendId = TDFramework.SingletonMgr.GameGlobalInfo.ServerInfo.Id;
                UInt16 u3dId = 111;
                UInt16 msgLen = (UInt16)bytes.Length;
                Packet packet = new Packet(sendId, u3dId, TDFramework.SingletonMgr.MessageIDMgr.ScreenBindCameraMessageId, msgLen, bytes);
                agent.SendPacket(packet.Packet2Bytes());
            }
        }

    }
    #endregion
}
