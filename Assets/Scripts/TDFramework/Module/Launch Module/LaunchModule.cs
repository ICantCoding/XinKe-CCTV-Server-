

namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;

    //LaunchModule 模块用于在启动程序之前从各种配置文件中读取程序所需的配置信息
    public class LaunchModule : IModule
    {
        #region 抽象方法实现
        public override void Init()
        {
            //读取 CCTV服务器信息(包括ServerName, ServerPort)
            SingletonMgr.GameGlobalInfo.ServerInfo = ServerInfo.DeserializeServerInfoFromXml();
            //读取 站台信息
            SingletonMgr.GameGlobalInfo.StationInfoList = StationInfoList.DeserializeStationInfoListFromXml();
            // //读取 站台0闸机信息, 不是很有必要初始化该类数据
            // SingletonMgr.GameGlobalInfo.ZhaJiDeviceInfoCollectionStation0 = ZhaJiDeviceInfoCollection.DeserializedZhaJiDeviceInfoCollectionAtStation0();
            //读取 站台设备与位置点关联信息
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SingletonMgr.GameGlobalInfo.StationDeviceAndPointInfo = DeviceAndPointInfo.DeserializationDeviceAndPointInfo4Xml();
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            UnityEngine.Debug.Log("LaunchModule总共花费" + ts2.TotalMilliseconds + "ms.");
        }
        public override void Release()
        {

        }
        #endregion
    }
}

