using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class SelectModePanelController : BasePanel
{
    public override void DidOnClick(GameObject sender)
    {
        // 如果暂停了则不再执行按钮操作
        if (IsPause) return;

        switch (sender.name)
        {
            case "Story Mode Button":
                UIManager.instance.PushPanel(UIPanelType.StoryModePanel);
                break;
            case "Return Button":
                UIManager.instance.PopPanel();
                break;
            default:
                break;
        }
    }
}
