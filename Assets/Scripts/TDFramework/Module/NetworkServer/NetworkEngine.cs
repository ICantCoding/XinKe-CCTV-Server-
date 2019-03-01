

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NetworkEngine : MonoSingleton<NetworkEngine>
    {

        #region 字段
        private ActorManager m_actorManager = null;
        private ServerActor m_serverActor = null;
        #endregion

        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        void OnDestroy()
        {
            if (m_serverActor != null)
            {
                m_serverActor.Stop(); //断开所有与客户端的连接，关闭服务器
                m_serverActor = null;
            }
        }
        void OnApplicationQuit()
        {
            if (m_serverActor != null)
            {
                m_serverActor.Stop(); //断开所有与客户端的连接，关闭服务器
                m_serverActor = null;
            }
        }
        #endregion

        #region 方法
        public void Run(int serverPort)
        {
            m_actorManager = new ActorManager();
            //创建看门狗Actor
            WatchDogActor watchDogActor = new WatchDogActor(this);
            m_actorManager.AddActor(watchDogActor, true);
            //创建WorldActor
            WorldActor worldActor = new WorldActor(this);
            m_actorManager.AddActor(worldActor, true);
            //创建服务器Actor
            m_serverActor = new ServerActor(this);
            ActorManager.Instance.AddActor(m_serverActor, true);
            m_serverActor.Start(serverPort); //启动服务器
        }
        public void Stop()
        {
            if (m_serverActor != null)
            {
                m_serverActor.Over(); //断开所有与客户端的连接，关闭服务器
                m_serverActor = null;
            }
        }
        #endregion
    }
}