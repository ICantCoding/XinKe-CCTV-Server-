
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourcesModule : IModule
    {
        #region 抽象方法实现
        public override void Init()
        {
            Transform poolGos = GameObject.FindGameObjectWithTag(GameTags.PoolGos_Tag).transform;
            Transform sceneGos = GameObject.FindGameObjectWithTag(GameTags.SceneGos_Tag).transform;
            if (poolGos != null && sceneGos != null)
            {
                ResourceEngine.Instance.InitResourceEngine(poolGos, sceneGos); //初始化资源加载引擎.
            }
        }
        public override void Release()
        {
            //释放资源加载引擎需要做些什么啦？
            
        }
        #endregion
    }
}