/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-08 11:22:19

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-07 16:24:06

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMgr
{
    #region 游戏物体对象
    public Transform Npc;
    #endregion

    #region 字段
    private NpcActionStatus m_npcActionStatus = NpcActionStatus.None;
    private Dictionary<int, NpcAction> m_npcActionDict = new Dictionary<int, NpcAction>();
    #endregion

    #region 属性
    public NpcActionStatus NpcActionStatus
    {
        get { return m_npcActionStatus; }
        set { m_npcActionStatus = value; }
    }
    #endregion

    #region 方法
    public void AddNpcAction(NpcAction npcAction)
    {
        if(npcAction == null) return;
        if(m_npcActionDict.ContainsKey(npcAction.NpcId) == false)
        {
            m_npcActionDict.Add(npcAction.NpcId, npcAction);
        }
    }
    public void RemoveNpcAction(int npcId)
    {
        if(m_npcActionDict.ContainsKey(npcId))
        {
            m_npcActionDict.Remove(npcId);
        }
    }
    public NpcAction GetNpcAction(int npcId)
    {
        NpcAction npcAction = null;
        m_npcActionDict.TryGetValue(npcId, out npcAction);
        return npcAction;
    }
    #endregion

}
