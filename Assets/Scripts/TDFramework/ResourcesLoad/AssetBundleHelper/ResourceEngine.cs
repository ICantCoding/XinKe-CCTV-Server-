
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
        public void InitResourceEngine()
        {
            Transform poolGoTrans = new GameObject(GameTagMgr.PoolGos_Tag).transform;
            poolGoTrans.gameObject.SetActive(false);
            poolGoTrans.localPosition = new Vector3(-9.4f, -14.995f, -18.56f);
            GameObject.DontDestroyOnLoad(poolGoTrans.gameObject);
            Transform sceneGoTrans = new GameObject(GameTagMgr.SceneGos_Tag).transform;
            sceneGoTrans.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(sceneGoTrans.gameObject);

            //初始化对象池数据
            ObjectManager.Instance.InitGoPool(poolGoTrans, sceneGoTrans); 
            //预加载第一种男Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Man1_Npc.prefab", 160, false);
            //预加载第二种男Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Man2_Npc.prefab", 160, false);
            //预加载第三种男Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Man3_Npc.prefab", 160, false);
            //预加载第四种男Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Man4_Npc.prefab", 160, false);
            //预加载第五种男Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Man5_Npc.prefab", 160, false);
            //预加载第一种女Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Woman1_Npc.prefab", 160, false);
            //预加载第二种女Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Woman2_Npc.prefab", 160, false);
            //预加载第三种女Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Woman3_Npc.prefab", 160, false);
            //预加载第四种女Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Woman4_Npc.prefab", 160, false);
            //预加载第五种女Npc
            ObjectManager.Instance.PreloadGameObject("Assets/Prefabs/Npc/Woman5_Npc.prefab", 160, false);
        }
        #endregion
    }
}