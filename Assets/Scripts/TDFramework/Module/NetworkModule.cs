
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class NetworkModule : IModule
    {
        #region 字段
        private NetworkEngine m_networkEngine = null;
        #endregion

        #region 抽象方法实现
        public override void Init()
        {
            m_networkEngine = SingletonMgr.NetworkEngine;
        }
        public override void Release()
        {
            //关闭客户端连接服务器
            if (m_networkEngine != null)
            {
                m_networkEngine.Destroy();
                m_networkEngine = null;
            }
        }
        #endregion

        #region 方法
        public void Run()
        {
            if(m_networkEngine != null)
            {
                m_networkEngine.Run(SingletonMgr.GameGlobalInfo.ServerInfo.ServerPort);
            }
        }
        public void Pause()
        {
            if(m_networkEngine != null)
            {
                m_networkEngine.Pause();
            }
        }
        public void Destroy()
        {
            if(m_networkEngine != null)
            {
                m_networkEngine.Destroy();
                m_networkEngine = null;
            }
        }
        #endregion
    }
}
