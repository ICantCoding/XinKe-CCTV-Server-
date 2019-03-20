
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
            SingletonMgr.ModuleMgr.RegisterModule(ModuleName.LaunchModule);
            //加载NetworkModule模块，用于启动网络服务器，与客户端连接通信
            SingletonMgr.ModuleMgr.RegisterModule(ModuleName.NetworkModule);
            //加载ResourcesModule模块，用于加载AssetBundle打包的各种资源和对象
            SingletonMgr.ModuleMgr.RegisterModule(ModuleName.ResourcesModule);
            //加载UIModule模块
            SingletonMgr.ModuleMgr.RegisterModule(ModuleName.UIModule);
            //加载StationModule模块
            SingletonMgr.ModuleMgr.RegisterModule(ModuleName.StationModule);
        }
        void OnDestroy()
        {
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.LaunchModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.NetworkModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.ResourcesModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.UIModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.StationModule);
        }
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.LaunchModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.NetworkModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.ResourcesModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.UIModule);
            SingletonMgr.ModuleMgr.RemoveModule(ModuleName.StationModule);
        }
        #endregion
    }
}