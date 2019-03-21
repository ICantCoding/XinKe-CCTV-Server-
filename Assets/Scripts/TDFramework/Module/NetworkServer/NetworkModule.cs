
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NetworkModule : IModule
    {
        #region 字段
        private NetworkEngine m_networkEngine = null;
        #endregion

        #region 抽象方法实现
        public override void Init()
        {
            string goName = StringMgr.NetworkEngineName;
            GameObject networkEngineGo = GameObject.Find(goName);
            if (networkEngineGo == null)
            {
                networkEngineGo = new GameObject(goName);
            }
            m_networkEngine = networkEngineGo.AddComponent<NetworkEngine>();
        }
        public override void Release()
        {
            Stop();
        }
        #endregion

        #region 方法
        public void Start()
        {
            if (m_networkEngine != null)
            {
                m_networkEngine.StartService(SingletonMgr.GameGlobalInfo.ServerInfo.ServerPort);
            }
        }
        public void Stop()
        {
            if (m_networkEngine != null)
            {
                m_networkEngine.StopServerActor();
            }
        }
        //获取Station类型的PlayerActor
        public List<PlayerActor> GetPlayerActorByStationIndexAndStationClientType(UInt16 stationIndex, UInt16 stationClientType)
        {
           return m_networkEngine.GetPlayerActorByStationIndexAndStationClientType(stationIndex, stationClientType);
        }
        #endregion
    }
}
