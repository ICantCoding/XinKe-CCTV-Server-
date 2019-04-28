using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ModifyPointNameWindow : EditorWindow
{
    #region 字段
    private string newName;
    #endregion

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        newName = EditorGUILayout.TextField("NewName", newName);
        if (GUILayout.Button("改名"))
        {
            GameObject go = Selection.activeGameObject;
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform tempTrans = go.transform.GetChild(i);
                for(int j = 0; j < tempTrans.childCount; j++)
                {
                    tempTrans.GetChild(j).gameObject.name = newName;
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
}
