
/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-07 16:24:06

** 功能描述:  Npc的管理器

** version:   v1.2.0

*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMgr
{
    #region 锁
    // private readonly object locker = new object();
    #endregion

    #region 游戏物体对象
    public Transform NpcParentTransform;
    #endregion

    #region 字段
    private NpcActionStatus m_npcActionStatus = NpcActionStatus.None;
    private Dictionary<int, NpcAction> m_npcActionDict = new Dictionary<int, NpcAction>();
    #endregion

    #region 属性
    public int Count
    {
        get { return m_npcActionDict.Count; }
    }
    public NpcActionStatus NpcActionStatus
    {
        get { return m_npcActionStatus; }
        set { m_npcActionStatus = value; }
    }
    public Dictionary<int, NpcAction> NpcActionDict
    {
        get { return m_npcActionDict; }
    }
    #endregion

    #region 方法
    public void AddNpcAction(NpcAction npcAction)
    {
        if (npcAction == null) return;
        npcAction.NpcId = System.Threading.Interlocked.Increment(ref StationEngine.StartNpcId);
        if (m_npcActionDict.ContainsKey(npcAction.NpcId) == false)
        {
            m_npcActionDict.Add(npcAction.NpcId, npcAction);
        }
    }
    public void RemoveNpcAction(int npcId)
    {
        if (m_npcActionDict.ContainsKey(npcId))
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
    //同步Npc信息
    public void SyncNpcInfo(PlayerActor stationPlayerActor)
    {
        float posX, posY, posZ, angleX, angleY, angleZ = 0.0f;
        int npcId;
        int npcType;
        List<PlayerActor> stationPlayerActorList = new List<PlayerActor>();

        var enumerator = m_npcActionDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            NpcAction npcAction = enumerator.Current.Value;
            Transform npcTransform = npcAction.transform;
            posX = npcTransform.localPosition.x;
            posY = npcTransform.localPosition.y;
            posZ = npcTransform.localPosition.z;
            angleX = npcTransform.localEulerAngles.x;
            angleY = npcTransform.localEulerAngles.y;
            angleZ = npcTransform.localEulerAngles.z;
            npcId = npcAction.NpcId;
            npcType = (int)npcAction.NpcType;
            GameGlobalComponent.NpcSync.SendNpcPositionRelink(stationPlayerActor, posX, posY, posZ, angleX, angleY, angleZ, npcId, npcType,
                npcAction.StationIndex, (System.UInt16)npcAction.NpcActionStatus);
        }
        enumerator.Dispose();
    }
    public void SyncNpcInfo()
    {
        //功能实现方式，是给Npc设置同步开关，打开开关，Npc主动向客户端发送Npc的消息，关闭开关，则只有Npc产生位移或动画时才发送Npc消息
        var enumerator = m_npcActionDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            NpcAction npcAction = enumerator.Current.Value;
            if(npcAction != null)
                npcAction.ClientReconnectFlag = true;
        }
        enumerator.Dispose();
    }
    #endregion

}
