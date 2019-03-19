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

public class NpcMgr : MonoBehaviour
{

    #region 单例
    private static NpcMgr m_instance;
    public static NpcMgr Instance
    {
        get { return m_instance; }
    }
    #endregion

    #region 状态字段
	//用于Npc自增标识
    private static int m_startNpcId = 0;
    #endregion

    #region 游戏物体对象
    public Transform Npc;
    #endregion

    #region 字段
	//List管理NpcAction
    public List<NpcAction> m_npcActionList = new List<NpcAction>();
	//Dictionary管理NpcAction
    public Dictionary<int, NpcAction> m_npcActionDict = new Dictionary<int, NpcAction>();
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_instance = this;
        int count = Npc.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform trans = Npc.GetChild(i);
            NpcAction npcAction = trans.GetComponent<NpcAction>();
            npcAction.m_npcId = ++m_startNpcId;
            m_npcActionList.Add(npcAction);
            m_npcActionDict.Add(npcAction.m_npcId, npcAction);
        }
    }
    #endregion

    #region 方法
    public NpcAction GetNpcAction(int npcId)
    {
        NpcAction npcAction = null;
        m_npcActionDict.TryGetValue(npcId, out npcAction);
        return npcAction;
    }
    #endregion

}
