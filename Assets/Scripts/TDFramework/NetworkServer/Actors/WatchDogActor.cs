


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WatchDogActor : Actor
{
    #region 常量
    private const string CreatePlayerActorStr = "CreatePlayerActor";
    private const string DestroyPlayerActorStr = "DestroyPlayerActor";
    private const char SplitChar = '|';
    #endregion

    #region 字段
    private WorldActor m_worldActor = null;
    #endregion

    #region 属性
    public WorldActor WorldActor
    {
        get
        {
            if (m_worldActor == null)
            {
                m_worldActor = ActorManager.Instance.GetActor<WorldActor>();
            }
            return m_worldActor;
        }
    }
    #endregion

    #region 构造函数
    public WatchDogActor(MonoBehaviour mono) : base(mono)
    {
        
    }
    #endregion

    #region 重写
    protected override void ReceiveMsg(ActorMessage actorMsg)
    {
        if (!string.IsNullOrEmpty(actorMsg.msg))
        {
            var cmds = actorMsg.msg.Split(SplitChar);
            if (cmds[0] == CreatePlayerActorStr)
            {
                var agentId = System.Convert.ToUInt32(cmds[1]);
                CreatePlayerActorCallback(agentId);
            }
            else if (cmds[0] == DestroyPlayerActorStr)
            {
                var agentId = System.Convert.ToUInt32(cmds[1]);
                DestroyPlayerActorCallback(agentId); //销毁PlayerActor
            }
        }
    }
    #endregion

    #region 方法
    private void CreatePlayerActorCallback(uint agentId)
    {
        var playerActor = new PlayerActor(agentId, m_monobehaviour); //创建PlayerActor
        ActorManager.Instance.AddActor(playerActor);
        WorldActor.AddPlayerActor(playerActor); //将新创建的PlayerActor添加到WorldActor进行管理
    }
    private void DestroyPlayerActorCallback(uint agentId)
    {
        WorldActor.RemovePlayerActor(agentId);
    }
    #endregion

    #region 看门狗发送特定的消息
    public void SendActorMessageToCreatePlayerActor(uint agentId)
    {
        SendMsg(string.Format("{0}|{1}", CreatePlayerActorStr, agentId));
    }
    public void SendActorMessageToDestroyPlayerActor(uint agentId)
    {
        SendMsg(string.Format("{0}|{1}", DestroyPlayerActorStr, agentId));
    }
    #endregion
}