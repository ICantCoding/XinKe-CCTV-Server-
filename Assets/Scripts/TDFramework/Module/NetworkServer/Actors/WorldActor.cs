
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//用来存放所有的玩家记录
public class WorldActor : Actor
{
    #region 常量

    #endregion

    #region 字段
    //世界WorldActor中保存管理了PlayerActor
    private List<PlayerActor> m_playerActorList = new List<PlayerActor>();
    //使用字典管理PlayerActor, Key为PlayerActor的U3DId客户端唯一编号
    private Dictionary<UInt16, PlayerActor> m_playerActorDict = new Dictionary<UInt16, PlayerActor>();
    #endregion

    #region 构造函数
    public WorldActor(MonoBehaviour mono) : base(mono)
    {

    }
    #endregion

    #region 方法
    private IEnumerator UpdateWorld()
    {
        while (!m_isStop)
        {
            yield return new WaitForSeconds(1); //每秒定时更新一次信息
            Debug.Log("世界信息每隔1秒更新...");
        }
    }

    #region PlayerActor的数据管理
    public void AddPlayerActor(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        if (m_playerActorList.Contains(playerActor) == false)
        {
            m_playerActorList.Add(playerActor);
        }
        if (m_playerActorDict.ContainsKey(playerActor.U3DId) == false)
        {
            m_playerActorDict.Add(playerActor.U3DId, playerActor);
        }
    }
    public void RemovePlayerActor(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        if (m_playerActorList.Contains(playerActor))
        {
            m_playerActorList.Remove(playerActor);
        }
        if (m_playerActorDict.ContainsKey(playerActor.U3DId))
        {
            m_playerActorDict.Remove(playerActor.U3DId);
        }
    }
    public void RemovePlayerActorByActorId(uint actorId)
    {
        PlayerActor actor = GetPlayerActorByActorId(actorId);
        if (actor != null)
        {
            RemovePlayerActor(actor);
        }
    }
    public void RemovePlayerActorByU3dId(UInt16 u3dId)
    {
        PlayerActor actor = GetPlayerActorByU3dId(u3dId);
        if(actor != null)
        {
            RemovePlayerActor(actor);
        }
    }
    public PlayerActor GetPlayerActorByActorId(uint actorId)
    {
        foreach (var temp in m_playerActorList)
        {
            if (temp.Id == actorId)
            {
                return temp;
            }
        }
        return null;
    }
    public PlayerActor GetPlayerActorByU3dId(UInt16 u3dId)
    {
        PlayerActor playerActor = null;
        m_playerActorDict.TryGetValue(u3dId, out playerActor);
        return playerActor;
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
    #endregion
}