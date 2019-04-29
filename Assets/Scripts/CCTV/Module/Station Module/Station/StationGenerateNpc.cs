using System;
using System.Collections;
using System.Collections.Generic;
using TDFramework;
using UnityEngine;
using UnityEngine.AI;

public class StationGenerateNpc : MonoBehaviour {
    #region 字段
    public UInt16 m_stationIndex;
    private int m_enterStationUpNpcCount;
    private int m_enterStationDownNpcCount;
    private int m_exitStationUpNpcCount;
    private int m_exitStationDownNpcCount;

    private StationModule m_stationModule;
    private StationInfo m_stationInfo;
    #endregion

    #region 属性
    public int EnterStationUpNpcCount {
        get {
            return m_stationModule.GetNpcCount (m_stationIndex, NpcActionStatus.EnterStationTrainUp_NpcActionStatus);
        }
    }
    public int EnterStationDownNpcCount {
        get {
            return m_stationModule.GetNpcCount (m_stationIndex, NpcActionStatus.EnterStationTrainDown_NpcActionStatus);
        }
    }
    public int ExitStationUpNpcCount {
        get {
            return m_stationModule.GetNpcCount (m_stationIndex, NpcActionStatus.ExitStationTrainUp_NpcActionStatus);
        }
    }
    public int ExitStationDownNpcCount {
        get {
            return m_stationModule.GetNpcCount (m_stationIndex, NpcActionStatus.ExitStationTrainDown_NpcActionStatus);
        }
    }
    #endregion

    #region 组合
    private NpcFactory m_npcFactory;
    #endregion

    #region Unity生命周期
    void Start () {
        m_stationModule = (StationModule) SingletonMgr.ModuleMgr.GetModule (StringMgr.StationModuleName);
        m_stationInfo = SingletonMgr.GameGlobalInfo.StationInfoList.GetStationInfo (m_stationIndex);
        m_npcFactory = new NpcFactory ();

        StartCoroutine (CheckStationEnterStationUpNpcStatus ());
        StartCoroutine (CheckStationEnterStationDownNpcStatus ());
        StartCoroutine (CheckStationExitStationUpNpcStatus ());
        StartCoroutine (CheckStationExitStationDownNpcStatus ());
    }
    #endregion

