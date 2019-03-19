

namespace TDFramework.UIFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UIMgr : MonoBehaviour
    {
        #region 字段和属性
        //使用栈结构保存UIPanel, 该栈中只有UIPanel
        private Stack<UIPanel> m_panelsStack = new Stack<UIPanel>();
        //使用字典记录UIPanel， Key为UIPanel类型
        private Dictionary<UIPanelType, UIPanel> m_panelsDict = new Dictionary<UIPanelType, UIPanel>();
        #endregion

        #region 方法
        /*
            得到指定UIPanel
        */
        public UIPanel GetUIPanel(UIPanelType type)
        {
            UIPanel panel = null;
            if(!m_panelsDict.TryGetValue(type, out panel) || panel == null)
            {
                //从AssetBundle中加载UIPanelPrefab
            }
            return panel;
        }
        /*
            获取栈顶UIPanel
        */
        public UIPanel GetTopUIPanel()
        {
            if(m_panelsStack.Count <= 0) return null;
            return m_panelsStack.Peek();
        }
        /*
            打开指定UIPanel
        */
        public void PushPanel(UIPanelType type)
        {
            UIPanel panel = GetUIPanel(type);
            if(panel == null) return;
            
            UIPanel prePanel = GetTopUIPanel();
            if(prePanel != null)
            {
                prePanel.OnPause(); 
            }
            m_panelsStack.Push(panel);
            panel.OnEnter();
            if(m_panelsDict.ContainsKey(type) == false)
            {
                m_panelsDict.Add(type, panel);
            }
        }
        /*
            关闭当前Panel
        */
        public void PopPanel()
        {
            if(m_panelsStack.Count <= 0) return;
            UIPanel panel = GetTopUIPanel();
            if(panel != null)
            {
                panel.OnExit();
            }
            m_panelsStack.Pop();
            panel = GetTopUIPanel();
            if(panel != null)
            {
                panel.OnResume();
            }
        }
        #endregion

    }
}