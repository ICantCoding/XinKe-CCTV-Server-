/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-06 15:23:28

** 功能描述:  Npc出站Action， 上行方向

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TDFramework;

public class NpcExitStationUpAction_New : NpcAction
{
    #region 常量字段
    //导航到目的地位置距离差
    private const float m_navDistance = 0.05f;
    private int EnterCheckTicketAnimatorHashValue = Animator.StringToHash("OpenZhaji");
    private int WalkAnimatorHashValue = Animator.StringToHash("Walk");
    private int StandUpAnimatorHashValue = Animator.StringToHash("StandUp");
    #endregion

    #region 条件字段
    private bool XXXXXXXFlag = true; //队列第一个位置点，跟设备相关时，对应设备的状态信息来表示true, false
    #endregion

    #region Unity生命周期
    protected override void Awake()
    {
        base.Awake();
        m_stepArray = new PointStatus[]
        {
            PointStatus.Train_Up_Birth,
            PointStatus.DownTrain_Up,
            PointStatus.ExitCheckTicket,
            PointStatus.ExitCheckTicketAfter,
            PointStatus.ExitStation
        };
        m_npcActionStatus = NpcActionStatus.ExitStationTrainUp_NpcActionStatus;
        m_endStepIndex = m_stepArray.Length - 1;
    }
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    public void StartAction(Point gotoPoint)
    {
        #region 这里在获取Npc生成后，需要到达的第一个位置点
        if (PointStatus.Train_Up_Birth == m_stepArray[m_startStepIndex])
        {
            m_gotoPoint = gotoPoint;
            tempPoint = m_gotoPoint;
        }
        else
        {
            //这个gotoPoint可能为null, 当所有位置点被预约完的时候
            tempPoint = GetNoReservationPoint2RandomPointQueue();
            if (tempPoint == null)
            {
                //当位置点被预约完的时候，去休息区, 那么站台中的Npc个数不能高于“排队点个数+对应休息区位置点个数”
                tempPoint = GetRestAreaPoint();
            }
            if (tempPoint != null)
            {
                tempPoint.IsReservation = true; //该位置点被预约, 预约了的位置点，不能被其他NPC再预约了， 除非为false
            }
        }
        if (tempPoint == null)
        {
            //m_gotPoint还是为null的话, 我们就放弃这个Npc, 销毁(或回收)gameObject
            //但是只要保证NPC的个数，比对应行为的排队+休息区的个数小，则不会出现这样的情况
            DestroyNpc4ObjectManager();
            return;
        }
        #endregion

        //Npc前往目的地位置点Point
        GotoDestination(tempPoint);
        //开启协程
        StartCoroutine(ActionCoroutine());
    }