    #region 协程, 用来检测站台中的Npc个数，以便找准时机创建Npc
    IEnumerator CheckStationEnterStationUpNpcStatus () {
        while (true) {
            if (EnterStationUpNpcCount < m_stationInfo.EnterStationUpMaxNpcCount) {
                //应该生成一个进站上行Npc
                yield return new WaitForSeconds (m_stationInfo.GenerateNpcIntervalTime);
                CreateNpc (NpcActionStatus.EnterStationTrainUp_NpcActionStatus, PointStatus.EnterStation);
            }
            yield return null;
        }
    }
    IEnumerator CheckStationEnterStationDownNpcStatus () {
        while (true) {
            if (EnterStationDownNpcCount < m_stationInfo.EnterStationDownMaxNpcCount) {
                //应该生成一个进站下行Npc
                yield return new WaitForSeconds (m_stationInfo.GenerateNpcIntervalTime);
                CreateNpc (NpcActionStatus.EnterStationTrainDown_NpcActionStatus, PointStatus.EnterStation);
            }
            yield return null;
        }
    }
    IEnumerator CheckStationExitStationUpNpcStatus () {
        while (true) {
            if (m_stationModule.IsOpenShangXingPingBiMen (m_stationIndex, DeviceType.PingBiMen) && ExitStationUpNpcCount < m_stationInfo.ExitStationUpMaxNpcCount) {
                //应该生成一个出站上行Npc
                yield return new WaitForSeconds (m_stationInfo.GenerateNpcIntervalTime);
                // for (int i = 0; i < m_stationInfo.ExitStationUpGenerateNpcCount; i++)
                int count = m_stationInfo.ExitStationUpMaxNpcCount - ExitStationUpNpcCount;
                for (int i = 0; i < count; ++i) {
                    CreateNpc (NpcActionStatus.ExitStationTrainUp_NpcActionStatus, PointStatus.Train_Up_Birth);
                    yield return new WaitForSeconds (0.05f);
                }
                m_stationModule.CloseShangXingPingBiMen (m_stationIndex, DeviceType.PingBiMen);
            }
            yield return null;
        }
    }
    IEnumerator CheckStationExitStationDownNpcStatus () {
        while (true) {
            if (m_stationModule.IsOpenXiaXingPingBiMen (m_stationIndex, DeviceType.PingBiMen) && ExitStationDownNpcCount < m_stationInfo.ExitStationDownMaxNpcCount) {
                yield return new WaitForSeconds (m_stationInfo.GenerateNpcIntervalTime);
                int count = m_stationInfo.ExitStationDownMaxNpcCount - ExitStationDownNpcCount;
                // for (int i = 0; i < m_stationInfo.ExitStationDownGenerateNpcCount; i++)
                for (int i = 0; i < count; ++i) {
                    CreateNpc (NpcActionStatus.ExitStationTrainDown_NpcActionStatus, PointStatus.Train_Down_Birth);
                    yield return new WaitForSeconds (0.05f);
                }
                //生成Npc之后，将屏蔽门是否可生成Npc的状态设置为false
                m_stationModule.CloseXiaXingPingBiMen (m_stationIndex, DeviceType.PingBiMen);
            }
            yield return null;
        }
    }
    private void CreateNpc (NpcActionStatus npcActionStatus, PointStatus pointStatus) {
        NpcType npcType = NpcType.None;
        GameObject npcGo = m_npcFactory.CreateNpc (ref npcType);
        if (npcGo == null) return;
        Transform parentTrans = m_stationModule.GetNpcParentTransform (m_stationIndex, npcActionStatus);
        if (parentTrans == null) return;
        npcGo.transform.SetParent (parentTrans);
        Point point = null;
        if (pointStatus == PointStatus.Train_Up_Birth || pointStatus == PointStatus.Train_Down_Birth) {
            point = StationEngine.Instance.GetNoReservationPoint2RandomPointQueue (m_stationIndex, (int) pointStatus, 0, 16);
        } else {
            point = StationEngine.Instance.GetNoReservationPoint2RandomPointQueue (m_stationIndex, (int) pointStatus);
        }
        if (point == null) return;
        if (pointStatus != PointStatus.EnterStation) {
            point.IsReservation = true;
        }
        npcGo.transform.localPosition = new Vector3 (point.PosX, point.PosY, point.PosZ);
        npcGo.transform.localEulerAngles = new Vector3 (point.EulerAngleX, point.EulerAngleY, point.EulerAngleZ);
        npcGo.GetComponent<NavMeshAgent> ().enabled = true;

        NpcAction npcAction = null;
        switch (npcActionStatus) {
            case NpcActionStatus.EnterStationTrainUp_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcEnterStationUpAction> ();
                    npcAction.NpcActionStatus = NpcActionStatus.EnterStationTrainUp_NpcActionStatus;
                    if (System.Object.ReferenceEquals (npcAction, null) == false) {
                        npcAction.NpcType = npcType;
                        npcAction.StationIndex = m_stationIndex;
                        m_stationModule.AddNpcAction (m_stationIndex, npcAction);
                    }
                    break;
                }
            case NpcActionStatus.EnterStationTrainDown_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcEnterStationDownAction> ();
                    npcAction.NpcActionStatus = NpcActionStatus.EnterStationTrainDown_NpcActionStatus;
                    if (System.Object.ReferenceEquals (npcAction, null) == false) {
                        npcAction.NpcType = npcType;
                        npcAction.StationIndex = m_stationIndex;
                        m_stationModule.AddNpcAction (m_stationIndex, npcAction);
                    }
                    break;
                }
            case NpcActionStatus.ExitStationTrainUp_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcExitStationUpAction_New> ();
                    if (System.Object.ReferenceEquals (npcAction, null) == false) {
                        npcAction.NpcType = npcType;
                        npcAction.StationIndex = m_stationIndex;
                        m_stationModule.AddNpcAction (m_stationIndex, npcAction);
                    }
                    ((NpcExitStationUpAction_New) npcAction).StartAction (point);
                    break;
                }
            case NpcActionStatus.ExitStationTrainDown_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcExitStationDownAction_New> ();
                    if (System.Object.ReferenceEquals (npcAction, null) == false) {
                        npcAction.NpcType = npcType;
                        npcAction.StationIndex = m_stationIndex;
                        m_stationModule.AddNpcAction (m_stationIndex, npcAction);
                    }
                    ((NpcExitStationDownAction_New) npcAction).StartAction (point);
                    break;
                }
            default:
                break;
        }
    }
    #endregion

}