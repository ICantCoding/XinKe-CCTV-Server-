using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

//Npc工厂，用于生成Npc
public class NpcFactory
{
    #region 字段
    private static string[] NpcModelNameList = new string[10]
    {
        "Assets/Prefabs/Npc/Man1_Npc.prefab",
        "Assets/Prefabs/Npc/Man2_Npc.prefab",
        "Assets/Prefabs/Npc/Man3_Npc.prefab",
        "Assets/Prefabs/Npc/Man4_Npc.prefab",
        "Assets/Prefabs/Npc/Man5_Npc.prefab",
        "Assets/Prefabs/Npc/Woman1_Npc.prefab",
        "Assets/Prefabs/Npc/Woman2_Npc.prefab",
        "Assets/Prefabs/Npc/Woman3_Npc.prefab",
        "Assets/Prefabs/Npc/Woman4_Npc.prefab",
        "Assets/Prefabs/Npc/Woman5_Npc.prefab"
    };
    #endregion

    public GameObject CreateNpc()
    {
        //任务模型随机, 从10种模型中随机选择一种
        int randomNpcModel = UnityEngine.Random.Range(0, 10);
        string modleName = NpcModelNameList[randomNpcModel];
        GameObject npcGo = ObjectManager.Instance.Instantiate(modleName);
        if(npcGo == null) return null;
        return npcGo;
    }
}
