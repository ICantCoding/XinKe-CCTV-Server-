

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
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
        }
        public override void Release()
        {

        }
        #endregion
    }
}

