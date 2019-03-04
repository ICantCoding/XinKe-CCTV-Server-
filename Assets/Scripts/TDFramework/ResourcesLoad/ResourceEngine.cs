﻿
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceEngine : MonoSingleton<ResourceEngine>
    {
        #region 字段

        #endregion

        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        void Start()
        {
            
        }
        #endregion

        #region 方法
        public void InitResourceEngine(Transform poolGos, Transform sceneGos)
        {
            ResourceMgr.Instance.Init(this); //开启异步加载的协程
            ObjectManager.Instance.InitGoPool(poolGos, sceneGos); //初始化对象池数据

            //下面是异步加载或者预加载各种资源
            //1. 加载ClientOnLineItem 类对象
            // ObjectManager.Instance
        }
        #endregion
    }
}