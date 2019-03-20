


using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

//ICommand是不具有SendNotification功能的， 但是SimpleCommand有，所以我们继承SimpleCommand. 
public class UIMainPanel_Command : SimpleCommand
{
    #region 构造函数
    public UIMainPanel_Command()
    {

    }
    #endregion

    #region 重写方法
    public override void Execute(INotification notification)
    {
        switch (notification.Name)
        {
            case EventID_Cmd.U3DClientOnLine:
                {
                    U3DClientOnLine_Callback(notification);
                    break;
                }
            case EventID_Cmd.U3DClientOffLine:
                {
                    U3DClientOffLine_Callback(notification);
                    break;
                }
            case EventID_Cmd.StationClientOnLine:
            {
                StationClientOnLine_Callback(notification);
                break;
            }
            case EventID_Cmd.StationClientOffLine:
            {
                StationClientOffLine_Callback(notification);
                break;
            }
            case EventID_Cmd.ServerStart:
                {
                    ServerStart_Callback(notification);
                    break;
                }
            case EventID_Cmd.ServerStop:
                {
                    ServerStop_Callback(notification);
                    break;
                }
            default:
                break;
        }
    }
    #endregion

    #region 方法
    private void U3DClientOnLine_Callback(INotification notification)
    {
        //参数解析
        object[] objs = notification.Body as object[];
        SendNotification(EventID_UI.U3DClientOnLine, objs, null);
    }
    private void U3DClientOffLine_Callback(INotification notification)
    {
        System.UInt16 u3dId = (System.UInt16)notification.Body;
        SendNotification(EventID_UI.U3DClientOffLine, u3dId, null);
    }
    private void StationClientOnLine_Callback(INotification notification)
    {
        SendNotification(EventID_UI.StationClientOnLine, null, null);
    }
    private void StationClientOffLine_Callback(INotification notification)
    {
        SendNotification(EventID_UI.StationClientOffLine, null, null);
    }
    private void ServerStart_Callback(INotification notification)
    {
        SendNotification(EventID_UI.ServerStart, null, null);
    }
    private void ServerStop_Callback(INotification notification)
    {
        SendNotification(EventID_UI.ServerStop, null, null);
    }
    #endregion
}
