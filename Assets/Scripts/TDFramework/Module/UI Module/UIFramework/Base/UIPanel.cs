
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
// 文件名(File Name):             BasePanel.cs
// 作者(Author):                  田山杉
// 创建时间(CreateTime):          2019-01-10 22:52:27
// 修改者列表(modifier):
// 模块描述(Module description):  Panel是指一个功能Panel页面, 这个功能页面可以延伸很多具体的子页面UIView, Panle管理着Panel下对应的View视图
// ***************************************************************

namespace TDFramework.UIFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : UIPage
    {
        #region 字段
        private Dictionary<UIViewType, UIView> m_viewsDict = new Dictionary<UIViewType, UIView>();
        #endregion

        #region 方法
        /*
            设置当前Panel是否可以交互
        */
        public void SetPanelInteractive(bool activeStatus)
        {
            if (m_canvasGroup != null)
            {
                m_canvasGroup.interactable = activeStatus;
                m_canvasGroup.blocksRaycasts = activeStatus;
            }
        }
        /*
            打开指定View
            isModalDialog是否是模态对话框(默认是模态对话框)
            isClosePreView是否关闭上一个被打开的View(默认要关闭上一个View)
        */
        public void OpenView(UIViewType viewType, bool isModalDialog = true)
        {
            UIView view = GetUIView(viewType);
            if (view != null)
            {
                SetPanelInteractive(!isModalDialog);
                view.OnEnter();
            }
        }
        /*
            关闭指定View
        */
        public void CloseView(UIViewType viewType)
        {
            UIView view = null;
            view = GetUIView(viewType);
            if (view != null)
            {
                view.OnExit();
            }
        }
        #endregion

        #region 私有方法
        private UIView GetUIView(UIViewType viewType)
        {
            UIView view = null;
            if (m_viewsDict.TryGetValue(viewType, out view) == false || view == null)
            {
                //从AssetBundle中加载UIView资源
                view.RootPanel = this;
            }
            return view;
        }
        #endregion

    }
}
