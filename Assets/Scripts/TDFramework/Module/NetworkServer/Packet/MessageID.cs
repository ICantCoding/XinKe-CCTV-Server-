using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageID
{
    public System.UInt16 FirstID;
    public System.UInt16 SecondID;

    public MessageID(System.UInt16 firstId, System.UInt16 secondId)
    {
        this.FirstID = firstId;
        this.SecondID = secondId;
    }
}

public class MessageIDMgr : TDFramework.Singleton<MessageIDMgr>
{
    #region 字段
    //U3D客户端登录消息
    public MessageID U3DClientLoginMessageID = new MessageID(0, 0);
    //U3D站台登录消息
    public MessageID StationClientLoginMessageID = new MessageID(0, 1);
    //NpcPosition同步消息
    public MessageID NpcPositionMessageID = new MessageID(0, 2);
    //NpcAnimation同步信令
    public MessageID NpcAnimationMessageID = new MessageID(0, 3);
    //NpcDestroy销毁信令
    public MessageID NpcDestroyMessageId = new MessageID(0, 4);
    //DeviceAction设备动作信令
    public MessageID DeviceActionMessageId = new MessageID(0, 5);
    //客户端重连信令
    public MessageID ClientRelinkMessageId = new MessageID(0, 6);
    //切割大屏信令
    public MessageID DivisionBigScreenMessageId = new MessageID(0, 7);
    //切割小屏信令
    public MessageID DivisionSmallScreenMessageId = new MessageID(0, 8);
    //屏幕绑定摄像机信令
    public MessageID ScreenBindCameraMessageId = new MessageID(0, 9);
    #endregion
}
