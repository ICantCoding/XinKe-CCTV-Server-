

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using PureMVC.Patterns.Observer;
using UnityEngine;

public class ActorMessage
{
    public string msg;          //ActorMessage可携带string消息
    public Packet packet;       //ActorMessage可携带Packet
    public object obj;          //ActorMessage可携带一个object参数
    public object obj1;         //ActorMessage可携带第二个object参数
}

public class Actor
{
    #region 字段
    protected object m_lockobj = new object(); //锁，用于Queue<ActorMessage>队列操作控制
    protected int m_Id; //地址
    protected bool m_isStop = false; //停止
    protected MonoBehaviour m_monobehaviour = null;
    protected Queue<ActorMessage> m_msgQueue;

    protected Notifier m_puremvcNotifier; //组合关系，让Actor具有发送PureMVC消息的能力
    #endregion

    #region 属性
    public int Id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }
    #endregion

    #region 构造函数
    public Actor(MonoBehaviour mono)
    {
        this.m_monobehaviour = mono;
        m_msgQueue = new Queue<ActorMessage>();
        m_puremvcNotifier = new Notifier();
    }
    #endregion

    //分发消息，Agent通过Agent的Actor发送消息出来，该Actor就接收到这个消息, 由Actor的ReceiveMsg来处理.
    private IEnumerator /*void*/ Dispatch()
    {
        while (!m_isStop)
        {
            if (m_msgQueue.Count > 0)
            {
                ActorMessage msg = null;
                lock (m_lockobj)
                {
                    msg = m_msgQueue.Dequeue();
                }
                ReceiveMsg(msg);
            }
            else
            {
                // Thread.Sleep(1);
                yield return null;
            }
        }
    }
    //虚方法，用于子类重写
    protected virtual void ReceiveMsg(ActorMessage msg)
    {

    }
    public virtual void Init()
    {
        //这里使用协程，还是线程？ 需要考虑比较
        m_monobehaviour.StartCoroutine(Dispatch()); //开启携程
        // Thread thread = new Thread(Dispatch);
        // thread.Start();
    }
    public void Stop()
    {
        m_isStop = true;
    }

    #region Actor之间通信
    //Actor给Actor发送string类型的信息
    public void SendMsg(string msg)
    {
        var m = new ActorMessage();
        m.msg = msg;
        lock (m_lockobj)
        {
            m_msgQueue.Enqueue(m);
        }
    }
    //Actor给Actor发送Packet类型的信息
    public void SendMsg(Packet packet)
    {
        var m = new ActorMessage() { packet = packet };
        lock (m_lockobj)
        {
            m_msgQueue.Enqueue(m);
        }
    }
    //Actor给Actor发送ActorMessage类型的信心
    public void SendMsg(ActorMessage msg)
    {
        lock (m_lockobj)
        {
            m_msgQueue.Enqueue(msg);
        }
    }
    #endregion

    #region Notifier发送PureMVC实现
    //PureMVC发送通知
    public void SendNotification(string notificationName, object body, string type)
    {
        if (m_puremvcNotifier != null)
        {
            m_puremvcNotifier.SendNotification(notificationName, body, type);
        }
    }
    #endregion
}

public class ActorManager
{
    #region 字段
    public static ActorManager Instance;
    Dictionary<int, Actor> m_actorDict; //根据Actor的Id来缓存Actor
    Dictionary<Type, Actor> m_actorType; //根据Actor的Type来缓存Actor

    private int m_actorId = 0; //进行原子操作递增， 用于Actor的唯一标识
    private bool m_isStop = false;
    #endregion

    #region 构造函数
    public ActorManager()
    {
        Instance = this;
        m_actorDict = new Dictionary<int, Actor>();
        m_actorType = new Dictionary<Type, Actor>();
    }
    #endregion

    #region 方法
    public int AddActor(Actor actor, bool addType = false)
    {
        if (m_isStop)
        {
            return -1;
        }
        int actorId = Interlocked.Increment(ref m_actorId); //原子操作
        lock (m_actorDict)
        {
            m_actorDict.Add(actorId, actor);
            if (addType)
            {
                m_actorType.Add(actor.GetType(), actor);
            }
        }
        actor.Id = actorId;
        actor.Init();       //向ActorManager添加Actor的时候，就初始化Actor，并启动Actor的任务
        return actorId;
    }
    public void RemoveActor(int actorId)
    {
#warning RemoveActor的时候是否需要关闭协程. 待尝试
        lock (m_actorDict)
        {
            if (m_actorDict.ContainsKey(actorId))
            {
                Actor actor = m_actorDict[actorId];
                if (m_actorType.ContainsKey(actor.GetType()))
                {
                    Actor actor2 = m_actorType[actor.GetType()];
                    if (actor2 == actor)
                    {
                        m_actorType.Remove(actor.GetType());
                    }
                }
                m_actorDict.Remove(actorId);
            }
        }
    }
    public Actor GetActor(int actorId)
    {
        Actor actor = null;
        lock (m_actorDict)
        {
            m_actorDict.TryGetValue(actorId, out actor);
        }
        return actor;
    }
    public T GetActor<T>() where T : Actor
    {
        T actor = null;
        lock (m_actorDict)
        {
            Actor temp = null;
            m_actorType.TryGetValue(typeof(T), out temp);
            actor = (T)temp;
        }
        return actor;
    }
    public void Stop()
    {
        m_isStop = true;
        lock (m_actorDict)
        {
            foreach (var actor in m_actorDict)
            {
                actor.Value.Stop();
            }
        }
    }
    #endregion
}