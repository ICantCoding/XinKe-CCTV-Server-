﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ServerActor : Actor
{
    #region 字段
    private TcpListener m_tcpListener = null;
    //tcp服务器端口号
    private int m_tcpPort = 0;
    //监听客户端连接的子线程
    private Thread m_thread = null;
    //对Agent使用字典管理, Key为Agent的Id, Value为Agent对象
    private Dictionary<uint, Agent> m_agentDict = new Dictionary<uint, Agent>();
    private WatchDogActor m_dogActor; //看门狗Actor
    #endregion

    #region 属性
    public Thread ListenerThread
    {
        get { return m_thread; }
    }
    #endregion

    #region 构造函数
    public ServerActor(MonoBehaviour mono) : base(mono)
    {
        m_dogActor = ActorManager.Instance.GetActor<WatchDogActor>();
    }
    #endregion

    #region 方法
    public bool Start(int tcpPort)
    {
        try
        {
            m_tcpPort = tcpPort;
            m_tcpListener = new TcpListener(IPAddress.Any, m_tcpPort);
            m_tcpListener.Start(50); //设置最大挂载连接数为50
        }
        catch (System.Exception e)
        {
            return false;
        }
        m_thread = new Thread(new ThreadStart(ListenClientConnectedThreadFunction));
        m_thread.IsBackground = true;
        m_thread.Start(); //开启一个子线程专门用于监听客户端的连接
        return true;
    }
    public void Close()
    {
        //关闭TcpListener
        if (m_tcpListener != null)
        {
            m_tcpListener.Stop();
            m_tcpListener = null;
        }
        //中断子线程
        try
        {
            if (m_thread != null)
            {
                m_thread.Abort();
            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
    }
    #region 数据管理方法
    public void AddAgent(Socket socket)
    {
        Agent agent = new Agent(socket);
        agent.DogActor = m_dogActor; //设置Agent的看门狗Actor
        agent.ServerActor = this;
        lock (m_agentDict)
        {
            m_agentDict.Add(agent.Id, agent);
        }
        agent.StartReceive();
    }
    public void RemoveAgent(Agent agent)
    {
        if (m_agentDict.ContainsKey(agent.Id))
        {
            lock (m_agentDict)
            {
                m_agentDict.Remove(agent.Id);
            }
        }
    }
    public Agent GetAgent(uint agentId)
    {
        Agent agent = null;
        lock (m_agentDict)
        {
            m_agentDict.TryGetValue(agentId, out agent);
        }
        return agent;
    }
    #endregion
    #endregion

    #region 子线程方法
    private void ListenClientConnectedThreadFunction()
    {
        while (true)
        {
            //确定是否有挂起的连接请求, Pending()返回true, 表示有从客户端来的连接请求
            if (m_tcpListener != null && m_tcpListener.Pending())
            {
                Socket socket = m_tcpListener.AcceptSocket();
                AddAgent(socket);
            }
            Thread.Sleep(1);
        }
    }
    #endregion
}
