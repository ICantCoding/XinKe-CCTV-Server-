using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    #region 字段
    //保存所有站台的所有摄像头
    private Dictionary<UInt16, Dictionary<UInt16, Camera>> m_allCameraDict = 
        new Dictionary<UInt16, Dictionary<UInt16, Camera>>();
    //保存站台启用的摄像头，所谓启用就是有客户端正在使用该摄像头
    private Dictionary<UInt16, Dictionary<UInt16, Camera>> m_cameraDict =
        new Dictionary<UInt16, Dictionary<UInt16, Camera>> ();
    #endregion

    #region 属性
    public Dictionary<UInt16, Dictionary<UInt16, Camera>> CameraDict {
        get { return m_cameraDict; }
    }
    #endregion

    #region Unity生命周期
    void Awake()
    {
        Transform cameraRootTrans = transform.Find("CameraRoot");
        if(cameraRootTrans == null) return;
        int stationCount = cameraRootTrans.childCount;
        Transform tempTrans = null;
        Dictionary<UInt16, Camera> tempDict = null;
        for(int i = 0; i < stationCount; ++i)
        {
            tempDict = new Dictionary<UInt16, Camera>();
            tempTrans = cameraRootTrans.GetChild(i);
            string name = tempTrans.gameObject.name;
            System.UInt16 stationIndex = (System.UInt16)System.Enum.Parse(typeof(StationType), tempTrans.gameObject.name);
            int cameraCount = tempTrans.childCount;
            for(UInt16 j = 0; j < cameraCount; ++j)
            {
                tempDict.Add(j, tempTrans.GetChild(j).GetComponent<Camera>());
            }
            m_allCameraDict.Add(stationIndex, tempDict);
        }
    }
    #endregion

    #region 方法
    public void AddCamera (UInt16 stationIndex, UInt16 cameraIndex, Camera pCamera) {
        Dictionary<UInt16, Camera> dict = null;
        if (m_cameraDict.TryGetValue (stationIndex, out dict) == false || dict == null) {
            dict = new Dictionary<UInt16, Camera> ();
        }
        if (dict != null && dict.ContainsKey (stationIndex) == false) {
            dict.Add (cameraIndex, pCamera);
        }
    }
    public void RemoveCamera (UInt16 stationIndex, UInt16 cameraIndex) {
        Dictionary<UInt16, Camera> dict = null;
        if (m_cameraDict.TryGetValue (stationIndex, out dict) && dict != null) {
            if (dict.ContainsKey (cameraIndex)) {
                dict.Remove (cameraIndex);
            }
        }
    }
    public Dictionary<UInt16, Camera> GetStationCamera (UInt16 stationIndex) {
        Dictionary<UInt16, Camera> dict = null;
        m_cameraDict.TryGetValue(stationIndex, out dict);
        return dict;
    }
    public Camera GetCamera(UInt16 stationIndex, UInt16 cameraIndex)
    {
        Camera tempCamer = null;
        Dictionary<UInt16, Camera> dict = null;
        if(m_cameraDict.TryGetValue(stationIndex, out dict) && dict != null)
        {
            dict.TryGetValue(cameraIndex, out tempCamer);
        }
        return tempCamer;
    }
    #endregion
}