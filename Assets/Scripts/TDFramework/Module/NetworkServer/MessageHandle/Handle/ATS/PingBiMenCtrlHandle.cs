using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class PingBiMenCtrlHandle : BaseHandle
{
    #region 构造函数
    public PingBiMenCtrlHandle(BaseClientNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "PingBiMenCtrlHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        PingBiMenCtrl response = new PingBiMenCtrl(packet.m_data);
        //站台索引
        UInt16 stationIndex = response.m_stationIndex;
        //上行或下行,1下行,0上行
        byte upOrDownFlag = response.m_upOrDownFlag;
        //打开或者关闭,1打开,0关闭
        byte statusFlag = response.m_statusFlag;

        #region 无意义，测试用
        //屏蔽门执行动画==================================可注释，提高性能
        // StationModule module = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
        // PingBiMenMgr mgr = (PingBiMenMgr)module.GetDeviceMgr(stationIndex, DeviceType.PingBiMen);
        // if (upOrDownFlag == 0)
        // {
        //     //上行
        //     if (statusFlag == 1)
        //     {
        //         for (int i = 0; i < mgr.ShangXingPingBiMenList.Count; ++i)
        //         {
        //             mgr.ShangXingPingBiMenList[i].Open(null);
        //         }
        //     }
        //     else if(statusFlag == 0)
        //     {
        //         for (int i = 0; i < mgr.ShangXingPingBiMenList.Count; ++i)
        //         {
        //             mgr.ShangXingPingBiMenList[i].Close(null);
        //         }
        //     }
        // }
        // else if (upOrDownFlag == 1)
        // {
        //     //下行
        //     if (statusFlag == 1)
        //     {
        //         for (int i = 0; i < mgr.XiaXingPingBiMenList.Count; ++i)
        //         {
        //             mgr.XiaXingPingBiMenList[i].Open(null);
        //         }
        //     }
        //     else if(statusFlag == 0)
        //     {
        //         for (int i = 0; i < mgr.XiaXingPingBiMenList.Count; ++i)
        //         {
        //             mgr.XiaXingPingBiMenList[i].Close(null);
        //         }
        //     }
        // }
        #endregion

        //转发给所有的客户端
        UnityEngine.Debug.Log("接收到PingBiMenCtrl消息.");
        Broadcast2AllU3DPlayerActor(packet);
    }
    #endregion
}
