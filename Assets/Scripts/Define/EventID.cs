
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
// 文件名(File Name):             EventID.cs
// 作者(Author):                  #AuthorName#
// 创建时间(CreateTime):          #CreateDate#
// 修改者列表(modifier):
// 模块描述(Module description):
// ***************************************************************


using System;
using System.Collections;
using System.Collections.Generic;

#region 命令类型的事件(包括网络消息命令, )
public class EventID_Cmd
{
    public const string U3DClientOnLine = "U3DClientOnLine";            //U3D客户端上线事件
    public const string U3DClientOffLine = "U3DClientOffLine";          //U3D客户端离线事件
    public const string CCTVCtrlClientOnLine = "CCTVCtrlClientOnLine";  //CCTV视频控制端上线事件
    public const string CCTVCtrlClientOffLine = "CCTVCtrlClientOffLine";//CCTV视频控制端下线事件
    public const string ATSClientOnLine = "ATSClientOnLine";            //ATS客户端上线事件
    public const string ATSClientOffLine = "ATSClientOffLine";          //ATS客户端下线事件
}
#endregion

#region UI类型的事件
public class EventID_UI
{
    
}
#endregion
