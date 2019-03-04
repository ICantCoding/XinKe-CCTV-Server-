
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
// 文件名(File Name):             ClientOnLineItem.cs
// 作者(Author):                  #AuthorName#
// 创建时间(CreateTime):          #CreateDate#
// 修改者列表(modifier):
// 模块描述(Module description):
// ***************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientOnLineItem : MonoBehaviour
{
    #region 字段
    private Text m_contentTxt;
    private Button m_btn;
    #endregion

    #region 属性
    public Text ContentTxt
    {
        get
        {
            if (m_contentTxt == null)
            {
                m_contentTxt = transform.Find("Text").GetComponent<Text>();
            }
            return m_contentTxt;
        }
    }
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_btn = transform.GetComponent<Button>();
        if (m_btn != null)
        {
            m_btn.onClick.AddListener(OnBtnClick);
        }
    }
    #endregion

    #region 方法
    public void SetCell(string content)
    {
        ContentTxt.text = content;
    }
    #endregion

    #region UI事件处理
    private void OnBtnClick()
    {
        Debug.Log("On Btn Click...");
    }
    #endregion
}
