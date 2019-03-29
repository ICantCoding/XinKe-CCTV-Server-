using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildPositionWindow : EditorWindow
{
    #region 字段
    private GameObject referenceGo;
    private GameObject offsetGo1;
    private GameObject offsetGo2;
    private GameObject offsetGo3;
    private GameObject offsetGo4;
    private GameObject offsetGo5;
    private GameObject offsetGo6;
    private float offsetX1;
    private float offsetZ;
    private float offsetX2;

    private int colCount;
    private int rowCount;
    #endregion

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("参考点");
        referenceGo = EditorGUILayout.ObjectField(referenceGo, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("队列数");
        colCount = EditorGUILayout.IntField(colCount);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("每队列个数");
        rowCount = EditorGUILayout.IntField(rowCount);
        EditorGUILayout.EndHorizontal();

        offsetGo1 = EditorGUILayout.ObjectField(offsetGo1, typeof(GameObject), true) as GameObject;
        offsetGo2 = EditorGUILayout.ObjectField(offsetGo2, typeof(GameObject), true) as GameObject;
        if (offsetGo1 != null && offsetGo2 != null)
        {
            offsetX1 = EditorGUILayout.FloatField("相邻偏移x1:", offsetGo1.transform.localPosition.x - offsetGo2.transform.localPosition.x);
        }
        offsetGo3 = EditorGUILayout.ObjectField(offsetGo3, typeof(GameObject), true) as GameObject;
        offsetGo4 = EditorGUILayout.ObjectField(offsetGo4, typeof(GameObject), true) as GameObject;
        if (offsetGo3 != null && offsetGo4 != null)
        {
            offsetX2 = EditorGUILayout.FloatField("相邻偏移x2:", offsetGo3.transform.localPosition.x - offsetGo4.transform.localPosition.x);
        }
        offsetGo5 = EditorGUILayout.ObjectField(offsetGo5, typeof(GameObject), true) as GameObject;
        offsetGo6 = EditorGUILayout.ObjectField(offsetGo6, typeof(GameObject), true) as GameObject;
        if (offsetGo5 != null && offsetGo6 != null)
        {
            offsetZ = EditorGUILayout.FloatField("相邻偏移Z:", offsetGo5.transform.localPosition.z - offsetGo6.transform.localPosition.z);
        }

        if (GUILayout.Button("生成"))
        {
            if (referenceGo == null) return;
            string referenceGoName = referenceGo.transform.parent.name;
            int curIndex = int.Parse(referenceGoName);
            int offset1Count = 0;
            int offset2Count = 0;
            for (int i = 1; i <= colCount; ++i)
            {
                curIndex++;
                GameObject parentGo = new GameObject(curIndex.ToString());
                parentGo.transform.SetParent(referenceGo.transform.parent.parent);
                int index = i % 2;
                // if (index == 1)
                // {
                    offset1Count++;
                    for (int j = 0; j < rowCount; ++j)
                    {
                        GameObject childGo = new GameObject(referenceGo.name);
                        childGo.transform.SetParent(parentGo.transform);
                        childGo.transform.localPosition = referenceGo.transform.localPosition + new Vector3((offsetX1 * offset1Count + offsetX2 * offset2Count), 0.0f, -offsetZ * j);
                        IconManager.SetIcon(childGo, IconManager.LabelIcon.Blue);
                        childGo.transform.localEulerAngles = new Vector3(childGo.transform.localEulerAngles.x, 0.0f, childGo.transform.localEulerAngles.z);
                    }
                // }
                // else if (index == 0)
                // {
                //     offset2Count++;
                //     for (int j = 0; j < rowCount; ++j)
                //     {
                //         GameObject childGo = new GameObject(referenceGo.name);
                //         childGo.transform.SetParent(parentGo.transform);
                //         childGo.transform.localPosition = referenceGo.transform.localPosition + new Vector3((offsetX1 * offset1Count + offsetX2 * offset2Count), 0.0f, -offsetZ * j);
                //         IconManager.SetIcon(childGo, IconManager.LabelIcon.Blue);
                //         childGo.transform.localEulerAngles = new Vector3(childGo.transform.localEulerAngles.x, 0.0f, childGo.transform.localEulerAngles.z);
                //     }
                // }
            }
        }
        EditorGUILayout.EndVertical();

    }
}
