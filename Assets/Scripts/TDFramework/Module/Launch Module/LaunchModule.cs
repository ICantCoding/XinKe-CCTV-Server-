
/* 
    LaunchModule 模块用于在启动程序之前从各种配置文件中读取程序所需的配置信息
*/


namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;

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
            // Stopwatch sw = new Stopwatch();
            // sw.Start();
            SingletonMgr.GameGlobalInfo.StationDeviceAndPointInfo = DeviceAndPointInfo.DeserializationDeviceAndPointInfo4Xml();
            // sw.Stop();
            // TimeSpan ts2 = sw.Elapsed;
            // UnityEngine.Debug.Log("LaunchModule总共花费" + ts2.TotalMilliseconds + "ms.");

            //读取 Station摄像头信息
            Dictionary<System.UInt16, CameraInfoList> m_cameraInfoListDict = new Dictionary<ushort, CameraInfoList>();
            //赵营
            CameraInfoList zhaoYingCameraInfoList = CameraInfoList.DeserializeCameraInfoFromXml(AppConfigPath.ZhaoYingCameraInfoXmlPath);
            m_cameraInfoListDict.Add(5, zhaoYingCameraInfoList);
            SingletonMgr.GameGlobalInfo.CameraInfoListDict = m_cameraInfoListDict;
        }
        public override void Release()
        {

        }
        #endregion
    }
}

