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
        "Assets/Prefabs/Npc/NpcMan 1.prefab",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10"
    };
    #endregion

    public GameObject CreateNpc()
    {
        //任务模型随机, 从10种模型中随机选择一种
        int randomNpcModel = UnityEngine.Random.Range(0, 10);
        string modleName = NpcModelNameList[0];
        GameObject npcGo = ObjectManager.Instance.Instantiate(modleName);
        if(npcGo == null) return null;
        return npcGo;
    }





    #region 私有

    #endregion
}
