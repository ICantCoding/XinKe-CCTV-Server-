

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ClickEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region 静态方法, 获取或添加GameObject的ClickEventListener组件
        public static ClickEventListener Get(GameObject obj)
        {
            ClickEventListener listener = obj.GetComponent<ClickEventListener>();
            if (listener == null)
            {
                listener = obj.AddComponent<ClickEventListener>();
            }
            return listener;
        }
        #endregion

        #region 代理
        private System.Action<GameObject> ClickHandler = null;
        private System.Action<GameObject> DoubleClickHandler = null;
        private System.Action<GameObject> OnPointerDownHandler = null;
        private System.Action<GameObject> OnPointerUpHandler = null;
        #endregion

        #region 状态字段
        private bool m_isPressed = false;
        #endregion

        #region 状态属性
        public bool IsPressed
        {
            get { return m_isPressed; }
        }
        #endregion

        #region 方法
        public void SetClickEventHandler(System.Action<GameObject> handler)
        {
            ClickHandler = handler;
        }
        public void SetDoubleClickEventHandler(System.Action<GameObject> handler)
        {
            DoubleClickHandler = handler;
        }
        public void SetPointerDownEventHandler(System.Action<GameObject> handler)
        {
            OnPointerDownHandler = handler;
        }
        public void SetPointerUpEventHandler(System.Action<GameObject> handler)
        {
            OnPointerUpHandler = handler;
        }
        #endregion

        #region UnityEngine.EventSystem接口实现
        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.clickCount == 2)
            {
                if(DoubleClickHandler != null)
                {
                    DoubleClickHandler(gameObject);
                }
            }
            else
            {
                if(ClickHandler != null)
                {
                    ClickHandler(gameObject);
                }
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            m_isPressed = true;
            if(OnPointerDownHandler != null)
            {
                OnPointerDownHandler(gameObject);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
           m_isPressed = false;
           if(OnPointerUpHandler != null)
           {
               OnPointerUpHandler(gameObject);
           }
        }
        #endregion
    }
}
