/********************************************************************************
** Coder：    ???

** 创建时间： 2019-03-06 15:23:28

** 功能描述:  ???

** version:   v1.2.0

*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//NPC人物在场景中的行为状态
public enum NpcActionStatus
{
    EnterStationTrainUp_NpcActionStatus = 0,    //NPC 执行进站上行方向坐车
    EnterStationTrainDown_NpcActionStatus = 1,  //NPC 执行进站下行方向坐车
    ExitStationTrainUp_NpcActionStatus = 2,     //NPC 执行上行方向下车出站
    ExitStationTrainDown_NpcActionStatus = 3,	//NPC 执行下行方向下车出站
}
/*
NPC每种具体执行行为，都对应一个顺序步骤的当前位置点状态(注意初始生成的Npc可以处于任意一个位置点状态)， 如下
EnterStationTrainUp_NpcActionStatus(Npc进站上行方向坐车):
生成Npc -> EnterStation -> BuyTicket -> EnterCheckTicket -> WaitTrain_Up -> TrainUp -> Npc消失
生成Npc -> EnterStationTrainDown_NpcActionStatus(Npc进站下行方向坐车):
EnterStation -> BuyTicket -> EnterCheckTicket -> WaitTrain_Down -> TrainDown -> Npc消失
ExitStationTrainUp_NpcActionStatus(Npc出站上行方向下车):
生成Npc -> TrainUp -> NpcDownTrain_Up -> ExitCheckTicket -> ExitStation -> Npc消失
ExitStationTrainDown_NpcActionStatus(Npc出站下行方向下车):
生成Npc -> TrainDown -> NpcDownTrain_Down -> ExitCheckTicket -> ExitStation -> Npc消失
*/
public class NpcActionStatusMap
{

    public Dictionary<NpcActionStatus, PointStatus[]> m_npcActionStatusActionDict = new Dictionary<NpcActionStatus, PointStatus[]>();

    public NpcActionStatusMap()
    {
        PointStatus[] EnterStationTrainUpAction_StepArray = new PointStatus[]
        {
            PointStatus.EnterStation,
            PointStatus.BuyTicket,
            PointStatus.EnterCheckTicket,
            PointStatus.WaitTrain_Up,
            PointStatus.Train_Up
        };
        m_npcActionStatusActionDict.Add(NpcActionStatus.EnterStationTrainDown_NpcActionStatus, EnterStationTrainUpAction_StepArray);
    }
}

