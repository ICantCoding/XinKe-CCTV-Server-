
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppStart : MonoBehaviour
    {
        #region 字段

        #endregion

        #region Unity生命周期
        void Start()
        {
            //加载LaunchModule模块，用于读取应用程序所需要的配置数据
            SingletonMgr.ModuleMgr.RegisterModule("LaunchModule");
            //加载NetworkModule模块，用于启动网络服务器，与客户端连接通信
            SingletonMgr.ModuleMgr.RegisterModule("NetworkModule");
        }
        void OnDestroy()
        {
            SingletonMgr.ModuleMgr.RemoveModule("LaunchModule");
            SingletonMgr.ModuleMgr.RemoveModule("NetworkModule");
        }
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule("LaunchModule");
            SingletonMgr.ModuleMgr.RemoveModule("NetworkModule");
        }
        #endregion
    }
}