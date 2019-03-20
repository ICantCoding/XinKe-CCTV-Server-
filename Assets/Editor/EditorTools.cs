
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

    [MenuItem("Tools/Navigation/Y")]
    private static void Y()
    {
        GameObject[] gos = Selection.gameObjects;
        for(int i = 0; i < gos.Length; i++)
        {
            GameObject go = gos[i];
            Vector3 pos = go.transform.localPosition;
            go.transform.localPosition = new Vector3(pos.x, pos.y - 0.1856f, pos.z);
        }
    }
}
