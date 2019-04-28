
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
// 模块描述(Module description):  服务器主页面UI
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
    #region 颜色字段
    private Color m_successLinkColor = new Color(165 / 255.0f, 221 / 255.0f, 130 / 255.0f, 1.0f);
    private Color m_faliLinkColor = new Color(221 / 255.0f, 130 / 255.0f, 130 / 255.0f, 1.0f);
    #endregion

    #region 状态字段
    private bool m_cctvServerIsConnected = false;
    private bool m_atsServerIsConnected = false;
    #endregion

    #region 字段
    private Transform m_clientOnLineItemContentTrans;
    private Button m_startBtn;
    private Button m_stopBtn;
    private Text m_serverIpTxt;
    private Text m_serverPortTxt;
    private Text m_statusTxt;
    public Text m_onlineCountText;
    public Text m_linkSocketCountText;

    public Image m_cctvBgImg;
    public Text m_cctvStatusText;
    public Button m_cctvRelinkBtn;
    public Image m_cctvSuccessFlagImg;
    public Image m_atsBgImg;
    public Text m_atsStatusText;
    public Button m_atsRelinkBtn;
    public Image m_atsSuccessFlagImg;
    #endregion

    #region 数据集合
    //用于管理登录在线的客户端显示UI的ClientOnLineItem
    private Dictionary<System.UInt16, ClientOnLineItem> m_clientOnLineItemDict = new Dictionary<ushort, ClientOnLineItem>();
    #endregion

    #region Unity生命周期
    protected override void Awake()
    {
        base.Awake();

        //绑定跟当前UIPanel相关的Mediator, Command, Proxy
        UIMainPanel_Command command = new UIMainPanel_Command();
        RegisterCommand(EventID_Cmd.U3DClientOnLine, command);
        RegisterCommand(EventID_Cmd.U3DClientOffLine, command);
        RegisterCommand(EventID_Cmd.StationClientOnLine, command);
        RegisterCommand(EventID_Cmd.StationClientOffLine, command);
        RegisterCommand(EventID_Cmd.ServerStart, command);
        RegisterCommand(EventID_Cmd.ServerStop, command);
        RegisterCommand(EventID_Cmd.CCTVCtrlClientOnLine, command);
        RegisterCommand(EventID_Cmd.CCTVCtrlClientOffLine, command);
        RegisterCommand(EventID_Cmd.ATSClientOnLine, command);
        RegisterCommand(EventID_Cmd.ATSClientOffLine, command);
        RegisterMediator(new UIMainPanel_Mediator());
        RegisterMediator(this);
        RegisterProxy(new UIMainPanel_Proxy());

        //获取UI
        m_clientOnLineItemContentTrans = transform.Find("Image/Image/Scroll View/Viewport/Content");
        m_startBtn = transform.Find("StartServerBtn").GetComponent<Button>();
        if (m_startBtn != null)
        {
            m_startBtn.onClick.AddListener(OnStartBtnClick);
        }
        m_stopBtn = transform.Find("StopServerBtn").GetComponent<Button>();
        if (m_stopBtn != null)
        {
            m_stopBtn.onClick.AddListener(OnStopBtnClick);
        }
        m_serverIpTxt = transform.Find("Image/ServerIPText").GetComponent<Text>();
        m_serverPortTxt = transform.Find("Image/ServerPortText").GetComponent<Text>();
        m_statusTxt = transform.Find("ServerStatus/Text").GetComponent<Text>();
    }
    void Start()
    {
        m_serverIpTxt.text = "IP地址: " + SingletonMgr.GameGlobalInfo.ServerInfo.ServerIpAddress;
        m_serverPortTxt.text = "端口: " + SingletonMgr.GameGlobalInfo.ServerInfo.ServerPort.ToString();
    }
    void Update()
    {
        if (m_atsServerIsConnected)
        {
            m_atsBgImg.color = m_successLinkColor;
            m_atsStatusText.color = Color.green;
            m_atsStatusText.text = "✔";
            m_atsRelinkBtn.gameObject.SetActive(false);
            m_atsSuccessFlagImg.gameObject.SetActive(true);
        }
        else
        {
            m_atsBgImg.color = m_faliLinkColor;
            m_atsStatusText.color = Color.red;
            m_atsStatusText.text = "✖";
            m_atsRelinkBtn.gameObject.SetActive(true);
            m_atsSuccessFlagImg.gameObject.SetActive(false);
        }
        if (m_cctvServerIsConnected)
        {
            m_cctvBgImg.color = m_successLinkColor;
            m_cctvStatusText.color = Color.green;
            m_cctvStatusText.text = "✔";
            m_cctvRelinkBtn.gameObject.SetActive(false);
            m_cctvSuccessFlagImg.gameObject.SetActive(true);
        }
        else
        {
            m_cctvBgImg.color = m_faliLinkColor;
            m_cctvStatusText.color = Color.red;
            m_cctvStatusText.text = "✖";
            m_cctvRelinkBtn.gameObject.SetActive(true);
            m_cctvSuccessFlagImg.gameObject.SetActive(false);
        }
    }
    protected void OnDestroy()
    {
        RemoveCommand(EventID_Cmd.U3DClientOnLine);
        RemoveCommand(EventID_Cmd.U3DClientOffLine);
        RemoveCommand(EventID_Cmd.StationClientOnLine);
        RemoveCommand(EventID_Cmd.U3DClientOffLine);
        RemoveCommand(EventID_Cmd.ServerStart);
        RemoveCommand(EventID_Cmd.ServerStop);
        RemoveCommand(EventID_Cmd.CCTVCtrlClientOnLine);
        RemoveCommand(EventID_Cmd.CCTVCtrlClientOffLine);
        RemoveCommand(EventID_Cmd.ATSClientOnLine);
        RemoveCommand(EventID_Cmd.ATSClientOffLine);
        RemoveMediator(UIMainPanel_Mediator.NAME);
        RemoveMediator(this.MediatorName);
        RemoveProxy(UIMainPanel_Proxy.NAME);
    }
    #endregion

    #region Mediator功能实现
    public override string MediatorName
    {
        get { return "UIMainPanel"; }
    }
    public override string[] ListNotificationInterests()
    {
        return new string[]{
            EventID_UI.ServerStart,
            EventID_UI.ServerStop,
            EventID_UI.U3DClientOnLine,
            EventID_UI.U3DClientOffLine,
            EventID_UI.StationClientOnLine,
            EventID_UI.StationClientOffLine,
            EventID_UI.CCTVCtrlClientOnLine,
            EventID_UI.CCTVCtrlClientOffLine,
            EventID_UI.ATSClientOnLine,
            EventID_UI.ATSClientOffLine,
        };
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case EventID_UI.U3DClientOnLine:
                {
                    U3DClientOnLine_Callback(notification);
                    break;
                }
            case EventID_UI.U3DClientOffLine:
                {
                    U3DClientOffLine_Callback(notification);
                    break;
                }
            case EventID_UI.StationClientOnLine:
                {
                    StationClientOnLine_Callback(notification);
                    break;
                }
            case EventID_UI.StationClientOffLine:
                {
                    StationClientOffLine_Callback(notification);
                    break;
                }
            case EventID_UI.ServerStart:
                {
                    ServerStart_Callback(notification);
                    break;
                }
            case EventID_UI.ServerStop:
                {
                    ServerStop_Callback(notification);
                    break;
                }
            case EventID_UI.CCTVCtrlClientOnLine:
                {
                    CCTVCtrlClientOnLine_Callback(notification);
                    break;
                }
            case EventID_UI.CCTVCtrlClientOffLine:
                {
                    CCTVCtrlClientOffLine_Callback(notification);
                    break;
                }
            case EventID_UI.ATSClientOnLine:
                {
                    ATSClientOnLine_Callback(notification);
                    break;
                }
            case EventID_UI.ATSClientOffLine:
                {
                    ATSClientOffLine_Callback(notification);
                    break;
                }
            default:
                break;
        }
    }
    #endregion

    #region 数据管理方法
    private void AddClientOnLineItem(System.UInt16 u3dId, ClientOnLineItem item)
    {
        if (item == null) return;
        if (m_clientOnLineItemDict.ContainsKey(u3dId) == false)
        {
            m_clientOnLineItemDict.Add(u3dId, item);
        }
    }
    private void RemoveClientOnLineItem(System.UInt16 u3dId)
    {
        ClientOnLineItem item = null;
        if (m_clientOnLineItemDict.TryGetValue(u3dId, out item) && item != null)
        {
            m_clientOnLineItemDict.Remove(u3dId);
        }
    }
    private ClientOnLineItem GetClientOnLineItem(System.UInt16 u3dId)
    {
        ClientOnLineItem item = null;
        m_clientOnLineItemDict.TryGetValue(u3dId, out item);
        return item;
    }
    #endregion

    #region 消息事件处理
    private void U3DClientOnLine_Callback(INotification notification)
    {
        //数据解析
        object[] objs = notification.Body as object[];
        U3DClientLogin u3dClientLogin = (U3DClientLogin)objs[0];
        System.Net.IPEndPoint endPoint = (System.Net.IPEndPoint)objs[1];
        string content = u3dClientLogin.m_clientId + " " + u3dClientLogin.m_clientName + " (" +
         endPoint.Address.ToString() + " " + endPoint.Port.ToString() + ")";
        //UI处理
        GameObject clientOnLineItemGo = SingletonMgr.ObjectManager.Instantiate("Assets/Prefabs/UI/ClientOnLineItem.prefab", false);
        if (clientOnLineItemGo != null && m_clientOnLineItemContentTrans != null)
        {
            clientOnLineItemGo.transform.SetParent(m_clientOnLineItemContentTrans);
            clientOnLineItemGo.transform.localPosition = Vector3.zero;
            clientOnLineItemGo.transform.localScale = Vector3.one;
            clientOnLineItemGo.transform.localEulerAngles = Vector3.zero;
            ClientOnLineItem itemCom = clientOnLineItemGo.GetComponent<ClientOnLineItem>();
            if (itemCom != null)
            {
                itemCom.SetCell(content);
                AddClientOnLineItem(u3dClientLogin.m_clientId, itemCom);
            }
        }
        m_onlineCountText.text = ActorManager.Instance.GetActor<WorldActor>().OnLineCount.ToString();
        m_linkSocketCountText.text = ActorManager.Instance.GetActor<WorldActor>().LinkSocketCount.ToString();
    }
    private void U3DClientOffLine_Callback(INotification notification)
    {
        System.UInt16 u3dId = (System.UInt16)notification.Body;
        ClientOnLineItem item = GetClientOnLineItem(u3dId);
        if (item != null)
        {
            SingletonMgr.ObjectManager.ReleaseGameObjectItem(item.gameObject); //回收
            RemoveClientOnLineItem(u3dId);
        }
        m_onlineCountText.text = ActorManager.Instance.GetActor<WorldActor>().OnLineCount.ToString();
        m_linkSocketCountText.text = ActorManager.Instance.GetActor<WorldActor>().LinkSocketCount.ToString();
    }
    private void StationClientOnLine_Callback(INotification notification)
    {
        m_linkSocketCountText.text = ActorManager.Instance.GetActor<WorldActor>().LinkSocketCount.ToString();
    }
    private void StationClientOffLine_Callback(INotification notification)
    {
        m_linkSocketCountText.text = ActorManager.Instance.GetActor<WorldActor>().LinkSocketCount.ToString();
    }
    private void ServerStart_Callback(INotification notification)
    {
        if (m_statusTxt != null)
        {
            m_statusTxt.text = "服务器运行中...";
        }
    }
    private void ServerStop_Callback(INotification notification)
    {
        if (m_statusTxt != null)
        {
            m_statusTxt.text = "服务器关闭!";
        }
    }
    private void CCTVCtrlClientOnLine_Callback(INotification notification)
    {
        m_cctvServerIsConnected = true;
    }
    private void CCTVCtrlClientOffLine_Callback(INotification notification)
    {
        m_cctvServerIsConnected = false;
    }
    private void ATSClientOnLine_Callback(INotification notification)
    {
        m_atsServerIsConnected = true;
    }
    private void ATSClientOffLine_Callback(INotification notification)
    {
        m_atsServerIsConnected = false;
    }
    #endregion

    #region UI事件处理
    private void OnStartBtnClick()
    {
        IModule module = SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
        if (module != null)
        {
            NetworkModule networkModule = (NetworkModule)module;
            networkModule.Start();
        }
    }
    private void OnStopBtnClick()
    {
        IModule module = SingletonMgr.ModuleMgr.GetModule(StringMgr.NetworkModuleName);
        if (module != null)
        {
            NetworkModule networkModule = (NetworkModule)module;
            networkModule.Stop();
        }
    }
    #endregion
}
