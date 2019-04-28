namespace TDFramework {
    using System.Collections.Generic;
    using System.Collections;
    using System;
    using UnityEngine;

    public class NetworkEngine : MonoBehaviour {
        #region 字段
        private ActorManager m_actorManager = null;
        private WatchDogActor m_watchDogActor = null;
        private WorldActor m_worldActor = null;
        private ServerActor m_serverActor = null;
        #endregion

        #region Unity生命周期
        void Awake () {
            DontDestroyOnLoad (this.gameObject);
        }
        #endregion

        #region 开启关闭网络引擎方法
        #region 开启服务器引擎
        public void StartService (int serverPort) {
            if (m_actorManager != null) return;

            //创建ActorManager
            m_actorManager = new ActorManager ();
            //创建看门狗Actor
            m_watchDogActor = new WatchDogActor (this);
            //Id为1的Actor, 不与Agent绑定，也没有U3DId标识
            m_actorManager.AddActor (m_watchDogActor, true);
            //创建WorldActor
            m_worldActor = new WorldActor (this);
            m_watchDogActor.WorldActor = m_worldActor;
            //Id为2的Actor, 不与Agent绑定，也没有U3DId标识
            m_actorManager.AddActor (m_worldActor, true);
            //创建服务器Actor
            m_serverActor = new ServerActor (this);
            m_watchDogActor.ServerActor = m_serverActor;
            m_serverActor.WatchDogActor = m_watchDogActor;
            //Id为3的Actor, 不与Agent绑定，也没有U3DId标识
            m_actorManager.AddActor (m_serverActor, true);
            //启动服务器
            m_serverActor.Start (serverPort);
        }
        #endregion
        #region 暂停服务器，禁止使用
        public void StopServerActor () {
            if (m_serverActor != null) {
                m_serverActor.End ();
                m_actorManager.RemoveActor (m_serverActor.Id);
                m_serverActor = null;
            }
        }
        public void StopDogActorAndWorldActor () {
            if (m_actorManager != null) {
                if (m_watchDogActor != null) {
                    m_watchDogActor.Stop ();
                    m_actorManager.RemoveActor (m_watchDogActor.Id);
                    m_watchDogActor = null;
                }
                if (m_worldActor != null) {
                    m_worldActor.Stop ();
                    m_actorManager.RemoveActor (m_worldActor.Id);
                    m_worldActor = null;
                }
                m_actorManager = null;
            }
        }
        #endregion
        #endregion

        #region 公有方法
        //获取Station类型的PlayerActor
        public void GetStationPlayerActorAboutU3DPlayerActor (PlayerActor u3dPlayerActor, UInt16 stationIndex, List<PlayerActor> stationPlayerActorList) {
            if (stationPlayerActorList == null) return;
            if (m_worldActor != null) {
                string keyStr = "";
                PlayerActor playerActor = null;
                for (int i = 1; i <= 4; i++) {
                    keyStr = string.Format ("{0}-{1}-{2}", u3dPlayerActor.U3DId, stationIndex, 1);
                    playerActor = m_worldActor.GetStationPlayerActor (keyStr);
                    if (playerActor != null) {
                        stationPlayerActorList.Add (playerActor);
                    }
                }
            }
        }
        public PlayerActor GetStationPlayerActorAboutU3DPlayerActor (PlayerActor u3dPlayerActor, UInt16 stationIndex,
            UInt16 stationClientType) {
            PlayerActor playerActor = null;
            if (m_worldActor != null) {
                string keyStr = "";
                keyStr = string.Format ("{0}-{1}-{2}", u3dPlayerActor.U3DId, stationIndex, stationClientType);
                playerActor = m_worldActor.GetStationPlayerActor (keyStr);

            }
            return playerActor;
        }
        //获得U3DPlayerActor
        public PlayerActor GetPlayerActorByU3dId (UInt16 u3dId) {
            if (m_worldActor != null) {
                return m_worldActor.GetPlayerActorByU3dId (u3dId);
            }
            return null;
        }
        //获得所有连接上的U3DPlayerActor
        public List<PlayerActor> GetU3DPlayerActors () {
            if (m_worldActor == null) {
                return null;
            }
            List<PlayerActor> list = new List<PlayerActor> ();
            foreach (var item in m_worldActor.U3DPlayerActorDict) {
                list.Add (item.Value);
            }
            return list;
        }
        #endregion
    }
}