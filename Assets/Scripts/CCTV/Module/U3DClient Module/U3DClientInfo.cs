
using System;
using System.Collections;
using System.Collections.Generic;

/*
用于缓存记录，每一个U3D客户端在游戏进行中的数据
 */

/*
分屏效果样式
----------------------------------------------------------
|            |             |                             |               
|            |             |                             |    
|     1      |      2      |                             |
|            |             |                             |
|--------------------------|              2              |
|            |             |                             |
|            |             |                             |
|     3      |      4      |                             |                   
|            |             |                             |
|--------------------------|-----------------------------|
|                          |                             |
|                          |                             |
|                          |                             |
|                          |                             |
|                          |                             |
|            3             |              4              |
|                          |                             |
|                          |                             |
|                          |                             |
|                          |                             |
|--------------------------|-----------------------------|
|                          |                             |
|                          |                             |
|                          |                             |
|                          |                             |
|                          |                             |
|            5             |              6              |
|                          |                             |
|                          |                             |
|                          |                             |
|                          |                             |
----------------------------------------------------------
 */
public class CCTVScreen
{
    #region 字段
    //客户端横向分屏个数    
    protected UInt16 m_xCount;
    //客户端纵向分屏个数
    protected UInt16 m_yCount;
    #endregion

    #region 属性
    public UInt16 XCount
    {
        get { return m_xCount; }
        set { m_xCount = value; }
    }
    public UInt16 YCount
    {
        get { return m_yCount; }
        set { m_yCount = value; }
    }
    #endregion
}
public class BigScreen
{
    #region 字段
    //大块屏是否被切割成小屏
    private bool m_isDivision = false;
    //大块屏没有被切割成小屏时，所绑定的数据
    private ScreenBindData m_bigScreenBindData = new ScreenBindData();
    //大块屏横向屏个数，1表示横向没有被切割为小屏
    private UInt16 m_xCount = 1;
    //大块屏纵向屏个数，1表示纵向没有被切割为小屏
    private UInt16 m_yCount = 1;
    //大块屏被切割成小屏时, 所绑定的数据
    private Dictionary<UInt16, ScreenBindData> m_smallScreenBindDataDict = new Dictionary<ushort, ScreenBindData>();
    #endregion

    #region 属性
    public bool IsDivision
    {
        get { return m_isDivision; }
        set { m_isDivision = value; }
    }
    public ScreenBindData BigScreenBindData
    {
        get { return m_bigScreenBindData; }
        set { m_bigScreenBindData = value; }
    }
    public Dictionary<UInt16, ScreenBindData> SmallScreenBindDataDict
    {
        get { return m_smallScreenBindDataDict; }
    }
    #endregion

    #region 方法
    public void ClearSmallScreenBindData()
    {
        m_smallScreenBindDataDict.Clear();
    }
    public void AddSmallScreenBindData(UInt16 smallScreenIndex, ScreenBindData data)
    {
        if (System.Object.ReferenceEquals(data, null)) return;
        m_smallScreenBindDataDict.Add(smallScreenIndex, data);
    }
    #endregion
}
public class ScreenBindData
{
    #region 字段
    //是否有绑定的数据, true表示没有绑定数据，false表示绑定了数据
    private bool m_isEmpty = true;
    private UInt16 m_stationIndex;
    private UInt16 m_cameraIndex;
    #endregion

    #region 属性
    public bool IsEmpty
    {
        get { return m_isEmpty; }
        set { m_isEmpty = value; }
    }
    public UInt16 StationIndex
    {
        get { return m_stationIndex; }
        set { m_stationIndex = value; }
    }
    public UInt16 CameraIndex
    {
        get { return m_cameraIndex; }
        set { m_cameraIndex = value; }
    }
    #endregion
}

public class U3DClientInfo
{
    #region 字段
    private CCTVScreen m_cctvScreen;
    //保存大快屏信息的字典
    private Dictionary<UInt16, BigScreen> m_bigScreenDict;
    //当前客户端CCTV在查看的站台列表
    private List<UInt16> m_staionIndexList = new List<UInt16>();
    //保存大块屏查看的站台
    private Dictionary<UInt16, List<UInt16>> m_stationIndexDict = new Dictionary<ushort, List<ushort>>();
    #endregion

    #region 属性

    #endregion

    #region 构造函数
    public U3DClientInfo()
    {
        m_cctvScreen = new CCTVScreen();
        m_bigScreenDict = new Dictionary<ushort, BigScreen>();
    }
    #endregion

    #region 方法
    //切割大屏
    public void ScreenDivision(UInt16 x, UInt16 y)
    {
        if (x == 0 || y == 0) return;
        m_cctvScreen.XCount = x;
        m_cctvScreen.YCount = y;

        m_bigScreenDict.Clear();
        m_stationIndexDict.Clear();

        int bigScreenCount = x * y;
        List<UInt16> list = null;
        for (UInt16 i = 0; i < bigScreenCount; ++i)
        {
            BigScreen bigScreen = new BigScreen();
            m_bigScreenDict.Add(i, bigScreen);

            list = new List<UInt16>();
            list.Add(9999); //起到扩容的作用， 目前针对大屏只含有一个元素
            m_stationIndexDict.Add(i, list);
        }
    }
    //切割小屏
    public void SmallScreenDivision(UInt16 bigScreenIndex, UInt16 x, UInt16 y)
    {
        if (x == 0 || y == 0) return;
        if (x == 1 && y == 1) return;
        BigScreen bigScreen = m_bigScreenDict[bigScreenIndex];
        bigScreen.IsDivision = true;

        List<UInt16> list = m_stationIndexDict[bigScreenIndex];
        list.Clear();
        int smallScreenCount = x * y;
        for (UInt16 i = 0; i < smallScreenCount; ++i)
        {
            ScreenBindData data = new ScreenBindData();
            bigScreen.AddSmallScreenBindData(i, data);
            list.Add(9999); //起到给List扩容的作用， 目前对大屏含小屏的情况，有x*y个元素
        }
    }
    //屏幕绑定CameraIndex和站台Index, 大屏索引，大屏中小屏索引, 索引从1开始
    public void ScreenBindCamera(UInt16 bigScreenIndex, UInt16 smallScreenIndex, UInt16 stationIndex, UInt16 cameraIndex)
    {
        ScreenBindData data = null;
        if (smallScreenIndex == 0)
        {
            //表示Camera画面绑定到大屏上
            List<UInt16> list = m_stationIndexDict[bigScreenIndex];
            list[0] = stationIndex;
        }
        else
        {
            //表示Camera画面绑定到某个大屏的小屏上
            data = m_bigScreenDict[bigScreenIndex].SmallScreenBindDataDict[smallScreenIndex];
            List<UInt16> list = m_stationIndexDict[bigScreenIndex];
            if(list.Count >= smallScreenIndex)
            {
                list[smallScreenIndex - 1] = stationIndex;
            }
        }
        if (data != null)
        {
            data.StationIndex = stationIndex;
            data.CameraIndex = cameraIndex;
        }
    }
    #endregion
}
