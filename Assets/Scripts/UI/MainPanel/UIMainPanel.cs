
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
// 文件名(File Name):             UIMainPanel.cs
// 作者(Author):                  #AuthorName#
// 创建时间(CreateTime):          #CreateDate#
// 修改者列表(modifier):
// 模块描述(Module description):
// ***************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework.UIFramework;
using TDFramework;
using UnityEngine.UI;
using PureMVC.Interfaces;

public class UIMainPanel : UIPanel
{
    #region 字段
    private Text m_text;
    #endregion

    #region Unity生命周期
    protected override void Awake()
    {
        base.Awake();

        //绑定跟当前UIPanel相关的Mediator, Command, Proxy
        Template_Command command = new Template_Command();
        RegisterCommand(EventID_Cmd.U3DClientOnLine, command);//注册Command
        RegisterMediator(new Template_Mediator()); //注册Mediator
        RegisterMediator(this); //注册UIPanelMediator
        RegisterProxy(new Template_Proxy());    //注册Proxy

        //获取UI
        m_text = transform.Find("Text").GetComponent<Text>();
    }
    protected void OnDestroy()
    {
        RemoveCommand(EventID_Cmd.U3DClientOnLine);
        RemoveMediator(Template_Mediator.NAME);
        RemoveMediator(this.MediatorName);
        RemoveProxy(Template_Proxy.NAME);
    }
    #endregion

    #region Mediator功能实现
    public override string[] ListNotificationInterests()
    {
        return new string[]{
            EventID_Cmd.U3DClientOnLine,
        };
    }
    public override void HandleNotification(INotification notification)
    {
        if (notification.Name == EventID_Cmd.U3DClientOnLine)
        {
            Packet packet = (Packet)notification.Body;
            m_text.text = packet.m_sendId.ToString();  
        }
    }
    public override void OnRegister()
    {

    }
    public override void OnRemove()
    {

    }
    public override string MediatorName
    {
        get
        {
            return "Template_Panel_Mediator";
        }
    }
    #endregion

    #region UI事件处理

    #endregion	


}
