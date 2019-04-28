﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class ATSClientNetworkEngine : BaseClientNetworkEngine
{
    
    #region Unity生命周期
    void Start()
    {
        Run(ATSClientConnectServerSuccess_Callback, ATSClientConnectServerFail_Callback);
    }
    #endregion

    #region 重写方法
    public override void Run(RemoteClientConnectServerSuccessCallback successCallback,
        RemoteClientConnectServerFailCallback failCallback)
    {
        Run(SingletonMgr.GameGlobalInfo.ServerInfo.AtsServerIpAddress, SingletonMgr.GameGlobalInfo.ServerInfo.AtsServerPort, 
            successCallback, failCallback);
    }
    #endregion

    #region 网络连接，回调方法
    private void ATSClientConnectServerSuccess_Callback()
    {
        SendNotification(EventID_Cmd.ATSClientOnLine, null, null);
    }
    private void ATSClientConnectServerFail_Callback()
    {
        SendNotification(EventID_Cmd.ATSClientOffLine, null, null);
        //连接服务器失败后，停止并删除之前连接的资源
        Stop(); 
    }
    #endregion
}