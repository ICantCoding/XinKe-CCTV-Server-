
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//用来存放所有的玩家记录
public class WorldActor : Actor
{
    #region 字段
    //使用字典管理U3DPlayerActor, Key为PlayerActor的U3DId客户端唯一编号
    private Dictionary<UInt16, PlayerActor> m_u3dPlayerActorDict;
    //使用字典管理StationPlayerActor, Key为U3DID-StationIndex-NpcActionStatus, Value为StationPlayerActor, U3DID客户端对应站台对应Npc行为的PlayerActor
    private Dictionary<string, PlayerActor> m_stationPlayerActorDict;
    //管理有哪些U3DPlayerActor在观看某个特定的StationIndex, 实时变化
    private Dictionary<UInt16, List<PlayerActor>> m_u3dPlayerActorBelongStationDict;
    #endregion

    #region 属性
    public int OnLineCount
    {
        get { return m_u3dPlayerActorDict.Count; }
    }
    public int LinkSocketCount
    {
        get { return m_stationPlayerActorDict.Count + OnLineCount; }
    }
    #endregion

    #region 构造函数
    public WorldActor(MonoBehaviour mono) : base(mono)
    {
        m_u3dPlayerActorDict = new Dictionary<UInt16, PlayerActor>();
        m_stationPlayerActorDict = new Dictionary<string, PlayerActor>();
        m_u3dPlayerActorBelongStationDict = new Dictionary<UInt16, List<PlayerActor>>();
        //动态获取站台数量
        int count = TDFramework.SingletonMgr.GameGlobalInfo.StationInfoList.Count;
        for (UInt16 i = 0; i < count; ++i)
        {
            List<PlayerActor> list = new List<PlayerActor>();
            m_u3dPlayerActorBelongStationDict.Add(i, list);
        }
    }
    #endregion

    #region PlayerActor的数据管理
    #region U3DPlayerActor相关
    public void AddPlayerActor2U3DDict(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        if (m_u3dPlayerActorDict.ContainsKey(playerActor.U3DId) == false)
        {
            m_u3dPlayerActorDict.Add(playerActor.U3DId, playerActor);
        }
    }
    public void RemovePlayerActor4U3DDict(UInt16 u3dId)
    {
        if (m_u3dPlayerActorDict.ContainsKey(u3dId))
        {
            m_u3dPlayerActorDict.Remove(u3dId);
        }
    }
    public void RemovePlayerActor4U3DDict(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        RemovePlayerActor4U3DDict(playerActor.U3DId);
    }
    public PlayerActor GetPlayerActorByU3dId(UInt16 u3dId)
    {
        PlayerActor playerActor = null;
        m_u3dPlayerActorDict.TryGetValue(u3dId, out playerActor);
        return playerActor;
    }
    public bool PlayerActorIsExitsByU3dId(UInt16 u3dId)
    {
        return m_u3dPlayerActorDict.ContainsKey(u3dId);
    }
    #endregion

    #region StationPlayerActor相关
    public void AddStationPlayerActor2StationDict(string keyStr, PlayerActor playerActor)
    {
        if (string.IsNullOrEmpty(keyStr) || playerActor == null) return;
        if (m_stationPlayerActorDict.ContainsKey(keyStr) == false)
        {
            m_stationPlayerActorDict.Add(keyStr, playerActor);
        }
    }
    public void AddStationPlayerActor2StationDict(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        string keyStr = string.Format("{0}-{1}-{2}", playerActor.BelongU3DId, playerActor.StationIndex, playerActor.StationClientType);
        AddStationPlayerActor2StationDict(keyStr, playerActor);
    }
    public void RemoveStationPlayerActor4StationDict(string keyStr)
    {
        if(string.IsNullOrEmpty(keyStr)) return;
        if(m_stationPlayerActorDict.ContainsKey(keyStr))
        {
            m_stationPlayerActorDict.Remove(keyStr);
        }
    }
    public void RemoveStationPlayerActor4StationDict(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        string keyStr = string.Format("{0}-{1}-{2}", playerActor.BelongU3DId, playerActor.StationIndex, playerActor.StationClientType);
        RemoveStationPlayerActor4StationDict(keyStr);
    }
    public PlayerActor GetStationPlayerActor(string keyStr)
    {
        if (m_stationPlayerActorDict.ContainsKey(keyStr))
        {
            return m_stationPlayerActorDict[keyStr];
        }
        return null;
    }
    #endregion

    #region U3DPlayerActorBelongStation相关
    public void AddU3DPlayerActor2BelongStationDict(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        List<PlayerActor> list = m_u3dPlayerActorBelongStationDict[playerActor.BelongStationIndex];
        if (list.Contains(playerActor) == false)
        {
            list.Add(playerActor);
        }
    }
    public void RemoveU3DPlayerActor2BelongStationDict(UInt16 stationIndex, PlayerActor playerActor)
    {
        if (playerActor == null) return;
        List<PlayerActor> list = m_u3dPlayerActorBelongStationDict[stationIndex];
        if (list.Contains(playerActor))
        {
            list.Remove(playerActor);
        }
    }
    public void RemoveU3DPlayerActor2BelongStationDict(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        RemoveU3DPlayerActor2BelongStationDict(playerActor.BelongStationIndex, playerActor);
    }
    public List<PlayerActor> GetU3DPlayerActorListAtXXStation(UInt16 stationIndex)
    {
        return m_u3dPlayerActorBelongStationDict[stationIndex];
    }
    #endregion

    #endregion

    #region 重载方法
    protected override void ReceiveMsg(ActorMessage actorMsg)
    {
        if (!string.IsNullOrEmpty(actorMsg.msg))
        {

        }
    }
    public override void Init()
    {
        base.Init();
        m_monobehaviour.StartCoroutine(UpdateWorld()); //用于定时更新需要的信息
    }
    private IEnumerator UpdateWorld()
    {
        while (!m_isStop)
        {
            yield return new WaitForSeconds(1); //每秒定时更新一次信息
        }
    }
    #endregion

}