using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildPingBiMenWindow : EditorWindow
{
    #region 字段
    private Transform referenceTrans;
    private GameObject pingBiMenPrefab;
    private Transform parentTrans;
    #endregion

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("参考点:");
        referenceTrans = EditorGUILayout.ObjectField(referenceTrans, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("屏蔽门预制件");
        pingBiMenPrefab = EditorGUILayout.ObjectField(pingBiMenPrefab, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        parentTrans = EditorGUILayout.ObjectField(parentTrans, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();

        if(GUILayout.Button("生成屏蔽门"))
        {
            int count = referenceTrans.childCount;
            for(int i = 0; i < count; ++i)
            {
                GameObject pingBiMen = GameObject.Instantiate(pingBiMenPrefab);
                pingBiMen.transform.SetParent(parentTrans);
                pingBiMen.transform.position = referenceTrans.GetChild(i).transform.position;
            }
        }

        EditorGUILayout.EndHorizontal();
    }
}
