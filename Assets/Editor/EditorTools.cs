﻿
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
    [MenuItem("Tools/Xml/生成ServerInfo.xml", false, 1)]
    private static void CreateNetworkXml()
    {
        ServerInfo.SerializeServerInfo2Xml();
        Debug.Log("生成ServerInfo.xml成功.");
        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/Xml/生成StationInfo.xml", false, 2)]
    private static void CreateStationInfoXml()
    {
        StationInfoList.SerializeStationInfoList2Xml();
        Debug.Log("生成StationInfo.xml成功.");
        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/Xml/生成DeviceInfo/站台0/闸机", false, 1)]
    private static void CreateZhaJiDeviceInfoXmlAboutStation0()
    {
        ZhaJiDeviceInfoCollection.SerializedZhaJiDeviceInfoCollectionAtStation0();
    }
    [MenuItem("Tools/Xml/生成DeviceInfo/站台0/屏蔽门", false, 2)]
    private static void CreatePingBiMenDeviceInfoXmlAboutStation0()
    {
        PingBiMenDeviceInfoCollection.SerializedPingBiMenDeviceInfoCollectionAtStation0();
    }
    [MenuItem("Tools/Xml/生成Device和Point关系DeviceAndPoint.xml", false, 3)]
    private static void CreateDeviceAndPointInfoXml()
    {
        DeviceAndPointInfo.SerializationDeviceAndPointInfo2Xml();
        Debug.Log("生成Device和Point位置点关系xml成功.");
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/摆点位置/点位Position配置")]
    private static void CreatePosition()
    {
        BuildPositionWindow window = (BuildPositionWindow)EditorWindow.GetWindow(typeof(BuildPositionWindow));
        window.Show();
    }
    [MenuItem("Tools/摆点位置/放置屏蔽门")]
    private static void CreatePingBiMen()
    {
        BuildPingBiMenWindow window = (BuildPingBiMenWindow)EditorWindow.GetWindow(typeof(BuildPingBiMenWindow));
        window.Show();
    }
    [MenuItem("Tools/摆位置点/修改上下行点名称")]
    private static void ModifyPointName()
    {
        ModifyPointNameWindow window = (ModifyPointNameWindow)EditorWindow.GetWindow(typeof(ModifyPointNameWindow));
        window.Show();
    }
    [MenuItem("Tools/摆位置点/修改子物体坐标 %1", false, 2)]
    private static void ModifyChildName()
    {
        GameObject[] xxs = Selection.gameObjects;
        for (int k = 0; k < xxs.Length; k++)
        {
            GameObject go = xxs[k];
            List<GameObject> gos = new List<GameObject>();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject tempGo = go.transform.GetChild(i).gameObject;
                gos.Add(tempGo);
            }
            for (int i = 0; i < gos.Count; i++)
            {
                gos[i].transform.SetParent(go.transform.parent);
            }
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            for (int i = 0; i < gos.Count; i++)
            {
                gos[i].transform.SetParent(go.transform);
            }
        }
    }
    [MenuItem("Tools/摆位置点/修改PointQueueIndex顺序", false, 3)]
    private static void ModifyPointQueueIndex()
    {
        ModifyPointQueueIndexWindow window = (ModifyPointQueueIndexWindow)EditorWindow.GetWindow(typeof(ModifyPointQueueIndexWindow));
        window.Show();
    }
}


