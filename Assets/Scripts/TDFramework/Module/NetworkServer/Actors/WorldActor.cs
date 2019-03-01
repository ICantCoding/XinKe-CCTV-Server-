


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//用来存放所有的玩家记录
public class WorldActor : Actor
{
    #region 字段
    //世界WorldActor中保存管理了PlayerActor
    private List<PlayerActor> m_playerActorList = new List<PlayerActor>();
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
    }
    public void RemovePlayerActor(PlayerActor playerActor)
    {
        if (playerActor == null) return;
        if (m_playerActorList.Contains(playerActor))
        {
            m_playerActorList.Remove(playerActor);
        }
    }
    public void RemovePlayerActor(uint agentId)
    {
        PlayerActor actor = GetPlayerActor(agentId);
        if (actor != null)
        {
            RemovePlayerActor(actor);
        }
    }
    public PlayerActor GetPlayerActor(uint agentId)
    {
        foreach (var temp in m_playerActorList)
        {
            if (temp.Id == agentId)
            {
                return temp;
            }
        }
        return null;
    }
    #endregion

    #endregion

    #region 重载方法
    protected override void ReceiveMsg(ActorMessage msg)
    {

    }
    public override void Init()
    {
        base.Init();
        m_monobehaviour.StartCoroutine(UpdateWorld()); //用于定时更新需要的信息
    }
    #endregion

}