    IEnumerator ActionCoroutine()
    {
        while (!IsDestroy)
        {
            m_selfPosV2.x = transform.position.x;
            m_selfPosV2.y = transform.position.z;

            if (m_gotoPoint != null)
            {
                #region 休息区NPC
                if (m_gotoPoint.IsRestAreaPoint)
                {
                    tempPoint = GetNoReservationPoint2RandomPointQueue();
                    if (tempPoint != null)
                    {
                        //队列中有位置空出来了, 尽量获取从该空位置点所在队列的最后一个位置点
                        int queueIndex = tempPoint.m_queueIndex;
                        tempPoint = StationEngine.Instance.GetReverseNoReservationPointByQueueIndex(m_stationIndex,
                            (int)m_stepArray[m_startStepIndex], queueIndex);
                        if (tempPoint != null)
                        {
                            GotoDestination(tempPoint);
                        }
                    }
                }
                #endregion

                if (Vector2.Distance(m_selfPosV2, m_desPosV2) < m_navDistance)
                {
                    //Npc到达了目的地位置点
                    m_gotoPoint.IsReservation = true;
                    m_gotoPoint.IsEmpty = false;
                    m_gotoPoint.m_npcId = m_npcId;
                    // transform.localEulerAngles = new Vector3(m_gotoPoint.EulerAngleX, m_gotoPoint.EulerAngleY, m_gotoPoint.EulerAngleZ);
                    if (m_gotoPoint != null && m_gotoPoint.IsRestAreaPoint)
                    {
                        //休息区NPC到达了休息区目的地位置点，则等待
                        m_gotoPoint.IsReservation = true;
                        m_gotoPoint.IsEmpty = false;
                        Wait();
                    }
                    else if (m_gotoPoint != null && m_gotoPoint.m_nextPoint != null && m_gotoPoint.IsRestAreaPoint == false)
                    {
                        //到达了队列中非第一个位置点, 需判断是否能够前往下一个位置点
                        if (m_gotoPoint.m_nextPoint.IsEmpty)
                        {
                            tempPoint = m_gotoPoint.m_nextPoint;
                            GotoDestination(tempPoint);
                        }
                        else
                        {
                            //下一个位置点还在占用中，下一个位置点非空, 则等待
                            yield return StartCoroutine(WaitCoroutine());
                        }
                    }
                    else if (m_gotoPoint != null && m_gotoPoint.m_nextPoint == null && m_gotoPoint.IsRestAreaPoint == false)
                    {
                        //Npc行为最后一个步骤完成，需要销毁或回收
                        if (m_gotoPoint.m_pointStatus == m_stepArray[m_endStepIndex])
                        {
                            //销毁NPC
                            DestroyNpc();
                            yield break;
                        }
                        //到达了队列中第一个位置点, 判断是否受到设备影响
                        if (m_gotoPoint.IsDeviceCtrl == true)
                        {
                            //闸机设备
                            if (m_gotoPoint.m_device.DeviceType == DeviceType.ZhaJi &&
                                ((ZhaJiDevice)(m_gotoPoint.m_device)).CanOpen == true)
                            {
                                ((ZhaJiDevice)(m_gotoPoint.m_device)).CanOpen = false;
                                NpcSync.SendNpcAnimation((UInt16)NpcAnimationType.OpenZhaJi, NpcId, StationIndex, (UInt16)NpcActionStatus);
                                m_animator.SetTrigger(EnterCheckTicketAnimatorHashValue);
                            }
                            else if (m_gotoPoint.m_device.DeviceType == DeviceType.ZhaJi &&
                                ((ZhaJiDevice)(m_gotoPoint.m_device)).CanOpen == false)
                            {
                                //设备不可通行, 则等待，直到设备可通行
                                yield return StartCoroutine(WaitCoroutine());
                            }
                            //屏蔽门设备
                            else if (m_gotoPoint.m_device.DeviceType == DeviceType.PingBiMen &&
                                ((PingBiMenDevice)(m_gotoPoint.m_device)).CanUp == true)
                            {
                                //设备打开之后, 可通行
                                ++m_startStepIndex;
                                tempPoint = GetMustGotoPoint(m_startStepIndex); //不可能获取到空位置点
                                tempPoint.m_prePoint = m_gotoPoint;
                                GotoDestination(tempPoint);
                            }
                            else if (m_gotoPoint.m_device.DeviceType == DeviceType.PingBiMen &&
                                ((PingBiMenDevice)(m_gotoPoint.m_device)).CanUp == false)
                            {
                                yield return StartCoroutine(WaitCoroutine());
                            }
                        }
                        else if (m_gotoPoint.IsDeviceCtrl == false)
                        {
                            ++m_startStepIndex;
                            if (m_startStepIndex == m_endStepIndex)
                            {
                                tempPoint = GetRandomExitStationPoint();
                            }
                            else
                            {
                                tempPoint = GetNoReservationPoint2RandomPointQueue();                                
                            }
                            if (tempPoint == null)
                            {
                                //当位置点被预约完的时候，去休息区, 那么站台中的Npc个数不能高于“排队点个数+对应休息区位置点个数”
                                tempPoint = GetRestAreaPoint();
                            }
                            if (tempPoint != null)
                            {
                                GotoDestination(tempPoint);
                            }
                            else
                            {
                                m_gotoPoint = null;
                            }
                        }
                    }
                }
                else
                {
                    //针对非休息区的NPC, 随时检查，目的地位置点是否被占用，如果被占用需要重新排队
                    if (m_gotoPoint != null && m_gotoPoint.IsEmpty == false && m_gotoPoint.IsRestAreaPoint == false)
                    {
                        m_gotoPoint = GetNoReservationPoint2RandomPointQueue();
                        if (m_gotoPoint == null)
                        {
                            m_gotoPoint = GetRestAreaPoint();
                        }
                        if (m_gotoPoint != null)
                        {
                            m_gotoPoint.IsReservation = true;
                            m_desPosV2.x = m_gotoPoint.PosX;
                            m_desPosV2.y = m_gotoPoint.PosZ;
                            m_navMeshAgent.SetDestination(new Vector3(m_gotoPoint.PosX, m_gotoPoint.PosY, m_gotoPoint.PosZ));
                        }
                    }
                }
            }
            yield return null;
        }
    }
































