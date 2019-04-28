using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class CCTVClientNetworkEngine : BaseClientNetworkEngine
{
    #region Unity生命周期
    void Start()
    {
        Run(CCTVClientConnectServerSuccess_Callback, CCTVClientConnectServerFail_Callback);
    }
    #endregion

    #region 重写方法
    public override void Run(RemoteClientConnectServerSuccessCallback successCallback,
        RemoteClientConnectServerFailCallback failCallback)
    {
        Run(SingletonMgr.GameGlobalInfo.ServerInfo.CctvServerIpAddress, SingletonMgr.GameGlobalInfo.ServerInfo.CctvServerPort, 
            successCallback, failCallback);
    }
    #endregion


    #region 网络连接，回调方法
    private void CCTVClientConnectServerSuccess_Callback()
    {
        SendNotification(EventID_Cmd.CCTVCtrlClientOnLine, null, null);
    }
    private void CCTVClientConnectServerFail_Callback()
    {
        SendNotification(EventID_Cmd.CCTVCtrlClientOffLine, null, null);
        //连接服务器失败后，停止并删除之前连接的资源
        Stop(); 
    }
    #endregion
}
