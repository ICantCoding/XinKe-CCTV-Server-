
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
        #endregion

        #region 公有方法
        //获取Station当前客户端正在观看的StationPlayerActor
        public void GetStationPlayerActorAboutU3DPlayerActor(PlayerActor u3dPlayerActor, UInt16 stationIndex, List<PlayerActor> stationPlayerActorList)
        {
            if (stationPlayerActorList == null) return;
            stationPlayerActorList.Clear();
            m_networkEngine.GetStationPlayerActorAboutU3DPlayerActor(u3dPlayerActor, stationIndex, stationPlayerActorList);
        }
        public PlayerActor GetStationPlayerActorAboutU3DPlayerActor(PlayerActor u3dPlayerActor, UInt16 stationIndex,
            UInt16 stationClientType)
        {
            return m_networkEngine.GetStationPlayerActorAboutU3DPlayerActor(u3dPlayerActor, stationIndex, stationClientType);
        }
        //获得U3DPlayerActor
        public PlayerActor GetPlayerActorByU3dId(UInt16 u3dId)
        {
            return m_networkEngine.GetPlayerActorByU3dId(u3dId);
        }
        //获取所有连接上的U3DPlayerActor List
        public List<PlayerActor> GetU3DPlayerActors()
        {
            return m_networkEngine.GetU3DPlayerActors();
        }
        #endregion
    }
}
