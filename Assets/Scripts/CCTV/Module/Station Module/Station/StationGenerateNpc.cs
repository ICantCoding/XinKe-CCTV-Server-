using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework;
using UnityEngine.AI;

public class StationGenerateNpc : MonoBehaviour
{
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
    public int EnterStationUpNpcCount
    {
        get
        {
            return m_stationModule.GetNpcCount(m_stationIndex, NpcActionStatus.EnterStationTrainUp_NpcActionStatus);
        }
    }
    public int EnterStationDownNpcCount
    {
        get
        {
            return m_stationModule.GetNpcCount(m_stationIndex, NpcActionStatus.EnterStationTrainDown_NpcActionStatus);
        }
    }
    public int ExitStationUpNpcCount
    {
        get
        {
            return m_stationModule.GetNpcCount(m_stationIndex, NpcActionStatus.ExitStationTrainUp_NpcActionStatus);
        }
    }
    public int ExitStationDownNpcCount
    {
        get
        {
            return m_stationModule.GetNpcCount(m_stationIndex, NpcActionStatus.ExitStationTrainDown_NpcActionStatus);
        }
    }
    #endregion

    #region 组合
    private NpcFactory m_npcFactory;
    #endregion

    #region Unity生命周期
    void Start()
    {
        m_stationModule = (StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName);
        m_stationInfo = SingletonMgr.GameGlobalInfo.StationInfoList.GetStationInfo(m_stationIndex);
        m_npcFactory = new NpcFactory();

        StartCoroutine(CheckStationNpcStatus());
    }
    #endregion

    #region 协程, 用来检测站台中的Npc个数，以便找准时机创建Npc
    IEnumerator CheckStationNpcStatus()
    {
        while (true)
        {
            if (EnterStationUpNpcCount < m_stationInfo.EnterStationUpMaxNpcCount)
            {
                //应该生成一个进站上行Npc
                yield return new WaitForSeconds(m_stationInfo.GenerateNpcIntervalTime);
                //CreateNpc(NpcActionStatus.EnterStationTrainUp_NpcActionStatus, PointStatus.EnterStation);
            }
            if (EnterStationDownNpcCount < m_stationInfo.EnterStationDownMaxNpcCount)
            {
                //应该生成一个进站下行Npc
                yield return new WaitForSeconds(m_stationInfo.GenerateNpcIntervalTime);
                //CreateNpc(NpcActionStatus.EnterStationTrainDown_NpcActionStatus, PointStatus.EnterStation);
            }
            if (ExitStationUpNpcCount < m_stationInfo.ExitStationUpMaxNpcCount)
            {
                //应该生成一个出站上行Npc
                yield return new WaitForSeconds(m_stationInfo.GenerateNpcIntervalTime);
                CreateNpc(NpcActionStatus.ExitStationTrainUp_NpcActionStatus, PointStatus.Train_Up_Birth);
            }
            if (ExitStationDownNpcCount < m_stationInfo.ExitStationDownMaxNpcCount)
            {
                //应该生成一个出站下行Npc
                yield return new WaitForSeconds(m_stationInfo.GenerateNpcIntervalTime);
                // CreateNpc(NpcActionStatus.ExitStationTrainDown_NpcActionStatus, PointStatus.Train_Down_Birth);
            }
            yield return null;
        }
    }
    private void CreateNpc(NpcActionStatus npcActionStatus, PointStatus pointStatus)
    {
        GameObject npcGo = m_npcFactory.CreateNpc();
        if (npcGo == null) return;
        Transform parentTrans = m_stationModule.GetNpcParentTransform(m_stationIndex, npcActionStatus);
        if (parentTrans == null) return;
        npcGo.transform.SetParent(parentTrans);
        Point point = StationEngine.Instance.GetFirstPoint2RandomPointQueue(m_stationIndex, (int)pointStatus);
        npcGo.transform.localPosition = new Vector3(point.PosX, point.PosY, point.PosZ);
        npcGo.transform.localEulerAngles = new Vector3(point.EulerAngleX, point.EulerAngleY, point.EulerAngleZ);
        npcGo.GetComponent<NavMeshAgent>().enabled = true;

        NpcAction npcAction = null;
        switch (npcActionStatus)
        {
            case NpcActionStatus.EnterStationTrainUp_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcEnterStationUpAction>();
                    npcAction.NpcActionStatus = NpcActionStatus.EnterStationTrainUp_NpcActionStatus;
                    break;
                }
            case NpcActionStatus.EnterStationTrainDown_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcEnterStationDownAction>();
                    npcAction.NpcActionStatus = NpcActionStatus.EnterStationTrainDown_NpcActionStatus;
                    break;
                }
            case NpcActionStatus.ExitStationTrainUp_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcExitStationUpAction_New>();
                    ((NpcExitStationUpAction_New)npcAction).StartAction(point);
                    break;
                }
            case NpcActionStatus.ExitStationTrainDown_NpcActionStatus:
                {
                    npcAction = npcGo.AddComponent<NpcExitStationDownAction>();
                    break;
                }
            default:
                break;
        }
        if (System.Object.ReferenceEquals(npcAction, null) == false)
        {
            npcAction.StationIndex = m_stationIndex;
            m_stationModule.AddNpcAction(m_stationIndex, npcAction);
        }
    }
    #endregion

}














