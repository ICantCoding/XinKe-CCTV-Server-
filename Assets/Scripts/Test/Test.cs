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
    }
    #endregion
}
