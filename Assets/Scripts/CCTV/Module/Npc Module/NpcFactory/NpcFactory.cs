using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;

//Npc工厂，用于生成Npc
public class NpcFactory
{
    #region 字段
    private static NpcType[] NpcTypeList = new NpcType[10]
    {
        NpcType.Man1,
        NpcType.Man2,
        NpcType.Man3,
        NpcType.Man4,
        NpcType.Man5,
        NpcType.Woman1,
        NpcType.Woman2,
        NpcType.Woman3,
        NpcType.Woman4,
        NpcType.Woman5
    };
    #endregion

    public GameObject CreateNpc(ref NpcType npcType)
    {
        //任务模型随机, 从10种模型中随机选择一种
        int randomNpcModel = UnityEngine.Random.Range(0, 10);
        npcType = NpcTypeList[randomNpcModel];
        string modleName = StringMgr.NpcModelNameList[randomNpcModel];
        GameObject npcGo = ObjectManager.Instance.Instantiate(modleName);
        if(npcGo == null) return null;
        return npcGo;
    }
    public void ReleaseNpc(GameObject npcGo)
    {
        if(npcGo == null)
        {
            return;
        }
        ObjectManager.Instance.ReleaseGameObjectItem(npcGo);
    }
}
