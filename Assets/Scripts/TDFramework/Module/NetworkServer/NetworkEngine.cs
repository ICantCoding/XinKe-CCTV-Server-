

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NetworkEngine : MonoSingleton<NetworkEngine>
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
        void OnDestroy()
        {
            Destroy();
        }
        void OnApplicationQuit()
        {
            Destroy();
        }
        #endregion

        #region 方法
        public void Run(int serverPort)
        {
            //先删除，终止ServerActor
            if (m_serverActor != null)
            {
                m_actorManager.RemoveActor(m_serverActor.Id);
                m_serverActor = null;
            }
            //先删除，终止WorldActor
            if (m_worldActor != null)
            {
                m_actorManager.RemoveActor(m_worldActor.Id);
                m_worldActor = null;
            }

            //创建ActorManager
            if (m_actorManager == null)
            {
                m_actorManager = new ActorManager();
            }
            //创建看门狗Actor
            if (m_watchDogActor == null)
            {
                m_watchDogActor = new WatchDogActor(this);
                m_actorManager.AddActor(m_watchDogActor, true); //Id为1的Actor, 不与Agent绑定，也没有U3DId标识
            }
            //创建WorldActor
            if (m_worldActor == null)
            {
                m_worldActor = new WorldActor(this);
                m_actorManager.AddActor(m_worldActor, true); //Id为2的Actor, 不与Agent绑定，也没有U3DId标识
            }
            //创建服务器Actor
            if (m_serverActor == null)
            {
                m_serverActor = new ServerActor(this);
                m_actorManager.AddActor(m_serverActor, true); //Id为3的Actor, 不与Agent绑定，也没有U3DId标识
                m_serverActor.Start(serverPort); //启动服务器
            }
        }
        public void Pause()
        {
            if (m_actorManager != null && m_serverActor != null)
            {
                m_serverActor.End();
            }
            //程序中ActorManager，还保留了WatchDogActor和WorldActor, 清空这两个Actor.
            if (m_actorManager != null)
            {
                //不移除看门狗Actor是因为，Agent的Close消息需要看门狗转发. 
                //不移除世界WorldActor， 因为看门狗处理消息事件会滞后一帧
                if (m_worldActor != null)
                {
                    m_worldActor.Stop();
                }
            }
        }
        public void Destroy()
        {
            if (m_actorManager != null && m_serverActor != null)
            {
                m_serverActor.End();
                m_actorManager.RemoveActor(m_serverActor.Id);
                m_serverActor = null;
            }
            //程序中ActorManager，还保留了WatchDogActor和WorldActor, 清空这两个Actor.
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
            }
        }
        #endregion
    }
}