    #region 方法
    public void GotoDestination(Point desPoint)
    {
        if (desPoint == null) return;
        //当前前往的点需要被设置为已经预约
        desPoint.IsReservation = true;
        //Npc离开当前点，前往下一个目的地desPoint时，需要将当前点还原
        if (m_gotoPoint != null)
        {
            m_gotoPoint.IsEmpty = true;
            m_gotoPoint.IsReservation = false;
        }
        //再赋值目的Point为当前前往的Point m_gotoPoint
        m_gotoPoint = desPoint;
        m_desPosV2.x = m_gotoPoint.PosX;
        m_desPosV2.y = m_gotoPoint.PosZ;
        if (m_navMeshAgent != null)
        {
            m_navMeshObstacle.enabled = false;
            m_navMeshAgent.enabled = true;
            m_navMeshAgent.SetDestination(new Vector3(m_gotoPoint.PosX, m_gotoPoint.PosY, m_gotoPoint.PosZ));
        }
        if (m_animator != null)
        {
            NpcSync.SendNpcAnimation((UInt16)NpcAnimationType.Walk, NpcId, StationIndex, (UInt16)NpcActionStatus);
            m_animator.SetBool(StandUpAnimatorHashValue, false);
            m_animator.SetBool(WalkAnimatorHashValue, true);
        }
    }
    public void Wait()
    {
        if (m_animator != null)
        {
            NpcSync.SendNpcAnimation((UInt16)NpcAnimationType.StandUp, NpcId, StationIndex, (UInt16)NpcActionStatus);
            m_animator.SetBool(WalkAnimatorHashValue, false);
            m_animator.SetBool(StandUpAnimatorHashValue, true);
        }
        if (m_navMeshAgent != null)
        {
            m_navMeshAgent.enabled = false;
            m_navMeshObstacle.enabled = true;
        }
    }
    IEnumerator WaitCoroutine()
    {
        if (m_animator != null)
        {
            NpcSync.SendNpcAnimation((UInt16)NpcAnimationType.StandUp, NpcId, StationIndex, (UInt16)NpcActionStatus);
            m_animator.SetBool(WalkAnimatorHashValue, false);
            m_animator.SetBool(StandUpAnimatorHashValue, true);
        }
        if (m_navMeshAgent != null)
        {
            m_navMeshAgent.enabled = false;
            m_navMeshObstacle.enabled = true;
        }
        while (XXXXXXXFlag == false)
        {
            yield return null;
        }
    }
    public void DestroyNpc()
    {
        NpcSync.SendNpcDestroy(NpcId, StationIndex, (UInt16)NpcActionStatus);
        if (m_gotoPoint != null)
        {
            m_gotoPoint.IsReservation = false;
            m_gotoPoint.IsEmpty = true;
        }
        IsDestroy = true;
        DestroyNpc4ObjectManager();
    }
    private void DestroyNpc4ObjectManager()
    {
        ((StationModule)SingletonMgr.ModuleMgr.GetModule(StringMgr.StationModuleName)).RemoveNpcAction(StationIndex, this);
        if (ObjectManager.Instance.IsCreatedByObjectManager(this.gameObject))
        {
            ObjectManager.Instance.ReleaseGameObjectItem(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region 动画事件回调
    //进站检票动作播放完毕回调函数
    public void CheckTicketAnimationEndCallback()
    {
        m_gotoPoint.m_device.Open(() =>
        {
            //设备打开之后, 可通行
            ++m_startStepIndex;
            tempPoint = GetMustGotoPoint(m_startStepIndex); //不可能获取到空位置点
            tempPoint.m_prePoint = m_gotoPoint;
            GotoDestination(tempPoint);
        });
    }
    #endregion
}
