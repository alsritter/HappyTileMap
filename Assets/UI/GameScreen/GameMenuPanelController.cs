using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class GameMenuPanelController : BasePanel
{
    public override UIPanelType uiType => UIPanelType.GameMenuPanel;

    public override void DidOnClick(GameObject sender)
    {
        if (IsPause) return;

        switch (sender.name)
        {
            case "CloseButton":
                UIManager.instance.PopPanel();
                break;
            default:
                break;
        }
    }
}
