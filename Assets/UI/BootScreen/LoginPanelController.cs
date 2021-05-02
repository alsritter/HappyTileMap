using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFrame;
using UnityEngine;

public class LoginPanelController : BasePanel
{
    public override UIPanelType uiType => UIPanelType.LoginPanel;

    public override void DidOnClick(GameObject sender)
    {
        // 如果暂停了则不再执行按钮操作
        if (IsPause) return;

        switch (sender.name)
        {
            case "Login Button":
                // do something
                break;
            case "Close Button":
                UIManager.instance.PopPanel();
                break;
            default:
                break;
        }
    }
}