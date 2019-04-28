using System;
using System.Collections;
using System.Collections.Generic;

public class CameraCtrl : IPacket
{
   #region 字段和属性
    public UInt16 m_stationIndex; //查看摄像头所在的站台索引
    public UInt16 m_cameraIndex; //对应站台所在摄像机的缩影
    public UInt16 m_bigScreenXDivisionCount; //一级分屏横向个数
    public UInt16 m_bigScreenYDivisionCount; //一级分屏纵向个数
    public UInt16 m_useBigScreenDivisionIndex; //使用分屏索引
    public UInt16 m_smallScreenXDivisionCount; //二级分屏横向个数
    public UInt16 m_smallScreenYDivisionCount; //二级分屏纵向个数
    public UInt16 m_useSmallScreenDivisionIndex; //使用二级分屏索引
    //数据占用字节大小
    public UInt16 Size
    {
        get { return 16; }
    }
    #endregion

    #region 构造函数
    public CameraCtrl()
    {

    }
    public CameraCtrl(byte[] bytes)
    {
        if (bytes.Length <= 0)
        {
            return;
        }
        int readIndex = 0;
        m_stationIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_cameraIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_bigScreenXDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_bigScreenYDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_useBigScreenDivisionIndex = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_smallScreenXDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_smallScreenYDivisionCount = BitConverter.ToUInt16(bytes, readIndex);
        readIndex += 2;
        m_useSmallScreenDivisionIndex = BitConverter.ToUInt16(bytes, readIndex);
    }
    #endregion

    #region 方法
    public byte[] Packet2Bytes()
    {
        byte[] stationIndexBytes = BitConverter.GetBytes(m_stationIndex);
        byte[] cameraIndexBytes = BitConverter.GetBytes(m_cameraIndex);
        byte[] bigScreenXDivisionCountBytes = BitConverter.GetBytes(m_bigScreenXDivisionCount);
        byte[] bigScreenYDivisionCountBytes = BitConverter.GetBytes(m_bigScreenYDivisionCount);
        byte[] useBigScreenDivisionIndexBytes = BitConverter.GetBytes(m_useBigScreenDivisionIndex);
        byte[] smallScreenXDivisionCount = BitConverter.GetBytes(m_smallScreenXDivisionCount);
        byte[] smallScreenYDivisionCount = BitConverter.GetBytes(m_smallScreenYDivisionCount);
        byte[] useSmallScreenDivisionIndex = BitConverter.GetBytes(m_useSmallScreenDivisionIndex);

        byte[] bytes = new byte[Size];
        int startIndex = 0;
        Array.Copy(stationIndexBytes, 0, bytes, startIndex, stationIndexBytes.Length);
        startIndex += stationIndexBytes.Length;
        Array.Copy(cameraIndexBytes, 0, bytes, startIndex, cameraIndexBytes.Length);
        startIndex += cameraIndexBytes.Length;
        Array.Copy(bigScreenXDivisionCountBytes, 0, bytes, startIndex, bigScreenXDivisionCountBytes.Length);
        startIndex += bigScreenXDivisionCountBytes.Length;
        Array.Copy(bigScreenYDivisionCountBytes, 0, bytes, startIndex, bigScreenYDivisionCountBytes.Length);
        startIndex += bigScreenYDivisionCountBytes.Length;
        Array.Copy(useBigScreenDivisionIndexBytes, 0, bytes, startIndex, useBigScreenDivisionIndexBytes.Length);
        startIndex += useBigScreenDivisionIndexBytes.Length;
        Array.Copy(smallScreenXDivisionCount, 0, bytes, startIndex, smallScreenXDivisionCount.Length);
        startIndex += smallScreenXDivisionCount.Length;
        Array.Copy(smallScreenYDivisionCount, 0, bytes, startIndex, smallScreenYDivisionCount.Length);
        startIndex += smallScreenYDivisionCount.Length;
        Array.Copy(useSmallScreenDivisionIndex, 0, bytes, startIndex, useSmallScreenDivisionIndex.Length);
        return bytes;
    }
    #endregion
}
