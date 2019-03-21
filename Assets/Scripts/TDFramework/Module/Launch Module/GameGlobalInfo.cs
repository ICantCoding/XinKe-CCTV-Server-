using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

public class GameGlobalInfo : Singleton<GameGlobalInfo>
{
    #region 客户端信息	
    private ServerInfo m_serverInfo = null;
    public ServerInfo ServerInfo
    {
        get { return m_serverInfo; }
        set { m_serverInfo = value; }
    }
    #endregion


    #region 其他信息

    #endregion
}



