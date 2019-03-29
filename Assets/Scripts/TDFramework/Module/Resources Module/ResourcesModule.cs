
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourcesModule : IModule
    {
        #region 字段
        private ResourceEngine m_resourceEngine;
        #endregion

        #region 抽象方法实现
        public override void Init()
        {
            Transform poolGoTrans = new GameObject(GameTagMgr.PoolGos_Tag).transform;
            poolGoTrans.localPosition = new Vector3(-9.4f, -14.995f, -18.56f);
            GameObject.DontDestroyOnLoad(poolGoTrans.gameObject);
            Transform sceneGoTrans = new GameObject(GameTagMgr.SceneGos_Tag).transform;
            GameObject.DontDestroyOnLoad(sceneGoTrans.gameObject);
            string goName = StringMgr.ResourceEngineName;
            GameObject resourceEngineGo = GameObject.Find(goName);
            if(resourceEngineGo == null)
            {
                resourceEngineGo = new GameObject(goName);
            }
            m_resourceEngine = resourceEngineGo.AddComponent<ResourceEngine>();
            m_resourceEngine.InitResourceEngine(poolGoTrans, sceneGoTrans);
        }
        public override void Release()
        {
            //释放资源加载引擎需要做些什么啦？
            
        }
        #endregion
    }
}