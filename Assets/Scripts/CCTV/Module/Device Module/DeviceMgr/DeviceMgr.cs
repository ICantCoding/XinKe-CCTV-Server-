/********************************************************************************
** Coder：    田山杉

** 创建时间： 2019-03-12 09:20:09

** 功能描述:  设备管理器，只对同一类设备进行管理，根据设备Id号来区别

** version:   v1.2.0

*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class DeviceMgr
{
    #region 字段
    //用于管理同类型的所有设备, Key表示设备Id
    protected Dictionary<int, Device> m_deviceDict;
    protected DeviceType m_deviceType;
    #endregion

    #region 属性
    public Dictionary<int, Device> DeviceDict
    {
        get { return m_deviceDict; }
    }
    public DeviceType DeviceType
    {
        get { return m_deviceType; }
        set { m_deviceType = value; }
    }
    #endregion

    #region 构造函数
    public DeviceMgr()
    {
        m_deviceDict = new Dictionary<int, Device>();
    }
    #endregion

    #region 方法
    public void AddDevice(Device device)
    {
        if (device == null) return;
        if (m_deviceDict.ContainsKey(device.DeviceId) == false)
        {
            m_deviceDict.Add(device.DeviceId, device);
        }
    }
    public Device GetDevice(int deviceId)
    {
        Device device = null;
        m_deviceDict.TryGetValue(deviceId, out device);
        return device;
    }
    //同步设备信息
    public virtual void SyncDeviceInfo(PlayerActor playerActor)
    {
        var enumerator = m_deviceDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Device device = enumerator.Current.Value;
            if(device != null)
                device.SyncDeviceInfo(playerActor);
        }
        enumerator.Dispose();
    }
    #endregion
}
