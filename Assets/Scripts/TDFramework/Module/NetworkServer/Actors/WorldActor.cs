
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
            // Debug.Log("世界信息每隔1秒更新...");
        }
    }

    #region PlayerActor的数据管理
    public void AddPlayerActor2List(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        m_playerActorList.Add(playerActor);
    }
    public void AddPlayerActor2Dict(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        if(m_playerActorDict.ContainsKey(playerActor.U3DId) == false)
        {
            m_playerActorDict.Add(playerActor.U3DId, playerActor);
        }
    }
    public void RemovePlayerActor4List(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        RemovePlayerActor4List(playerActor.Id);
    }
    public void RemovePlayerActor4List(int id)
    {
        PlayerActor tempPlayerActor = null;
        foreach(PlayerActor item in m_playerActorList)
        {
            if(item.Id == id)
            {
                tempPlayerActor = item;
                break;
            }
        }
        if(tempPlayerActor != null)
        {
            m_playerActorList.Remove(tempPlayerActor);
        }
    }
    public void RemovePlayerActor4Dict(PlayerActor playerActor)
    {
        if(playerActor == null) return;
        RemovePlayerActor4Dict(playerActor.U3DId);
    }
    public void RemovePlayerActor4Dict(UInt16 u3dId)
    {
        if(m_playerActorDict.ContainsKey(u3dId))
        {
            m_playerActorDict.Remove(u3dId);
        }
    }
    public void RemovePlayeractor(PlayerActor playerActor)
    {
        RemovePlayerActor4List(playerActor);
        RemovePlayerActor4Dict(playerActor);
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
    public bool PlayerActorIsExitsByU3dId(UInt16 u3dId)
    {
        return m_playerActorDict.ContainsKey(u3dId);
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