public class NpcAction : MonoBehaviour
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

    #region 组件字段
    private NavMeshAgent m_navMeshAgent;
    private NavMeshObstacle m_navMeshObstacle;
    private Animator m_animator;
    #endregion

    #region 状态字段
    public int m_npcId;
    //Npc所属站台Station
    public int m_stationIndex;
    //Npc行为流程
    private PointStatus[] EnterStationTrainUpAction_StepArray = new PointStatus[]
    {
        PointStatus.EnterStation,
        PointStatus.BuyTicket,
        PointStatus.EnterCheckTicket,
        PointStatus.EnterCheckTicketAfter,
        PointStatus.WaitTrain_Up,
        PointStatus.Train_Up
    };
    //Npc开始行为流程索引
    public int m_startStepIndex;
    //Npc结束行为流程索引
    public int m_endStepIndex;
    //Npc当前目的地位置点Point
    public Point m_gotoPoint;
    //缓存临时变量
    Point tempPoint = null;
    public Vector2 m_selfPosV2; //Npc自身位置X, Z
    public Vector2 m_desPosV2;  //目标位置X, Z
    //Npc是否需要销毁或回收到对象池
    public bool m_isDestroy = false;
    #endregion

    #region 属性
    public bool IsDestroy
    {
        get { return m_isDestroy; }
        set { m_isDestroy = value; }
    }
    #endregion

    #region Unity生命周期
    void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshObstacle = GetComponent<NavMeshObstacle>();
        m_animator = GetComponent<Animator>();
        m_stationIndex = 0; //测试为站台1
        //该值应该随机或始终为0, 随机值为0, 1, 2 不能为后边的了
        m_startStepIndex = 0;
        m_endStepIndex = EnterStationTrainUpAction_StepArray.Length - 1;
    }
    void Start()
    {
        StartAction();
    }
    #endregion

    #region 方法
    //获得一个随机的进站位置点
    private Point GetRandomEnterStationPoint()
    {
        int pointStatus = (int)EnterStationTrainUpAction_StepArray[m_startStepIndex];
        Point point = StationEngine.Instance.GetFirstPoint2RandomPointQueue(m_stationIndex, pointStatus);
        return point;
    }
    //获得一个当前步骤的没有预约的位置点
    public Point GetNoReservationPoint2RandomPointQueue()
    {
        Point point = StationEngine.Instance.GetNoReservationPoint2RandomPointQueue(m_stationIndex,
            (int)EnterStationTrainUpAction_StepArray[m_startStepIndex]);
        return point;
    }
    //获得一个当前步骤对应的休息区的位置点
    public Point GetRestAreaPoint()
    {
        Point point = null;
        int restStatus = (int)EnterStationTrainUpAction_StepArray[m_startStepIndex] + 1;
        //排队买票休息区
        //排队进站检票休息区
        //排队上车休息区
        if (restStatus == (int)PointStatus.BuyTicket_RestArea ||
            restStatus == (int)PointStatus.EnterCheckTicket_RestArea ||
            restStatus == (int)PointStatus.WaitTrain_Up_RestArea ||
            restStatus == (int)PointStatus.WaitTrain_Down_RestArea ||
            restStatus == (int)PointStatus.ExitCheckTicket_RestArea)
        {
            point = StationEngine.Instance.GetRandomNoReservationPointAtRestArea(m_stationIndex, restStatus);
        }
        return point;
    }
    //获得指定必达位置点，这样的点通常是EnterCheckTicketAfter, ExitCheckTicketAfter, Train_Up等类型的位置点
    public Point GetMustGotoPoint(int stepIndex)
    {
        Point point = null;
        PointStatus status = EnterStationTrainUpAction_StepArray[stepIndex];
        if ((status == PointStatus.EnterCheckTicketAfter ||
            status == PointStatus.ExitCheckTicketAfter ||
            status == PointStatus.Train_Up) && m_gotoPoint != null)
        {
            //下一个位置点，必须为特定位置点时, 跟该位置点当前状态没有任何关系，必须获取到该位置点
            point = StationEngine.Instance.GetFirstPoint(m_stationIndex,
                (int)EnterStationTrainUpAction_StepArray[stepIndex],
                m_gotoPoint.m_belongPointQueue.m_queueIndex);
        }
        return point;
    }
    #endregion


    private void StartAction()
    {
        #region 这里在获取Npc生成后，需要到达的第一个位置点
        if (PointStatus.EnterStation == EnterStationTrainUpAction_StepArray[m_startStepIndex])
        {
            //这个gotoPoint不可能为null
            tempPoint = GetRandomEnterStationPoint();
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
            Destroy(this.gameObject);
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
                            (int)EnterStationTrainUpAction_StepArray[m_startStepIndex], queueIndex);
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
                        if (m_gotoPoint.m_pointStatus == EnterStationTrainUpAction_StepArray[m_endStepIndex])
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
                            tempPoint = GetNoReservationPoint2RandomPointQueue();
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
            m_animator.SetBool(StandUpAnimatorHashValue, false);
            m_animator.SetBool(WalkAnimatorHashValue, true);
        }
    }
    public void Wait()
    {
        if (m_animator != null)
        {
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
        if (m_gotoPoint != null)
        {
            m_gotoPoint.IsReservation = false;
            m_gotoPoint.IsEmpty = true;
        }
        IsDestroy = true;
        Destroy(this.gameObject);
    }
    #endregion

    #region 动画事件回调
    //进站检票动作播放完毕回调函数
    public void EnterCheckTicketAnimationEndCallback()
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
