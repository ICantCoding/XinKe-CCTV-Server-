
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//用来存放所有的玩家记录
public class WorldActor : Actor
{
    #region 字段
    //使用字典管理PlayerActor, Key为PlayerActor的U3DId客户端唯一编号
    private Dictionary<UInt16, PlayerActor> m_u3dPlayerActorDict =
        new Dictionary<UInt16, PlayerActor>();
    //使用字典管理PlayerActor, 用于管理Station的Client, 第一个Key为StationIndex， 第二个Key表示StationSocketType
    private Dictionary<UInt16, Dictionary<UInt16, List<PlayerActor>>> m_stationPlayerActorDict =
        new Dictionary<UInt16, Dictionary<UInt16, List<PlayerActor>>>();
    #endregion

    #region 属性
    public int OnLineCount
    {
        get { return m_u3dPlayerActorDict.Count; }
    }
    public int LinkSocketCount
    {
        get
        {
            int count = 0;
            var enumerator = m_stationPlayerActorDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current.Value;
                var enumerator2 = item.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    var item2 = enumerator2.Current.Value;
                    count += item2.Count;
                }
                enumerator2.Dispose();
            }
            enumerator.Dispose();
            return count + OnLineCount;
        }
    }
    #endregion

    #region 构造函数
    public WorldActor(MonoBehaviour mono) : base(mono)
    {

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
    public void AddPlayerActor2StationDict(UInt16 stationIndex, UInt16 stationSocketType, PlayerActor playerActor)
    {
        if (playerActor == null) return;
        Dictionary<UInt16, List<PlayerActor>> playerActorDict = null;
        List<PlayerActor> list = null;
        if (m_stationPlayerActorDict.ContainsKey(stationIndex) == false)
        {
            list = new List<PlayerActor>();
            playerActorDict = new Dictionary<UInt16, List<PlayerActor>>();
            playerActorDict.Add(stationSocketType, list);
            m_stationPlayerActorDict.Add(stationIndex, playerActorDict);
        }
        else
        {
            playerActorDict = m_stationPlayerActorDict[stationIndex];
            if(playerActorDict.ContainsKey(stationSocketType))
            {
                list = playerActorDict[stationSocketType];
            }
            else
            {
                list = new List<PlayerActor>();
                playerActorDict.Add(stationSocketType, list);
            }
        }
        list.Add(playerActor);
    }
    public void RemovePlayerActor4StationDict(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        Dictionary<UInt16, List<PlayerActor>> playerActorDict = null;
        List<PlayerActor> list = null;
        if (m_stationPlayerActorDict.ContainsKey(playerActor.StationIndex) == true)
        {
            playerActorDict = m_stationPlayerActorDict[playerActor.StationIndex];
            if (playerActorDict.ContainsKey(playerActor.StationClientType) == true)
            {
                list = playerActorDict[playerActor.StationClientType];
                if (list != null && list.Contains(playerActor) == true)
                {
                    list.Remove(playerActor);
                }
            }
        }
    }
    public List<PlayerActor> GetPlayerActorByStationIndexAndStationClientType(UInt16 stationIndex, UInt16 stationClientType)
    {
        if(m_stationPlayerActorDict.ContainsKey(stationIndex))
        {
            if(m_stationPlayerActorDict[stationIndex].ContainsKey(stationClientType))
            {
                return m_stationPlayerActorDict[stationIndex][stationClientType];
            }
        }
        return null;
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