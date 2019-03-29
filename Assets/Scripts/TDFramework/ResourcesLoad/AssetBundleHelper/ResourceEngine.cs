
namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceEngine : MonoBehaviour
    {
        #region Unity生命周期
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion

        #region 方法
        public void InitResourceEngine(Transform poolGos, Transform sceneGos)
        {
            //初始化对象池数据
            ObjectManager.Instance.InitGoPool(poolGos, sceneGos); 
            //预加载第一种Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/NpcMan 1.prefab", 100, false);
            //预加载第二种Npc

        }
        #endregion
    }
}