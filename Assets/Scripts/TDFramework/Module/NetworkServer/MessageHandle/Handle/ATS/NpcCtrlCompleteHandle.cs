using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;

public class NpcCtrlCompleteHandle : BaseHandle
{
    #region 构造函数
    public NpcCtrlCompleteHandle(BaseClientNetworkEngine networkEngine) :
        base(networkEngine)
    {
        Name = "NpcCtrlCompleteHandle";
    }
    #endregion

    #region 重写方法
    public override void ReceivePacket(Packet packet)
    {
        if (packet == null) return;
        //Npc控制不需要在服务器进行转发给所有的客户端
        if (packet == null) return;
        NpcCtrlComplete response = new NpcCtrlComplete(packet.m_data);
        //站台索引
        UInt16 stationIndex = response.m_stationIndex;
        //0上行,1下行
        byte upOrDownFlag = response.m_upOrDownFlag;
        //0上车,1下车
        byte statusFlag = response.m_statusFlag;

        UnityEngine.Debug.Log("接收到NpcCtrlComplete消息.");

        //该消息控制屏蔽门状态，因为屏蔽门状态决定了Npc行为
        StationModule module = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
        PingBiMenMgr mgr = (PingBiMenMgr)module.GetDeviceMgr(stationIndex, DeviceType.PingBiMen);
        if (upOrDownFlag == 0)
        {
            //上行
            mgr.ShangXingPingBiMenIsOpen = false;
            if (statusFlag == 0)
            {
                //上车
                for (int i = 0; i < mgr.ShangXingPingBiMenList.Count; ++i)
                {
                    ((PingBiMenDevice)mgr.ShangXingPingBiMenList[i]).CanUp = false;
                }
            }
            else if (statusFlag == 1)
            {
                //下车
                for (int i = 0; i < mgr.ShangXingPingBiMenList.Count; ++i)
                {
                    ((PingBiMenDevice)mgr.ShangXingPingBiMenList[i]).CanDown = false;
                }
            }
        }
        else if (upOrDownFlag == 1)
        {
            //下行
            mgr.XiaXingPingBiMenIsOpen = false;
            if (statusFlag == 0)
            {
                //上车
                for (int i = 0; i < mgr.XiaXingPingBiMenList.Count; ++i)
                {
                    ((PingBiMenDevice)mgr.XiaXingPingBiMenList[i]).CanUp = false;
                }
            }
            else if (statusFlag == 1)
            {
                //下车
                for (int i = 0; i < mgr.XiaXingPingBiMenList.Count; ++i)
                {
                    ((PingBiMenDevice)mgr.XiaXingPingBiMenList[i]).CanDown = false;
                }
            }
        }
    }
    #endregion
}
