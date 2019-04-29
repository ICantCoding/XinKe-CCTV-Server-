using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ModifyPointQueueIndexWindow : EditorWindow
{
    #region 字段
    private Transform parentTrans; 
    #endregion

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        parentTrans = EditorGUILayout.ObjectField(parentTrans, typeof(Transform), true) as Transform;
        if (GUILayout.Button("修改PointQueueIndex"))
        {
            if(parentTrans != null)
            {
                int childCount = parentTrans.childCount;
                for(int i = childCount - 1; i >= 0; i--)
                {
                    Transform tempTrans = parentTrans.GetChild(i);
                    tempTrans.gameObject.name = ((childCount - 1) - i).ToString();
                    tempTrans.SetAsLastSibling();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
}
