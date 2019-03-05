


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WatchDogActor : Actor
{
    #region 常量
    private const string CreatePlayerActorStr = "CreatePlayerActor";
    private const string DestroyPlayerActorStr = "DestroyPlayerActor";
    private const string StopSelfActorStr = "StopSelfActor";
    private const char SplitChar = '|';
    #endregion

    #region 字段
    private WorldActor m_worldActor = null;
    private ServerActor m_serverActor = null;
    #endregion

    #region 属性
    public WorldActor WorldActor
    {
        get
        {
            m_worldActor = ActorManager.Instance.GetActor<WorldActor>();
            return m_worldActor;
        }
    }
    public ServerActor ServerActor
    {
        get
        {
            m_serverActor = ActorManager.Instance.GetActor<ServerActor>();
            return m_serverActor;
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
        //将新创建的Playeractor添加到WorldActor的List集合中管理， 暂时还不要交给Dict管理(需要设置U3DID的时候才能交给Dict管理)
        WorldActor.AddPlayerActor2List(playerActor); 
    }
    private void DestroyPlayerActorCallback(uint agentId)
    {
        Agent agent = ServerActor.GetAgent(agentId);
        PlayerActor playerActor = (PlayerActor)agent.Actor;
        if (playerActor != null)
        {
            //发送Cmd，表明某个客户端下线了
            SendNotification(EventID_Cmd.U3DClientOffLine, playerActor.U3DId, null);
            ActorManager.Instance.RemoveActor(playerActor.Id);
            WorldActor.RemovePlayeractor(playerActor);
            if (ServerActor != null)
            {
                ServerActor.RemoveAgent(agent);
            }
        }
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