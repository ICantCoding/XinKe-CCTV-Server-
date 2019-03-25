
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
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.LaunchModuleName);
            //加载NetworkModule模块，用于启动网络服务器，与客户端连接通信
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.NetworkModuleName);
            //加载ResourcesModule模块，用于加载AssetBundle打包的各种资源和对象
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.ResourcesModuleName);
            //加载UIModule模块
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.UIModuleName);
            //加载StationModule模块
            SingletonMgr.ModuleMgr.RegisterModule(StringMgr.StationModuleName);
        }
        void OnDestroy()
        {
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.ResourcesModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.UIModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.StationModuleName);
        }
        void OnApplicationQuit()
        {
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.LaunchModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.NetworkModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.ResourcesModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.UIModuleName);
            SingletonMgr.ModuleMgr.RemoveModule(StringMgr.StationModuleName);
        }
        #endregion
    }
}