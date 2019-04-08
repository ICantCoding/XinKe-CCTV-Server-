using System;
using System.Collections;
using System.Collections.Generic;

public class PingBiMenMgr : DeviceMgr
{
    #region 字段
    //上行屏蔽门列表
    private List<Device> m_shangXingPingBiMenList;
    //下行屏蔽门列表
    private List<Device> m_xiaXingPingBiMenList;
    //上行屏蔽门是否打开
    private bool m_shangXingPingBiMenIsOpen = false;
    //下行屏蔽门是否打开
    private bool m_xiaXingPingBiMenIsOpen = false;
    #endregion

    #region 属性
    public List<Device> ShangXingPingBiMenList
    {
        get { return m_shangXingPingBiMenList; }
    }
    public List<Device> XiaXingPingBiMenList
    {
        get { return m_xiaXingPingBiMenList; }
    }
    public bool ShangXingPingBiMenIsOpen
    {
        get { return m_shangXingPingBiMenIsOpen; }
        set { m_shangXingPingBiMenIsOpen = value; }
    }
    public bool XiaXingPingBiMenIsOpen
    {
        get { return m_xiaXingPingBiMenIsOpen; }
        set { m_xiaXingPingBiMenIsOpen = value; }
    }
    #endregion

    #region 构造函数
    public PingBiMenMgr() : base()
    {
        m_shangXingPingBiMenList = new List<Device>();
        m_xiaXingPingBiMenList = new List<Device>();
        DeviceType = DeviceType.PingBiMen;
    }
    #endregion

    #region 方法
    public void AddDevice2ShangXingPingBiMenList(Device device)
    {
        if (device == null) return;
        m_shangXingPingBiMenList.Add(device);
    }
    public void AddDevice2XiaXingPingBiMenList(Device device)
    {
        if (device == null) return;
        m_xiaXingPingBiMenList.Add(device);
    }
    #endregion
}
