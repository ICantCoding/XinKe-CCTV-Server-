using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGlobalComponent
{
    #region 字段
    private static DeviceSync m_deviceSync;
    private static NpcSync m_npcSync;
    #endregion

    #region 属性
    //设备同步组件
    public static DeviceSync DeviceSync
    {
        get
        {
            if(m_deviceSync == null)
            {
                GameObject go = GameObject.Find(StringMgr.DeviceSyncGoName);
                if(go != null)
                {
                    m_deviceSync = go.GetComponent<DeviceSync>();
                }
            }
            return m_deviceSync;
        }
    }
    //Npc同步组件
    public static NpcSync NpcSync
    {
        get
        {
            if(m_npcSync == null)
            {
                GameObject go = GameObject.Find(StringMgr.NpcSyncGoName);
                if(go != null)
                {
                    m_npcSync = go.GetComponent<NpcSync>();
                }
            }
            return m_npcSync;
        }
    }
    #endregion
}
