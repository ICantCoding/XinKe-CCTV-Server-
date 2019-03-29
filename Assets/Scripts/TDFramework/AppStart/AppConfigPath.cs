
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
// 文件名(File Name):             AppConfigPath.cs
// 作者(Author):                  #AuthorName#
// 创建时间(CreateTime):          #CreateDate#
// 修改者列表(modifier):
// 模块描述(Module description):
// ***************************************************************

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppConfigPath
    {
        //网络配置xml文件
        public static string ServerInfoXmlPath = Application.streamingAssetsPath + "/Xml/ServerInfo.xml";
        //站台配置xml文件
        public static string StationInfoXmlPath = Application.streamingAssetsPath + "/Xml/StationInfo.xml";
        //站台闸机配置xml文件
        public static string Station0ZhaJiXmlPath = Application.streamingAssetsPath + "/Xml/DeviceInfo/Station0ZhaJiDeviceInfo.xml";
        public static string Station1ZhaJiXmlPath = Application.streamingAssetsPath + "/Xml/DeviceInfo/Station1ZhaJiDeviceInfo.xml";
        //站台屏蔽门配置xml文件
        public static string Station0PingBiMenXmlPath = Application.streamingAssetsPath + "/Xml/DeviceInfo/Station0PingBiMenDeviceInfo.xml";
        public static string Station1PingBiMenXmlPath = Application.streamingAssetsPath + "/Xml/DeviceInfo/Station1PingBiMenDeviceInfo.xml";
    }
}