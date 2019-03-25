
//////////////////////////////////////////////////////////////////
//                            _ooOoo_                           //
//                           o8888888o                          //
//                           88" . "88                          //
//                           (| -_- |)                          //
//                           O\  =  /O                          //    
//                        ____/`---'\____                       //
//                      .'  \\|     |//  `.                     //
//                     /  \\|||  :  |||//  \                    //
//                    /  _||||| -:- |||||-  \                   //
//                    |   | \\\  -  /// |   |                   //
//                    | \_|  ''\---/''  |   |                   //
//                    \  .-\__  `-`  ___/-. /                   //
//                  ___`. .'  /--.--\  `. . __                  //
//               ."" '<  `.___\_<|>_/___.'  >'"".               //
//              | | :  `- \`.;`\ _ /`;.`/ - ` : | |             //
//              \  \ `-.   \_ __\ /__ _/   .-` /  /             //
//         ======`-.____`-.___\_____/___.-`____.-'======        //
//                            `=---='                           //
//        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^         //
//           佛祖保佑 程序员一生平安,健康,快乐,没有Bug!            //
//////////////////////////////////////////////////////////////////

// ***************************************************************
// Copyright (C) 2017 The company name
//
// 文件名(File Name):             AppConfigXmlEditor.cs
// 作者(Author):                  #AuthorName#
// 创建时间(CreateTime):          #CreateDate#
// 修改者列表(modifier):
// 模块描述(Module description):
// ***************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AppConfigXmlEditor
{
    [MenuItem("Tools/Xml/生成ServerInfo.xml")]
    private static void CreateNetworkXml()
    {
        ServerInfo.SerializeServerInfo2Xml();
        Debug.Log("生成ServerInfo.xml成功.");
    }
    [MenuItem("Tools/Xml/生成StationInfo.xml", false, 2)]
    private static void CreateStationInfoXml()
    {
        StationInfoList.SerializeStationInfoList2Xml();
    }
    [MenuItem("Tools/摆点位置/点位Position配置")]
    private static void CreatePosition()
    {
        BuildPositionWindow window = (BuildPositionWindow)EditorWindow.GetWindow(typeof(BuildPositionWindow));
        window.Show();
    }
}


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
                if (index == 1)
                {
                    offset1Count++;
                    for (int j = 0; j < rowCount; ++j)
                    {
                        GameObject childGo = new GameObject(referenceGo.name);
                        childGo.transform.SetParent(parentGo.transform);
                        childGo.transform.localPosition = referenceGo.transform.localPosition + new Vector3((offsetX1 * offset1Count + offsetX2 * offset2Count), 0.0f, -offsetZ * j);
                        IconManager.SetIcon(childGo, IconManager.LabelIcon.Blue);
                        childGo.transform.localEulerAngles = new Vector3(childGo.transform.localEulerAngles.x, 180.0f, childGo.transform.localEulerAngles.z);
                    }
                }
                else if (index == 0)
                {
                    offset2Count++;
                    for (int j = 0; j < rowCount; ++j)
                    {
                        GameObject childGo = new GameObject(referenceGo.name);
                        childGo.transform.SetParent(parentGo.transform);
                        childGo.transform.localPosition = referenceGo.transform.localPosition + new Vector3((offsetX1 * offset1Count + offsetX2 * offset2Count), 0.0f, -offsetZ * j);
                        IconManager.SetIcon(childGo, IconManager.LabelIcon.Blue);
                        childGo.transform.localEulerAngles = new Vector3(childGo.transform.localEulerAngles.x, 180.0f, childGo.transform.localEulerAngles.z);
                    }
                }
            }
        }
        EditorGUILayout.EndVertical();

    }
}
