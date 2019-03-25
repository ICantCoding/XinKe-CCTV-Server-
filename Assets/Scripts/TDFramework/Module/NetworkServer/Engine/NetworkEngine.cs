

namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NetworkEngine : MonoBehaviour
    {
        #region 字段
        private ActorManager m_actorManager = null;
        private WatchDogActor m_watchDogActor = null;
        private WorldActor m_worldActor = null;
        private ServerActor m_serverActor = null;
        #endregion

        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        #region 方法
        public void StartService(int serverPort)
        {
            #region 二次开启所需处理
            //开启之前先尝试关闭服务
            StopDogActorAndWorldActor();
            #endregion

            //创建ActorManager
            if (m_actorManager == null)
            {
                m_actorManager = new ActorManager();
            }
            //创建看门狗Actor
            if (m_watchDogActor == null)
            {
                m_watchDogActor = new WatchDogActor(this);
                //Id为1的Actor, 不与Agent绑定，也没有U3DId标识
                m_actorManager.AddActor(m_watchDogActor, true);
            }
            //创建WorldActor
            if (m_worldActor == null)
            {
                m_worldActor = new WorldActor(this);
                m_watchDogActor.WorldActor = m_worldActor;
                //Id为2的Actor, 不与Agent绑定，也没有U3DId标识
                m_actorManager.AddActor(m_worldActor, true);
            }
            //创建服务器Actor
            if (m_serverActor == null)
            {
                m_serverActor = new ServerActor(this);
                m_watchDogActor.ServerActor = m_serverActor;
                m_serverActor.WatchDogActor = m_watchDogActor;
                //Id为3的Actor, 不与Agent绑定，也没有U3DId标识
                m_actorManager.AddActor(m_serverActor, true);
                m_serverActor.Start(serverPort); //启动服务器
            }
        }

        public void StopServerActor()
        {
            if (m_serverActor != null)
            {
                m_serverActor.End();
                m_actorManager.RemoveActor(m_serverActor.Id);
                m_serverActor = null;
            }
        }

        public void StopDogActorAndWorldActor()
        {
            if (m_actorManager != null)
            {
                if (m_watchDogActor != null)
                {
                    m_watchDogActor.Stop();
                    m_actorManager.RemoveActor(m_watchDogActor.Id);
                    m_watchDogActor = null;
                }
                if (m_worldActor != null)
                {
                    m_worldActor.Stop();
                    m_actorManager.RemoveActor(m_worldActor.Id);
                    m_worldActor = null;
                }
                m_actorManager = null;
            }
        }
        //获取Station类型的PlayerActor
        public void GetStationPlayerActorAtXXStation(UInt16 stationIndex, UInt16 stationClientType, List<PlayerActor> stationPlayerActorList)
        {
            if (m_worldActor != null)
            {
                List<PlayerActor> u3dPlayerActorList = m_worldActor.GetU3DPlayerActorListAtXXStation(stationIndex);
                if(u3dPlayerActorList == null || u3dPlayerActorList.Count == 0) return;

                var enumerator = u3dPlayerActorList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    PlayerActor playerActor = enumerator.Current;
                    string keyStr = string.Format("{0}-{1}-{2}", playerActor.U3DId, stationIndex, stationClientType);
                    playerActor = m_worldActor.GetStationPlayerActor(keyStr);
                    if (playerActor != null)
                    {
                        stationPlayerActorList.Add(playerActor);
                    }
                }
                enumerator.Dispose();
            }
        }
        #endregion
    }
}