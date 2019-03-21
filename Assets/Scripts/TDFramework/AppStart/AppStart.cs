
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppStart : MonoBehaviour
    {
        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            //加载LaunchModule模块，用于读取应用程序所需要的配置数据
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.LaunchModule);
            //加载NetworkModule模块，用于启动网络服务器，与客户端连接通信
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.NetworkModule);
            //加载ResourcesModule模块，用于加载AssetBundle打包的各种资源和对象
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.ResourcesModule);
            //加载UIModule模块
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.UIModule);
            //加载StationModule模块
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.StationModule);
        }
        void OnDestroy()
        {
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.ResourcesModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.UIModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.StationModule);
        }
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.ResourcesModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.UIModule);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.StationModule);
        }
        #endregion
    }
}